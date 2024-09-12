using SerialForm2;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace SerialComm
{
    public class SerialCommunication
    {
        private ConcurrentDictionary<string, SerialPort> activePorts = new ConcurrentDictionary<string, SerialPort>();
        private readonly DatabaseManager _databaseManager;
        // 6개의 큐를 사용
        private ConcurrentQueue<string> queueForS001 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueForS002 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueForS003 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueForS004 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueForS005 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> queueForS006 = new ConcurrentQueue<string>();

        public SerialCommunication()
        {
            _databaseManager = new DatabaseManager();
        }

        private void LogMessage(string message)
        {
            try
            {
                // 현재 날짜와 시간을 기반으로 로그 파일 이름 생성
                string year = DateTime.Now.ToString("yyyy");
                string month = DateTime.Now.ToString("MM");
                string day = DateTime.Now.ToString("dd");
                string hour = DateTime.Now.ToString("HH");

                string logFileName = $"SerialLog_{year}{month}{day}{hour}.txt";
                string logDirectoryPath = Variable.LogPath; // 로그 파일이 저장될 기본 디렉터리 경로
                string fullLogFilePath = Path.Combine(logDirectoryPath, logFileName); // 로그 파일의 전체 경로

                // 로그 파일 경로의 디렉터리가 존재하는지 확인하고, 없다면 생성
                if (!Directory.Exists(logDirectoryPath))
                {
                    Directory.CreateDirectory(logDirectoryPath);
                }
                // 로그 메시지를 동기적으로 파일에 추가
                using (StreamWriter writer = new StreamWriter(fullLogFilePath, append: true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                // 예외 발생 시 기본적으로 무시하지만, 필요 시 다른 처리 가능
                Console.WriteLine($"로그 기록 실패: {ex.Message}");
            }
        }

        public void Start()
        {
            try
            {
                LogMessage("시리얼 포트 모니터링을 시작합니다. 종료하려면 [q]를 누르세요.");

                var monitorThread = new Thread(() => MonitorSerialPorts());
                var processS001Thread = new Thread(() => ProcessQueue(queueForS001, "S_001"));
                var processS002Thread = new Thread(() => ProcessQueue(queueForS002, "S_002"));
                var processS003Thread = new Thread(() => ProcessQueue(queueForS003, "S_003"));
                var processS004Thread = new Thread(() => ProcessQueue(queueForS004, "S_004"));
                var processS005Thread = new Thread(() => ProcessQueue(queueForS005, "S_005"));
                var processS006Thread = new Thread(() => ProcessQueue(queueForS006, "S_006"));

                monitorThread.Start();
                processS001Thread.Start();
                processS002Thread.Start();
                processS003Thread.Start();
                processS004Thread.Start();
                processS005Thread.Start();
                processS006Thread.Start();

                monitorThread.Join();
                processS001Thread.Join();
                processS002Thread.Join();
                processS003Thread.Join();
                processS004Thread.Join();
                processS005Thread.Join();
                processS006Thread.Join();

                // 모든 포트를 닫음
                foreach (var port in activePorts.Values)
                {
                    try
                    {
                        port.Close();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"포트 {port.PortName} 닫는 중 오류 발생: {ex.Message}");
                    }
                }

                LogMessage("포트 모니터링이 종료되었습니다.");
            }
            catch (Exception ex)
            {
                LogMessage($"시리얼 통신 중 예외 발생: {ex.Message}");
            }
            finally
            {
                // 모든 포트를 닫음 (예외 발생 후에도)
                foreach (var port in activePorts.Values)
                {
                    if (port.IsOpen)
                    {
                        try
                        {
                            port.Close();
                        }
                        catch (Exception ex)
                        {
                            LogMessage($"포트 {port.PortName} 닫는 중 오류 발생 (finally): {ex.Message}");
                        }
                    }
                }
            }
        }

        #region SerialCommunication

        private void MonitorSerialPorts()
        {
            while (true)
            {
                try
                {
                    var availablePort = SerialPort.GetPortNames();

                    // 새로운 포트 열기
                    OpenPort(availablePort);

                    // 비활성 포트 닫기
                    CloseInactivePort(availablePort);

                    // 포트 다시 연결
                    ReconnectPort(availablePort);

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    LogMessage("포트 모니터링 중 오류 발생: " + ex.Message);
                    Thread.Sleep(1);
                }
            }
        }

        private void OpenPort(string[] availablePorts)
        {
            foreach (var portName in availablePorts)
            {
                if (!activePorts.ContainsKey(portName))
                {
                    try
                    {
                        var serialPort = new SerialPort(portName, 115200);
                        serialPort.Open();
                        activePorts[portName] = serialPort;
                        LogMessage($"{portName} 포트가 열렸습니다.");

                        // 데이터 수신을 위한 스레드 시작
                        new Thread(() => ReceiveData(serialPort)).Start();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"{portName} 포트 열기 중 오류 발생: {ex.Message}");
                    }
                }
            }
        }

        private void CloseInactivePort(string[] availablePorts)
        {
            var activePortsKeys = activePorts.Keys.ToList();
            foreach (var portName in activePortsKeys)
            {
                var port = activePorts[portName];
                if (!availablePorts.Contains(portName) || !port.IsOpen)
                {
                    LogMessage($"{portName} 포트 연결이 끊겼습니다.");
                    try
                    {
                        port.Close();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"{portName} 포트 닫는 중 오류 발생: {ex.Message}");
                    }
                    finally
                    {
                        activePorts.TryRemove(portName, out _); // Remove 대신 TryRemove 사용
                    }
                }
            }
        }

        private void ReconnectPort(string[] availablePorts)
        {
            foreach (var portName in availablePorts)
            {
                if (!activePorts.ContainsKey(portName))
                {
                    try
                    {
                        var serialPort = new SerialPort(portName, 115200);
                        serialPort.Open();
                        activePorts[portName] = serialPort;
                        LogMessage($"{portName} 포트를 다시 연결했습니다.");

                        // 데이터 수신을 위한 스레드 시작
                        new Thread(() => ReceiveData(serialPort)).Start();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"{portName} 포트를 다시 여는 중 오류 발생: {ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region Create Folder & SQLiteDB

        private void ProcessQueue(ConcurrentQueue<string> queue, string prefix)
        {
            while (true)
            {
                if (queue.TryDequeue(out string data))
                {
                    try
                    {
                        string dbFilePath = _databaseManager.CreateFolder(prefix);
                        _databaseManager.AddDBTable(dbFilePath);
                        _databaseManager.AddDataToDB(dbFilePath, prefix, data);
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"데이터 처리 중 오류 발생 ({prefix}): " + ex.Message);
                    }
                }

                Thread.Sleep(500);
            }
        }

        #endregion

        #region Data Receive & EnQueue

        private void ReceiveData(SerialPort port)
        {
            StringBuilder buffer = new StringBuilder();

            while (port.IsOpen)
            {
                try
                {
                    string receivedData = ReadData(port);
                    buffer.Append(receivedData);

                    while (buffer.ToString().Contains("\n"))
                    {
                        string completeSegment = ExtractCompleteSegment(buffer);
                        if (!string.IsNullOrEmpty(completeSegment))
                        {
                            EnqueueSegment(completeSegment);
                        }
                    }
                }
                catch (TimeoutException)
                {
                    // TimeoutException 처리 로직이 필요하다면 추가
                }
                catch (Exception ex)
                {
                    LogMessage($"{port.PortName} 데이터 수신 중 오류 발생: " + ex.Message);
                }

                Thread.Sleep(1);
            }
        }

        private string ReadData(SerialPort port)
        {
            return port.ReadExisting();
        }

        private void EnqueueSegment(string completeSegment)
        {
            if (completeSegment.StartsWith("S_001"))
            {
                queueForS001.Enqueue(completeSegment);
            }
            else if (completeSegment.StartsWith("S_002"))
            {
                queueForS002.Enqueue(completeSegment);
            }
            else if (completeSegment.StartsWith("S_003"))
            {
                queueForS003.Enqueue(completeSegment);
            }
            else if (completeSegment.StartsWith("S_004"))
            {
                queueForS004.Enqueue(completeSegment);
            }
            else if (completeSegment.StartsWith("S_005"))
            {
                queueForS005.Enqueue(completeSegment);
            }
            else if (completeSegment.StartsWith("S_006"))
            {
                queueForS006.Enqueue(completeSegment);
            }
        }

        private string ExtractCompleteSegment(StringBuilder buffer)
        {
            int newlineIndex = buffer.ToString().IndexOf('\n');
            if (newlineIndex >= 0)
            {
                string segment = buffer.ToString(0, newlineIndex + 1);
                buffer.Remove(0, newlineIndex + 1);
                return segment;
            }
            return null;
        }

        #endregion
    }
}

