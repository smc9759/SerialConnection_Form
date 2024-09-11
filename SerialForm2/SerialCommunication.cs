using SerialForm2;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // 로그 메시지를 비동기로 파일에 기록하는 메서드

        // 로그 메시지를 비동기로 파일에 기록하는 메서드
        private async Task LogMessageAsync(string message)
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
                // 로그 메시지를 비동기적으로 파일에 추가
                using (StreamWriter writer = new StreamWriter(fullLogFilePath, append: true))
                {
                    await writer.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
            catch (Exception ex)
            {
                await LogMessageAsync(ex.Message);
                // 파일에 로그 기록이 실패한 경우, 기본적으로 예외를 무시
                // 필요에 따라 다른 예외 처리 로직을 추가할 수 있습니다.
            }
        }

        public async Task StartAsync()
        {
            try
            {
                await LogMessageAsync("시리얼 포트 모니터링을 시작합니다. 종료하려면 [q]를 누르세요.");

                var monitorTask = MonitorSerialPortsAsync();
                var processS001Task = ProcessQueueAsync(queueForS001, "S_001");
                var processS002Task = ProcessQueueAsync(queueForS002, "S_002");
                var processS003Task = ProcessQueueAsync(queueForS003, "S_003");
                var processS004Task = ProcessQueueAsync(queueForS004, "S_004");
                var processS005Task = ProcessQueueAsync(queueForS005, "S_005");
                var processS006Task = ProcessQueueAsync(queueForS006, "S_006");



                // 모든 포트를 닫음
                foreach (var port in activePorts.Values)
                {
                    try
                    {
                        port.Close();
                    }
                    catch (Exception ex)
                    {
                        await LogMessageAsync($"포트 {port.PortName} 닫는 중 오류 발생: {ex.Message}");
                    }
                }

                await LogMessageAsync("포트 모니터링이 종료되었습니다.");

                // 모든 작업 완료 대기
                await Task.WhenAll(monitorTask, processS001Task, processS002Task, processS003Task, processS004Task, processS005Task, processS006Task);
            }
            catch (Exception ex)
            {
                await LogMessageAsync($"시리얼 통신 중 예외 발생: {ex.Message}");
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
                            await LogMessageAsync($"포트 {port.PortName} 닫는 중 오류 발생 (finally): {ex.Message}");
                        }
                    }
                }
            }
        }

        #region SerialCommunication

        private async Task MonitorSerialPortsAsync()
        {
            while (true)
            {
                try
                {
                    var availablePort = SerialPort.GetPortNames();

                    // 새로운 포트 열기
                    await OpenPort(availablePort);

                    // 비활성 포트 닫기
                    await CloseInactivePort(availablePort);

                    // 포트 다시 연결
                    await ReconnectPort(availablePort);

                    await Task.Delay(5000);
                }
                catch (Exception ex)
                {
                    await LogMessageAsync("포트 모니터링 중 오류 발생: " + ex.Message);
                    await Task.Delay(5000);
                }
            }
        }

        private async Task OpenPort(string[] availablePorts)
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
                        await LogMessageAsync($"{portName} 포트가 열렸습니다.");

                        _ = ReceiveData(serialPort);
                    }
                    catch (Exception ex)
                    {
                        await LogMessageAsync($"{portName} 포트 열기 중 오류 발생: {ex.Message}");
                    }
                }
            }
        }

        private async Task CloseInactivePort(string[] availablePorts)
        {
            var activePortsKeys = activePorts.Keys.ToList();
            foreach (var portName in activePortsKeys)
            {
                var port = activePorts[portName];
                if (!availablePorts.Contains(portName) || !port.IsOpen)
                {
                    await LogMessageAsync($"{portName} 포트 연결이 끊겼습니다.");
                    try
                    {
                        port.Close();
                    }
                    catch (Exception ex)
                    {
                        await LogMessageAsync($"{portName} 포트 닫는 중 오류 발생: {ex.Message}");
                    }
                    finally
                    {
                        activePorts.TryRemove(portName, out _); // Remove 대신 TryRemove 사용
                    }
                }
            }
        }

        private async Task ReconnectPort(string[] availablePorts)
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
                        await LogMessageAsync($"{portName} 포트를 다시 연결했습니다.");

                        _ = ReceiveData(serialPort);
                    }
                    catch (Exception ex)
                    {
                        await LogMessageAsync($"{portName} 포트를 다시 여는 중 오류 발생: {ex.Message}");
                    }
                }
            }
        }

        #endregion

        #region Create Folder & SQLiteDB

        private async Task ProcessQueueAsync(ConcurrentQueue<string> queue, string prefix)
        {
            while (true)
            {
                if (queue.TryDequeue(out string data))
                {
                    try
                    {
                        string dbFilePath = _databaseManager.CreateFolder(prefix);
                        await _databaseManager.AddDBTableAsync(dbFilePath);
                        await _databaseManager.AddDataToDBAsync(dbFilePath, prefix, data);
                    }
                    catch (Exception ex)
                    {
                        await LogMessageAsync($"데이터 처리 중 오류 발생 ({prefix}): " + ex.Message);
                    }
                }

                await Task.Delay(500);
            }
        }

        #endregion

        #region Data Receive & EnQueue

        private async Task ReceiveData(SerialPort port)
        {
            StringBuilder buffer = new StringBuilder();

            while (port.IsOpen)
            {
                try
                {
                    string receivedData = await ReadData(port);
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
                    await LogMessageAsync($"{port.PortName} 데이터 수신 중 오류 발생: " + ex.Message);
                }

                await Task.Delay(1);
            }
        }

        private async Task<string> ReadData(SerialPort port)
        {
            return await Task.Run(() => port.ReadExisting());
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