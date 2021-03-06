﻿namespace GPSNavigator
{
    partial class RTKDetail
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ResetAvgBtn = new System.Windows.Forms.Button();
            this.SetReferenceBtn = new System.Windows.Forms.Button();
            this.altitudeavgText = new System.Windows.Forms.TextBox();
            this.longitudeavgText = new System.Windows.Forms.TextBox();
            this.latitudeavgText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ReAverageBtn = new System.Windows.Forms.Button();
            this.SetBtn = new System.Windows.Forms.Button();
            this.GetBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.autoRadio = new System.Windows.Forms.RadioButton();
            this.manualRadio = new System.Windows.Forms.RadioButton();
            this.altitudevalText = new System.Windows.Forms.TextBox();
            this.longitudevalText = new System.Windows.Forms.TextBox();
            this.latitudevaltext = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chart1 = new ChartDirector.WinChartViewer();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.SNRLabel = new System.Windows.Forms.Label();
            this.ClearBtn = new System.Windows.Forms.Button();
            this.RotRightBtn = new System.Windows.Forms.Button();
            this.RotLeftBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.DataStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.AverateTimerLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.chartMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ChartSaveBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.chartMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ResetAvgBtn);
            this.groupBox1.Controls.Add(this.SetReferenceBtn);
            this.groupBox1.Controls.Add(this.altitudeavgText);
            this.groupBox1.Controls.Add(this.longitudeavgText);
            this.groupBox1.Controls.Add(this.latitudeavgText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Average Position";
            // 
            // ResetAvgBtn
            // 
            this.ResetAvgBtn.Location = new System.Drawing.Point(184, 63);
            this.ResetAvgBtn.Name = "ResetAvgBtn";
            this.ResetAvgBtn.Size = new System.Drawing.Size(90, 23);
            this.ResetAvgBtn.TabIndex = 7;
            this.ResetAvgBtn.Text = "Reset Avg";
            this.ResetAvgBtn.UseVisualStyleBackColor = true;
            this.ResetAvgBtn.Click += new System.EventHandler(this.ResetAvgBtn_Click);
            // 
            // SetReferenceBtn
            // 
            this.SetReferenceBtn.Location = new System.Drawing.Point(184, 33);
            this.SetReferenceBtn.Name = "SetReferenceBtn";
            this.SetReferenceBtn.Size = new System.Drawing.Size(90, 23);
            this.SetReferenceBtn.TabIndex = 6;
            this.SetReferenceBtn.Text = "Set Reference";
            this.SetReferenceBtn.UseVisualStyleBackColor = true;
            this.SetReferenceBtn.Click += new System.EventHandler(this.SetReferenceBtn_Click);
            // 
            // altitudeavgText
            // 
            this.altitudeavgText.Location = new System.Drawing.Point(74, 76);
            this.altitudeavgText.Name = "altitudeavgText";
            this.altitudeavgText.Size = new System.Drawing.Size(100, 20);
            this.altitudeavgText.TabIndex = 5;
            // 
            // longitudeavgText
            // 
            this.longitudeavgText.Location = new System.Drawing.Point(74, 48);
            this.longitudeavgText.Name = "longitudeavgText";
            this.longitudeavgText.Size = new System.Drawing.Size(100, 20);
            this.longitudeavgText.TabIndex = 4;
            // 
            // latitudeavgText
            // 
            this.latitudeavgText.Location = new System.Drawing.Point(74, 21);
            this.latitudeavgText.Name = "latitudeavgText";
            this.latitudeavgText.Size = new System.Drawing.Size(100, 20);
            this.latitudeavgText.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Altitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Longitude";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Latitude";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ReAverageBtn);
            this.groupBox2.Controls.Add(this.SetBtn);
            this.groupBox2.Controls.Add(this.GetBtn);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.altitudevalText);
            this.groupBox2.Controls.Add(this.longitudevalText);
            this.groupBox2.Controls.Add(this.latitudevaltext);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(360, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 113);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Set Reference Position";
            // 
            // ReAverageBtn
            // 
            this.ReAverageBtn.Location = new System.Drawing.Point(191, 87);
            this.ReAverageBtn.Name = "ReAverageBtn";
            this.ReAverageBtn.Size = new System.Drawing.Size(81, 23);
            this.ReAverageBtn.TabIndex = 9;
            this.ReAverageBtn.Text = "Re-Average";
            this.ReAverageBtn.UseVisualStyleBackColor = true;
            this.ReAverageBtn.Click += new System.EventHandler(this.ReAverageBtn_Click);
            // 
            // SetBtn
            // 
            this.SetBtn.Enabled = false;
            this.SetBtn.Location = new System.Drawing.Point(104, 87);
            this.SetBtn.Name = "SetBtn";
            this.SetBtn.Size = new System.Drawing.Size(81, 23);
            this.SetBtn.TabIndex = 8;
            this.SetBtn.Text = "Set";
            this.SetBtn.UseVisualStyleBackColor = true;
            this.SetBtn.Click += new System.EventHandler(this.SetBtn_Click);
            // 
            // GetBtn
            // 
            this.GetBtn.Location = new System.Drawing.Point(16, 87);
            this.GetBtn.Name = "GetBtn";
            this.GetBtn.Size = new System.Drawing.Size(81, 23);
            this.GetBtn.TabIndex = 7;
            this.GetBtn.Text = "Get";
            this.GetBtn.UseVisualStyleBackColor = true;
            this.GetBtn.Click += new System.EventHandler(this.GetBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.autoRadio);
            this.groupBox3.Controls.Add(this.manualRadio);
            this.groupBox3.Location = new System.Drawing.Point(181, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(100, 68);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Mode";
            // 
            // autoRadio
            // 
            this.autoRadio.AutoSize = true;
            this.autoRadio.Checked = true;
            this.autoRadio.Location = new System.Drawing.Point(8, 47);
            this.autoRadio.Name = "autoRadio";
            this.autoRadio.Size = new System.Drawing.Size(72, 17);
            this.autoRadio.TabIndex = 1;
            this.autoRadio.TabStop = true;
            this.autoRadio.Text = "Automatic";
            this.autoRadio.UseVisualStyleBackColor = true;
            // 
            // manualRadio
            // 
            this.manualRadio.AutoSize = true;
            this.manualRadio.Location = new System.Drawing.Point(8, 20);
            this.manualRadio.Name = "manualRadio";
            this.manualRadio.Size = new System.Drawing.Size(60, 17);
            this.manualRadio.TabIndex = 0;
            this.manualRadio.Text = "Manual";
            this.manualRadio.UseVisualStyleBackColor = true;
            this.manualRadio.CheckedChanged += new System.EventHandler(this.manualRadio_CheckedChanged);
            // 
            // altitudevalText
            // 
            this.altitudevalText.Enabled = false;
            this.altitudevalText.Location = new System.Drawing.Point(74, 63);
            this.altitudevalText.Name = "altitudevalText";
            this.altitudevalText.Size = new System.Drawing.Size(100, 20);
            this.altitudevalText.TabIndex = 5;
            // 
            // longitudevalText
            // 
            this.longitudevalText.Enabled = false;
            this.longitudevalText.Location = new System.Drawing.Point(74, 40);
            this.longitudevalText.Name = "longitudevalText";
            this.longitudevalText.Size = new System.Drawing.Size(100, 20);
            this.longitudevalText.TabIndex = 4;
            // 
            // latitudevaltext
            // 
            this.latitudevaltext.Enabled = false;
            this.latitudevaltext.Location = new System.Drawing.Point(74, 17);
            this.latitudevaltext.Name = "latitudevaltext";
            this.latitudevaltext.Size = new System.Drawing.Size(100, 20);
            this.latitudevaltext.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Altitude";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Longitude";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Latitude";
            // 
            // chart1
            // 
            this.chart1.Location = new System.Drawing.Point(12, 269);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(648, 366);
            this.chart1.TabIndex = 11;
            this.chart1.TabStop = false;
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // chart2
            // 
            this.chart2.BackColor = System.Drawing.Color.Gainsboro;
            this.chart2.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.VerticalCenter;
            this.chart2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chart2.BackImageTransparentColor = System.Drawing.Color.White;
            this.chart2.BackSecondaryColor = System.Drawing.Color.White;
            this.chart2.BorderSkin.PageColor = System.Drawing.Color.Gainsboro;
            this.chart2.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Raised;
            chartArea1.Area3DStyle.PointDepth = 50;
            chartArea1.Area3DStyle.Rotation = 15;
            chartArea1.Area3DStyle.WallWidth = 10;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelAutoFitMinFontSize = 5;
            chartArea1.AxisX.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep45)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX.LabelStyle.TruncatedLabels = true;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisX2.LabelAutoFitMinFontSize = 5;
            chartArea1.AxisX2.LabelAutoFitStyle = ((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)(((((((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.StaggeredLabels)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep30)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep45)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.LabelsAngleStep90)
                        | System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap)));
            chartArea1.AxisX2.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY2.LineColor = System.Drawing.Color.LightGray;
            chartArea1.BorderColor = System.Drawing.Color.DarkGray;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 75F;
            chartArea1.InnerPlotPosition.Width = 95F;
            chartArea1.InnerPlotPosition.X = 5F;
            chartArea1.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea1);
            this.chart2.Location = new System.Drawing.Point(12, 122);
            this.chart2.Name = "chart2";
            this.chart2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartArea1";
            series1.IsValueShownAsLabel = true;
            series1.Legend = "Legend1";
            series1.Name = "Series0";
            series1.SmartLabelStyle.Enabled = false;
            this.chart2.Series.Add(series1);
            this.chart2.Size = new System.Drawing.Size(648, 148);
            this.chart2.TabIndex = 12;
            this.chart2.Text = "GPS";
            title1.Name = "Title1";
            this.chart2.Titles.Add(title1);
            // 
            // SNRLabel
            // 
            this.SNRLabel.AutoSize = true;
            this.SNRLabel.Location = new System.Drawing.Point(26, 134);
            this.SNRLabel.Name = "SNRLabel";
            this.SNRLabel.Size = new System.Drawing.Size(30, 13);
            this.SNRLabel.TabIndex = 13;
            this.SNRLabel.Text = "SNR";
            // 
            // ClearBtn
            // 
            this.ClearBtn.Location = new System.Drawing.Point(20, 277);
            this.ClearBtn.Name = "ClearBtn";
            this.ClearBtn.Size = new System.Drawing.Size(75, 23);
            this.ClearBtn.TabIndex = 14;
            this.ClearBtn.Text = "Clear";
            this.ClearBtn.UseVisualStyleBackColor = true;
            this.ClearBtn.Click += new System.EventHandler(this.ClearBtn_Click);
            // 
            // RotRightBtn
            // 
            this.RotRightBtn.Location = new System.Drawing.Point(484, 305);
            this.RotRightBtn.Name = "RotRightBtn";
            this.RotRightBtn.Size = new System.Drawing.Size(35, 30);
            this.RotRightBtn.TabIndex = 15;
            this.RotRightBtn.Text = ">";
            this.RotRightBtn.UseVisualStyleBackColor = true;
            this.RotRightBtn.Click += new System.EventHandler(this.RotRightBtn_Click);
            // 
            // RotLeftBtn
            // 
            this.RotLeftBtn.Location = new System.Drawing.Point(151, 305);
            this.RotLeftBtn.Name = "RotLeftBtn";
            this.RotLeftBtn.Size = new System.Drawing.Size(35, 30);
            this.RotLeftBtn.TabIndex = 16;
            this.RotLeftBtn.Text = "<";
            this.RotLeftBtn.UseVisualStyleBackColor = true;
            this.RotLeftBtn.Click += new System.EventHandler(this.RotLeftBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.LightGray;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DataStatusLabel,
            this.AverateTimerLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 636);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(672, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // DataStatusLabel
            // 
            this.DataStatusLabel.BackColor = System.Drawing.Color.Salmon;
            this.DataStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.DataStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.DataStatusLabel.Name = "DataStatusLabel";
            this.DataStatusLabel.Size = new System.Drawing.Size(50, 17);
            this.DataStatusLabel.Text = "No Data";
            // 
            // AverateTimerLabel
            // 
            this.AverateTimerLabel.Name = "AverateTimerLabel";
            this.AverateTimerLabel.Size = new System.Drawing.Size(51, 17);
            this.AverateTimerLabel.Text = "00:00:00";
            // 
            // chartMenuStrip
            // 
            this.chartMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChartSaveBtn});
            this.chartMenuStrip.Name = "chartMenuStrip";
            this.chartMenuStrip.Size = new System.Drawing.Size(99, 26);
            // 
            // ChartSaveBtn
            // 
            this.ChartSaveBtn.Name = "ChartSaveBtn";
            this.ChartSaveBtn.Size = new System.Drawing.Size(98, 22);
            this.ChartSaveBtn.Text = "Save";
            // 
            // RTKDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(672, 658);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RotLeftBtn);
            this.Controls.Add(this.RotRightBtn);
            this.Controls.Add(this.ClearBtn);
            this.Controls.Add(this.SNRLabel);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTKDetail";
            this.Text = "RTKDetail";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.chartMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ResetAvgBtn;
        private System.Windows.Forms.Button SetReferenceBtn;
        private System.Windows.Forms.TextBox altitudeavgText;
        private System.Windows.Forms.TextBox longitudeavgText;
        private System.Windows.Forms.TextBox latitudeavgText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ReAverageBtn;
        private System.Windows.Forms.Button SetBtn;
        private System.Windows.Forms.Button GetBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton autoRadio;
        private System.Windows.Forms.RadioButton manualRadio;
        private System.Windows.Forms.TextBox altitudevalText;
        private System.Windows.Forms.TextBox longitudevalText;
        private System.Windows.Forms.TextBox latitudevaltext;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private ChartDirector.WinChartViewer chart1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Label SNRLabel;
        private System.Windows.Forms.Button ClearBtn;
        private System.Windows.Forms.Button RotRightBtn;
        private System.Windows.Forms.Button RotLeftBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel DataStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel AverateTimerLabel;
        private System.Windows.Forms.ContextMenuStrip chartMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ChartSaveBtn;

    }
}