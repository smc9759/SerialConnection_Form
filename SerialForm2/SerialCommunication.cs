using System;
using System.IO.Ports;

namespace SerialComm
{
    public class SerialCommunication
    {
        private SerialPort _serialPort;

        public SerialCommunication()
        {
            _serialPort = new SerialPort("COM6", 115200);
        }

        public bool IsPortOpen
        {
            get { return _serialPort.IsOpen; }
        }

        public void OpenPort()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log error or show message)
                throw new InvalidOperationException($"Error opening port: {ex.Message}", ex);
            }
        }

        public void ClosePort()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log error or show message)
                throw new InvalidOperationException($"Error closing port: {ex.Message}", ex);
            }
        }
    }
}