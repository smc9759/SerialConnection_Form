using ComponentFactory.Krypton.Toolkit;
using SerialComm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialForm2
{
    public partial class Form1 : KryptonForm
    {
        private SerialCommunication _serialComm;

        public Form1()
        {
            InitializeComponent();
            _serialComm = new SerialCommunication();

            // Initialize and configure the timer
            _statusTimer = new Timer();
            _statusTimer.Interval = 1000; // Check every second
            _statusTimer.Tick += OnStatusTimerTick;

            StartBackgroundTasks();
        }


        private void OnStatusTimerTick(object sender, EventArgs e)
        {
            // Timer Tick event handler

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


    }
}
