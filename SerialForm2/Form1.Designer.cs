namespace SerialForm2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtReceivedData = new ComponentFactory.Krypton.Toolkit.KryptonListBox();
            this._statusTimer = new System.Windows.Forms.Timer(this.components);
            this.btn_isportopen = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(101, 72);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(143, 137);
            this.kryptonButton1.TabIndex = 0;
            this.kryptonButton1.Values.Text = "Open";
            this.kryptonButton1.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(288, 72);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(146, 137);
            this.kryptonButton2.TabIndex = 1;
            this.kryptonButton2.Values.Text = "Close";
            this.kryptonButton2.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // txtReceivedData
            // 
            this.txtReceivedData.Location = new System.Drawing.Point(492, 12);
            this.txtReceivedData.Name = "txtReceivedData";
            this.txtReceivedData.Size = new System.Drawing.Size(257, 426);
            this.txtReceivedData.TabIndex = 3;
            // 
            // btn_isportopen
            // 
            this.btn_isportopen.Location = new System.Drawing.Point(177, 258);
            this.btn_isportopen.Name = "btn_isportopen";
            this.btn_isportopen.Size = new System.Drawing.Size(176, 108);
            this.btn_isportopen.TabIndex = 2;
            this.btn_isportopen.Values.Text = "Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtReceivedData);
            this.Controls.Add(this.btn_isportopen);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.kryptonButton1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private ComponentFactory.Krypton.Toolkit.KryptonListBox txtReceivedData;
        private System.Windows.Forms.Timer _statusTimer;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btn_isportopen;
    }
}

