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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this._statusTimer = new System.Windows.Forms.Timer(this.components);
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.kryptonButton1_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton3_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton4_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton5_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton6_Model1_ST = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.labelTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // _statusTimer
            // 
            this._statusTimer.Tick += new System.EventHandler(this._statusTimer_Tick);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(154, 108);
            this.chart1.Name = "chart1";
            this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "Series4";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "Series5";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "Series6";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Series.Add(series4);
            this.chart1.Series.Add(series5);
            this.chart1.Series.Add(series6);
            this.chart1.Size = new System.Drawing.Size(940, 693);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // kryptonButton1_Model1_ST
            // 
            this.kryptonButton1_Model1_ST.Location = new System.Drawing.Point(44, 59);
            this.kryptonButton1_Model1_ST.Name = "kryptonButton1_Model1_ST";
            this.kryptonButton1_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton1_Model1_ST.TabIndex = 1;
            this.kryptonButton1_Model1_ST.Values.Text = "kryptonButton1";
            // 
            // kryptonButton2_Model1_ST
            // 
            this.kryptonButton2_Model1_ST.Location = new System.Drawing.Point(44, 143);
            this.kryptonButton2_Model1_ST.Name = "kryptonButton2_Model1_ST";
            this.kryptonButton2_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton2_Model1_ST.TabIndex = 2;
            this.kryptonButton2_Model1_ST.Values.Text = "kryptonButton2";
            // 
            // kryptonButton3_Model1_ST
            // 
            this.kryptonButton3_Model1_ST.Location = new System.Drawing.Point(44, 235);
            this.kryptonButton3_Model1_ST.Name = "kryptonButton3_Model1_ST";
            this.kryptonButton3_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton3_Model1_ST.TabIndex = 3;
            this.kryptonButton3_Model1_ST.Values.Text = "kryptonButton3";
            // 
            // kryptonButton4_Model1_ST
            // 
            this.kryptonButton4_Model1_ST.Location = new System.Drawing.Point(46, 333);
            this.kryptonButton4_Model1_ST.Name = "kryptonButton4_Model1_ST";
            this.kryptonButton4_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton4_Model1_ST.TabIndex = 4;
            this.kryptonButton4_Model1_ST.Values.Text = "kryptonButton4";
            // 
            // kryptonButton5_Model1_ST
            // 
            this.kryptonButton5_Model1_ST.Location = new System.Drawing.Point(44, 423);
            this.kryptonButton5_Model1_ST.Name = "kryptonButton5_Model1_ST";
            this.kryptonButton5_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton5_Model1_ST.TabIndex = 5;
            this.kryptonButton5_Model1_ST.Values.Text = "kryptonButton5";
            // 
            // kryptonButton6_Model1_ST
            // 
            this.kryptonButton6_Model1_ST.Location = new System.Drawing.Point(44, 527);
            this.kryptonButton6_Model1_ST.Name = "kryptonButton6_Model1_ST";
            this.kryptonButton6_Model1_ST.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton6_Model1_ST.TabIndex = 6;
            this.kryptonButton6_Model1_ST.Values.Text = "kryptonButton6";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(334, 34);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(38, 12);
            this.labelTime.TabIndex = 7;
            this.labelTime.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1924, 1041);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.kryptonButton6_Model1_ST);
            this.Controls.Add(this.kryptonButton5_Model1_ST);
            this.Controls.Add(this.kryptonButton4_Model1_ST);
            this.Controls.Add(this.kryptonButton3_Model1_ST);
            this.Controls.Add(this.kryptonButton2_Model1_ST);
            this.Controls.Add(this.kryptonButton1_Model1_ST);
            this.Controls.Add(this.chart1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer _statusTimer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1_Model1_ST;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2_Model1_ST;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3_Model1_ST;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4_Model1_ST;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton5_Model1_ST;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton6_Model1_ST;
        private System.Windows.Forms.Label labelTime;
    }
}

