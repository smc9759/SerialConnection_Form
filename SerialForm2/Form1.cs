using ComponentFactory.Krypton.Toolkit;
using SerialComm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SerialForm2
{
    public partial class Form1 : KryptonForm
    {
        //private List<string> loadedFiles = new List<string>(); // 이미 로드된 파일 목록
        private Dictionary<string, long> lastReadRowIds = new Dictionary<string, long>(); // 각 시리즈의 마지막으로 읽은 ROWID 저장
        private Dictionary<string, DateTime> lastReceivedTimes = new Dictionary<string, DateTime>(); // 각 시리즈의 마지막 수신 시간 저장
        private Dictionary<string, int> noDataCount = new Dictionary<string, int>();
        private int timeoutSeconds = 5; // 데이터 미수신 타임아웃 기준 (초)
        private Dictionary<string, string> lastFilePaths = new Dictionary<string, string>();
        private SerialCommunication _serialComm;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            InitializeLastReceivedTimes();
            _serialComm = new SerialCommunication();

            // Initialize and configure the timer
            _statusTimer.Interval = 1000; // Check every second
            _statusTimer.Start();

            StartBackgroundTasks();
        }
        private async void _statusTimer_Tick(object sender, EventArgs e)
        {
            // Timer Tick event handler
            DateTime now = DateTime.Now;
            labelTime.Text = now.ToString("yyyy-MM-dd HH:mm:ss");

            // 1초마다 최신 데이터베이스 파일을 체크하고 차트 업데이트
            await CheckForNewDbFilesAsync();


            // 타임아웃 체크
            CheckTimeouts();
        }



        //        btn_isportopen.StateCommon.Back.Color1 = Color.LightGray; // Default color
        private static async void StartBackgroundTasks()
        {
            // 비동기 시리얼 통신 시작
            await StartSerialCommunicationAsync();
        }

        // 비동기 시리얼 통신 메서드
        private static async Task StartSerialCommunicationAsync()
        {
            try
            {
                // 시리얼 통신 인스턴스 생성 및 비동기 시작
                SerialCommunication serialCommunication = new SerialCommunication();
                await serialCommunication.StartAsync(); // 시리얼 통신 비동기 작업 시작
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in serial communication: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void CheckTimeouts()
        {
            DateTime now = DateTime.Now;
            foreach (var kvp in lastReceivedTimes)
            {
                string seriesName = kvp.Key;
                DateTime lastReceivedTime = kvp.Value;
                if ((now - lastReceivedTime).TotalSeconds > timeoutSeconds)
                {
                    // 타임아웃 발생, 버튼을 빨간색으로 설정
                    //SetButtonColor(seriesName, Color.Red);
                    SetButtonColor(seriesName, Color.FromArgb(255, 53, 94)); // Lime Green 색상
                }
                else
                {
                    SetButtonColor(seriesName, Color.FromArgb(144, 238, 144));
                    //SetButtonColor(seriesName, Color.FromArgb(50, 205, 50)); 
                }
            }
        }

        /// <summary>
        /// 버튼 색깔 
        /// </summary>
        /// <param name="seriesName"></param>
        /// <param name="color"></param>


        private void SetButtonColor(string seriesName, Color color)
        {
            // 모든 버튼에 대해 동일한 색상 설정
            if (seriesName == "S_001")
            {
                kryptonButton1_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton1_Model1_ST.StateCommon.Back.Color2 = color; // 그라데이션 제거를 위해 동일한 색상으로 설정
            }
            else if (seriesName == "S_002")
            {
                kryptonButton2_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton2_Model1_ST.StateCommon.Back.Color2 = color;
            }
            else if (seriesName == "S_003")
            {
                kryptonButton3_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton3_Model1_ST.StateCommon.Back.Color2 = color;
            }
            else if (seriesName == "S_004")
            {
                kryptonButton4_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton4_Model1_ST.StateCommon.Back.Color2 = color;
            }
            else if (seriesName == "S_005")
            {
                kryptonButton5_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton5_Model1_ST.StateCommon.Back.Color2 = color;
            }
            else if (seriesName == "S_006")
            {
                kryptonButton6_Model1_ST.StateCommon.Back.Color1 = color;
                kryptonButton6_Model1_ST.StateCommon.Back.Color2 = color;
            }
        }


        private void InitializeChart()
        {
            try
            {
                chart1.ChartAreas[0].AxisX.Minimum = Variable.Model1_Chart_Minimum_X;
                chart1.ChartAreas[0].AxisX.Maximum = Variable.Model1_Chart_Maximum_X;
                chart1.ChartAreas[0].AxisY.Minimum = Variable.Model1_Chart_Minimum_Y;
                chart1.ChartAreas[0].AxisY.Maximum = Variable.Model1_Chart_Maximum_Y;

                chart1.Series[0].Name = Variable.Model1_Sensor_Name1;
                chart1.Series[0].Color = Color.Red; // Series0: 빨강

                chart1.Series[1].Name = Variable.Model1_Sensor_Name2;
                chart1.Series[1].Color = Color.Orange; // Series1: 주황

                chart1.Series[2].Name = Variable.Model1_Sensor_Name3;
                chart1.Series[2].Color = Color.Yellow; // Series2: 노랑

                chart1.Series[3].Name = Variable.Model1_Sensor_Name4;
                chart1.Series[3].Color = Color.Green; // Series3: 초록

                chart1.Series[4].Name = Variable.Model1_Sensor_Name5;
                chart1.Series[4].Color = Color.Blue; // Series4: 파랑

                chart1.Series[5].Name = Variable.Model1_Sensor_Name6;
                chart1.Series[5].Color = Color.Purple; // Series5: 남색

            }
            catch (Exception ex)
            {
                //string str_Msg = $"{MethodBase.GetCurrentMethod().Name} - {ex.Message}";
                //Variable.WriteLog(str_Msg);
            }
        }

        private void InitializeLastReceivedTimes()
        {
            string[] seriesNames = { "S_001", "S_002", "S_003", "S_004", "S_005", "S_006" };
            foreach (string seriesName in seriesNames)
            {
                lastReceivedTimes[seriesName] = DateTime.Now;
            }
        }


        /// <summary>
        /// DB 파일 확인 
        /// </summary>
        /// <returns></returns>

        private async Task CheckForNewDbFilesAsync()
        {
            string latestFolderPath = FindLatestFolderPath(Variable.baseFolderPath);
            if (string.IsNullOrEmpty(latestFolderPath))
            {
                return; // 최신 폴더를 찾지 못한 경우
            }

            string[] seriesNames = { "S_001", "S_002", "S_003", "S_004", "S_005", "S_006" };

            // 현재 날짜와 시간을 사용하여 파일 이름을 동적으로 생성
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string hour = DateTime.Now.ToString("HH");

            // baseFolderPath를 사용하여 동적으로 경로 설정
            string folderPath = Path.Combine(Variable.baseFolderPath, $"SVMU_{year}", $"SVMU_{year}{month}", $"SVMU_{year}{month}{day}");

            for (int j = 0; j < seriesNames.Length; j++)
            {
                string seriesName = seriesNames[j];
                string dbFilePath = Path.Combine(folderPath, $"SVMU_{year}{month}{day}_{hour}_{seriesName}.db");
                /*
                if (!File.Exists(dbFilePath) || loadedFiles.Contains(dbFilePath))
                {
                    // 파일이 없거나 이미 로드된 파일이면 건너뜀
                    continue;
                }

                await LoadDataFromFileAsync(dbFilePath, seriesName);
                loadedFiles.Add(dbFilePath); // 로드된 파일로 추가
                */
                if (!File.Exists(dbFilePath))
                {
                    continue;
                }
                // 파일 변경 감지: 기존 파일 경로와 다르면 초기화
                if (!lastFilePaths.ContainsKey(seriesName) || lastFilePaths[seriesName] != dbFilePath)
                {
                    // 새로운 파일이 로드되었을 때 데이터 초기화
                    lastReadRowIds[seriesName] = 0;
                    chart1.Series[seriesName].Points.Clear(); // 그래프 초기화

                    // 새로운 파일 경로 업데이트
                    lastFilePaths[seriesName] = dbFilePath;
                }
                //MessageBox.Show($"Tlqkf {dbFilePath}");
                // 데이터 파일 로드
                await LoadDataFromFileAsync(dbFilePath, seriesName);
            }
        }


        private string FindLatestFolderPath(string baseFolder)
        {
            try
            {
                // SVMU_YYYY 형식의 최신 폴더 찾기
                var yearFolders = Directory.GetDirectories(baseFolder, "SVMU_*", SearchOption.TopDirectoryOnly)
                                           .OrderByDescending(d => d) // 내림차순으로 정렬 (최신 폴더 우선)
                                           .ToList();

                if (!yearFolders.Any())
                    return null; // 연도 폴더를 찾지 못한 경우

                // 최신 연도 폴더 선택
                string latestYearFolder = yearFolders.First();

                // SVMU_MM 형식의 최신 폴더 찾기
                var monthFolders = Directory.GetDirectories(latestYearFolder, "SVMU_*", SearchOption.TopDirectoryOnly)
                                            .OrderByDescending(d => d) // 내림차순으로 정렬 (최신 폴더 우선)
                                            .ToList();

                if (!monthFolders.Any())
                    return null; // 월 폴더를 찾지 못한 경우

                // 최신 월 폴더 반환
                //return monthFolders.First();

                // 최신 월 폴더 선택
                string latestMonthFolder = monthFolders.First();

                // SVMU_DD 형식의 최신 일 폴더 찾기
                var dayFolders = Directory.GetDirectories(latestMonthFolder, "SVMU_*", SearchOption.TopDirectoryOnly)
                                          .OrderByDescending(d => d) // 내림차순으로 정렬 (최신 일 폴더 우선)
                                          .ToList();

                if (!dayFolders.Any())
                    return null; // 일 폴더를 찾지 못한 경우

                // 최신 일 폴더 반환
                return dayFolders.First();

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error finding latest folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // 주어진 데이터베이스 파일에서 데이터를 로드하고 차트를 업데이트하는 비동기 메서드
        private async Task LoadDataFromFileAsync(string dbFilePath, string seriesName)
        {
            try
            {

                string year = DateTime.Now.ToString("yyyy");
                string month = DateTime.Now.ToString("MM");
                string day = DateTime.Now.ToString("dd");
                string hour = DateTime.Now.ToString("HH");

                // baseFolderPath를 사용하여 동적으로 경로 설정
                string folderPath = Path.Combine(Variable.baseFolderPath, $"SVMU_{year}", $"SVMU_{year}{month}", $"SVMU_{year}{month}{day}");


                string connectionString = $"Data Source={dbFilePath};Version=3;";
                string query = "SELECT ROWID, Data FROM DataLog ORDER BY ROWID DESC LIMIT 1";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = (SQLiteDataReader)await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                long rowId = reader.GetInt64(0);
                                string str_SensorData = reader.GetString(1);
                                string[] stringValues = str_SensorData.Split(',');
                                // ROWID 초기화 조건 추가
                                // 새로운 파일이면 lastReadRowIds 초기화 상태에서 시작
                                if (!lastReadRowIds.ContainsKey(seriesName) || rowId > lastReadRowIds[seriesName])
                                {
                                    lastReadRowIds[seriesName] = rowId; // 마지막으로 읽은 ROWID 갱신

                                    List<double> values = new List<double>();
                                    foreach (var stringValue in stringValues)
                                    {
                                        if (double.TryParse(stringValue, out double parsedValue))
                                        {
                                            values.Add(parsedValue);
                                        }
                                    }

                                    if (chart1.Series.IndexOf(seriesName) == -1)
                                    {
                                        chart1.Series.Add(seriesName);
                                        chart1.Series[seriesName].ChartType = SeriesChartType.Line;
                                    }

                                    chart1.Series[seriesName].Points.Clear(); // 그래프 초기화
                                    for (int i = 0; i < values.Count; i++)
                                    {
                                        chart1.Series[seriesName].Points.AddXY(i + 1, values[i]);
                                    }
                                    chart1.Series[seriesName].Enabled = true;
                                    lastReceivedTimes[seriesName] = DateTime.Now; // 마지막 수신 시간 갱신

                                    // 데이터 수신 시 미수신 카운트를 초기화
                                    if (noDataCount.ContainsKey(seriesName))
                                    {
                                        noDataCount[seriesName] = 0;
                                    }
                                }

                                else
                                {
                                    // 데이터가 수신되지 않은 경우
                                    if (!noDataCount.ContainsKey(seriesName))
                                    {
                                        noDataCount[seriesName] = 0;
                                    }

                                    noDataCount[seriesName]++; // 미수신 카운트 증가

                                    if (noDataCount[seriesName] > 2)
                                    {
                                        // 2회 이상 데이터가 수신되지 않았을 때 시리즈 비활성화
                                        if (chart1.Series.IndexOf(seriesName) != -1)
                                        {
                                            chart1.Series[seriesName].Enabled = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
