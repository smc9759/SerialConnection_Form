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
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                _serialComm.OpenPort();
                ChangeButtonColor(_serialComm.IsPortOpen); // Update button color
                _statusTimer.Start(); // Start the timer to check port status
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                _serialComm.ClosePort();
                ChangeButtonColor(_serialComm.IsPortOpen); // Update button color
                _statusTimer.Stop(); // Stop the timer
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OnStatusTimerTick(object sender, EventArgs e)
        {
            // Timer Tick event handler
            // Update button color based on the port status
            ChangeButtonColor(_serialComm.IsPortOpen);
        }

        private void ChangeButtonColor(bool isOpen)
        {
            if (isOpen)
            {
                btn_isportopen.StateCommon.Back.Color1 = Color.Orange;
            }
            else
            {
                btn_isportopen.StateCommon.Back.Color1 = Color.LightGray; // Default color
            }
        }


    }
}
