using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GPSNavigator.Classes;
using Infragistics.UltraGauge.Resources;
using System.Windows.Forms.DataVisualization.Charting;


namespace GPSNavigator
{
    public partial class MomentDetail : Form
    {
        GPSData data;
        List<GPSData> CacheData = new List<GPSData>();
        List<GPSData> CacheData2 = new List<GPSData>();
        LogFileManager filemanager;
        float position;
        int IndexCounter = 0,SlowCounter = 0, VisibleGPS,VisibleGLONASS,UsedGPS,UsedGLONASS,Chart1Item, Chart2Item=-1, DataTimeOut = 100;
        bool playing = false, reading = false,returned = false,realtime = false,Receiving = false;
        public bool paused = false;
        PlaybackSpeed playspeed = PlaybackSpeed.NormalSpeed;
        Infragistics.UltraGauge.Resources.EllipseAnnotation DateLabel;
        Form1 ParentForm;// = new Form1();

        public MomentDetail(LogFileManager manager)
        {
            filemanager = manager;
            InitializeComponent();
            toolStripStatusLabel1.Visible = false;
            ControlPanel.Visible = true;
            DateLabel = ultraGaugeClock.Annotations[0] as Infragistics.UltraGauge.Resources.EllipseAnnotation;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            ReadCache(0f);
            data = CacheData2[0];
            UpdateComboBoxes(data.GPS.Count);
            if (data.GPS.Count > 1)
                ShowSecondGraph();
            else
                HideSecondGraph();
            PlotGraph(data);
            CacheData = CacheData2;
            realtime = false;
        }

        public MomentDetail(Form1 Parent)
        {
            InitializeComponent();
            ParentForm = Parent;
            toolStripStatusLabel1.Visible = true;
            ControlPanel.Visible = false;
            DateLabel = ultraGaugeClock.Annotations[0] as Infragistics.UltraGauge.Resources.EllipseAnnotation;
            chart1.Series[0].XValueType = ChartValueType.String;
            realtime = true;
            HideSecondGraph();
        }

        public void UpdateData(Globals vars, SingleDataBuffer data, int ChannelNum)
        {
            Receiving = true;
            DataTimeOut = 100;
            var tempgpsdata = new GPSData();
            for (int i = 0; i < vars.GPSlist.Count; i++)
            {
                tempgpsdata.GPS.Add(vars.GPSlist[i]);
                tempgpsdata.Glonass.Add(vars.GLONASSlist[i]);
                UpdateComboBoxes(vars.GPSlist.Count);
            }
            tempgpsdata.Time = vars.PacketTime;
            tempgpsdata.PDOP = (float)data.PDOP;
            tempgpsdata.Latitude = (float)data.Latitude;
            tempgpsdata.Longitude = (float)data.Longitude;
            tempgpsdata.Altitude = (float)data.Altitude;
            var t = chart1.Series.Count.ToString();
            PlotGraph(tempgpsdata);
        }

        public void UpdateData(double xpos, DateTime time)
        {
            position = (float)xpos;
            data = filemanager.ReadGPSstatus(position);
            ReadCache(position);
            updateLabels(time,0,0,0,0,data.PDOP,data.Altitude,data.Longitude,data.Latitude);
            PlotGraph(data);
            CacheData = CacheData2;
        }

        public void UpdateComboBoxes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                try
                {
                    comboBox1.Items[i] = "Antenna " + i.ToString();
                    comboBox2.Items[i] = "Antenna " + i.ToString();
                }
                catch
                {
                    comboBox1.Items.Add("Antenna " + i.ToString());
                    comboBox2.Items.Add("Antenna " + i.ToString());
                }
            }
            if (comboBox1.SelectedIndex < 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
        }

        public void updateLabels(DateTime dt, int vgps, int ugps, int vglonass, int uglonass, float PDOP, float Altitude, float Longitude, float Latitude)
        {
            if (vgps != 0 || ugps != 0 || vglonass != 0 || uglonass != 0)
            {
                label2.Text = vgps.ToString();
                label4.Text = ugps.ToString();
                label6.Text = vglonass.ToString();
                label8.Text = uglonass.ToString();
            }

            PDOPValue.Text = PDOP.ToString();
            LatitudeValue.Text = Latitude.ToString();
            LongitudeValue.Text = Longitude.ToString();
            AltitudeValue.Text = Altitude.ToString();

            label12.Text = "Time: " + dt.ToString();
            DateLabel.Label.FormatString = dt.Month.ToString("D2") + "/" + dt.Day.ToString("D2") + "/" + dt.Year.ToString();
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[0].Markers[0].Value = dt.Hour + (double)dt.Minute / 60;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[1].Markers[0].Value = dt.Minute;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[2].Markers[0].Value = dt.Second;
            label14.Text = "Position: " + position.ToString();
        }

        public void ReadCache(float position)
        {
            AsyncCaller asynctask = new AsyncCaller(filemanager.ReadGPSCache);
            IAsyncResult asyncresult = asynctask.BeginInvoke(position, null, null);
            CacheData2 = asynctask.EndInvoke(asyncresult);
            reading = false;
            returned = true;
        }

        public void PlotGraph(GPSData data)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart tempchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            int counter = 0,showindex = -1;
            for (int j = 0; j < 2; j++)
            {
                switch (j)
                {
                    case 0:
                        tempchart = chart1;
                        showindex = Chart1Item;
                        break;
                    case 1:
                        tempchart = chart2;
                        showindex = Chart2Item;
                        break;
                }
                if (showindex != -1 && data.GPS.Count > j)
                {
                    VisibleGLONASS = VisibleGPS = UsedGLONASS = UsedGPS = 0;
                    var tempsat = new Satellite[64];
                    for (int i = 0; i < 64; i++)
                    {
                        tempsat[i] = (i < 32) ? data.GPS[showindex][i] : data.Glonass[showindex][i - 32];
                        if (tempsat[i].Signal_Status == 1)
                        {
                            try
                            {
                                tempchart.Series[0].Points[counter].SetValueXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                tempchart.Series[0].Points[counter].Color = Color.Blue;
                                tempchart.Series[0].Points[counter].BackHatchStyle = (i >= 32) ? ChartHatchStyle.ForwardDiagonal : ChartHatchStyle.None;
                            }
                            catch
                            {
                                tempchart.Series[0].Points.AddXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = Color.Blue;
                                tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].BackHatchStyle = (i >= 32) ? ChartHatchStyle.ForwardDiagonal : ChartHatchStyle.None;
                            }
                            if (i < 32) VisibleGPS++; else VisibleGLONASS++;
                            counter++;
                        }
                        if (tempsat[i].Signal_Status == 2)
                        {
                            try
                            {
                                tempchart.Series[0].Points[counter].SetValueXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                tempchart.Series[0].Points[counter].Color = Color.Green;
                                tempchart.Series[0].Points[counter].BackHatchStyle = (i >= 32) ? ChartHatchStyle.ForwardDiagonal : ChartHatchStyle.None;
                            }
                            catch
                            {
                                tempchart.Series[0].Points.AddXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = Color.Green;
                                tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].BackHatchStyle = (i >= 32) ? ChartHatchStyle.ForwardDiagonal : ChartHatchStyle.None;
                            }
                            if (i < 32) VisibleGPS++; else VisibleGLONASS++;
                            if (i < 32) UsedGPS++; else UsedGLONASS++;
                            counter++;
                        }
                    }
                }
                if (counter < tempchart.Series[0].Points.Count)// && counter != 0)
                {
                    for (int i = tempchart.Series[0].Points.Count - 1; i >= counter; i--)
                        tempchart.Series[0].Points.RemoveAt(i);
                }
                counter = 0;
            }
            //removing previous unneccesary datas
            updateLabels(data.Time,VisibleGPS,UsedGPS,VisibleGLONASS,UsedGLONASS,data.PDOP,data.Altitude,data.Longitude,data.Latitude);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!paused)
            {
                if (DataTimeOut >= 0)
                    DataTimeOut--;
                if (Receiving)
                {
                    toolStripStatusLabel1.Text = "Receiving Data";
                    toolStripStatusLabel1.BackColor = Color.Lime;
                }
                if (DataTimeOut < 0 && Receiving)
                {
                    toolStripStatusLabel1.Text = "No Data";
                    toolStripStatusLabel1.BackColor = Color.Salmon;
                    Receiving = false;
                    PlotGraph(new GPSData());
                }
            }
            if (playing)
            {
                if (position >= 1f)
                {
                    button1.Text = "Play";
                    playing = false;
                    this.Text = "MomentDetail";
                }
                var toplot = CacheData[(IndexCounter < 100) ? IndexCounter : 99];
                PlotGraph(toplot);
                UpdateComboBoxes(toplot.GPS.Count);
                switch (playspeed)
                {
                    case PlaybackSpeed.NormalSpeed:
                        IndexCounter+=2;
                        break;
                    case PlaybackSpeed.Half:
                        if (SlowCounter++ >= 2)
                        {
                            IndexCounter++;
                            SlowCounter = 0;
                        }
                        break;
                    case PlaybackSpeed.Quarter:
                        if (SlowCounter++ >= 4)
                        {
                            IndexCounter++;
                            SlowCounter = 0;
                        }
                        break;
                    case PlaybackSpeed.Double:
                        IndexCounter += 4;
                        break;
                    case PlaybackSpeed.Quadrople:
                        IndexCounter += 8;
                        break;
                }

                if (IndexCounter >= 100 && returned)
                {
                    IndexCounter = 0;
                    CacheData = CacheData2;
                    reading = false;
                }
                if (IndexCounter >= 10 && !reading)
                {
                    returned = false;
                    position += filemanager.delta;
                    ReadCache(position);
                    reading = true;
                }
            }
        }

        #region PlaybackFuncs
        private void button1_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                this.Text = "MomentDetail";
                button1.Text = "Play";
                playing = false;
            }
            else
            {
                this.Text = "MomentDetail (Playing)";
                button1.Text = "Pause";
                playing = true;
            }
        }

        private void NextFrame_Click(object sender, EventArgs e)
        {
            if (IndexCounter < 99)
                IndexCounter++;
            PlotGraph(CacheData[IndexCounter]);
        }

        private void PreviousFrame_Click(object sender, EventArgs e)
        {
            if (IndexCounter > 0)
                IndexCounter--;
            PlotGraph(CacheData[IndexCounter]);
        }

        private void QuarterRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (QuarterRadio.Checked)
                playspeed = PlaybackSpeed.Quarter;
        }

        private void HalfRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (HalfRadio.Checked)
                playspeed = PlaybackSpeed.Half;
        }

        private void NormalRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (NormalRadio.Checked)
                playspeed = PlaybackSpeed.NormalSpeed;
        }

        private void DoubleRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (DoubleRadio.Checked)
                playspeed = PlaybackSpeed.Double;
        }

        private void QuadRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (QuadRadio.Checked)
                playspeed = PlaybackSpeed.Quadrople;
        }
        #endregion

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label13.Text = comboBox2.SelectedItem.ToString() + " SNR Value";
            Chart2Item = comboBox2.SelectedIndex;
            if (Chart2Item == Chart1Item)
                Chart2Item = -1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label9.Text = comboBox1.SelectedItem.ToString() + " SNR Value";
            Chart1Item = comboBox1.SelectedIndex;
        }

        private void ChartVisibleCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ChartVisibleCheck.Checked)
                ShowSecondGraph();
            else
                HideSecondGraph();

        }
        private void HideSecondGraph()
        {
            chart2.Visible = false;
            label13.Visible = false;
            comboBox2.Visible = false;
        }
        private void ShowSecondGraph()
        {
            chart2.Visible = true;
            label13.Visible = true;
            label13.BringToFront();
            comboBox2.Visible = true;
        }

        private void MomentDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (realtime)
                ParentForm.checkBox2.Checked = false;
        }
    }
}
