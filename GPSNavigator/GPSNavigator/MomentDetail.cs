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


namespace GPSNavigator
{
    public partial class MomentDetail : Form
    {
        GPSData data;
        List<GPSData> CacheData = new List<GPSData>();
        List<GPSData> CacheData2 = new List<GPSData>();
        LogFileManager filemanager;
        float position;
        int IndexCounter = 0,SlowCounter = 0, VisibleGPS,VisibleGLONASS,UsedGPS,UsedGLONASS;
        bool playing = false, reading = false,returned = false,realtime = false;
        PlaybackSpeed playspeed = PlaybackSpeed.NormalSpeed;
        EllipseAnnotation DateLabel;

        public MomentDetail(LogFileManager manager)
        {
            filemanager = manager;
            InitializeComponent();
            ControlPanel.Visible = true;
            DateLabel = ultraGaugeClock.Annotations[0] as EllipseAnnotation;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            ReadCache(0f);
            data = CacheData2[0];
            PlotGraph(data);
            CacheData = CacheData2;
        }

        public MomentDetail()
        {
            InitializeComponent();
            ControlPanel.Visible = false;
            DateLabel = ultraGaugeClock.Annotations[0] as EllipseAnnotation;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            realtime = true;
        }

        public void UpdateData(Globals vars, SingleDataBuffer data, int ChannelNum)
        {
            var tempgpsdata = new GPSData();
            for (int i = 0; i < vars.GPSlist.Count; i++)
            {
                tempgpsdata.GPS.Add(vars.GPSlist[i]);
                tempgpsdata.Glonass.Add(vars.GLONASSlist[i]);
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

        public void updateLabels(DateTime dt,int vgps,int ugps,int vglonass,int uglonass,float PDOP, float Altitude, float Longitude, float Latitude)
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
            VisibleGLONASS = VisibleGPS = UsedGLONASS = UsedGPS = 0;
            System.Windows.Forms.DataVisualization.Charting.Chart tempchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            int counter = 0;
            for (int j = 0; j < data.GPS.Count; j++)
            {
                switch (j)
                {
                    case 0:
                        tempchart = chart1;
                        break;
                    case 1:
                        tempchart = chart2;
                        break;
                }
                /////for showing data series j > 0, make other charts
                var tempsat = new Satellite[64];
                for (int i = 0; i < 64; i++)
                {
                    tempsat[i] = (i < 32) ? data.GPS[j][i] : data.Glonass[j][i - 32];
                    if (tempsat[i].Signal_Status == 1)
                    {
                        try
                        {
                            tempchart.Series[0].Points[counter].SetValueXY(((i < 32) ? "GP" + i.ToString() : "GL" + (i - 32).ToString()), tempsat[i].SNR);
                            tempchart.Series[0].Points[counter].Color = (i < 32) ? Color.Blue : Color.Red;
                        }
                        catch
                        {
                            tempchart.Series[0].Points.AddXY(((i < 32) ? "GP" + i.ToString() : "GL" + (i - 32).ToString()), tempsat[i].SNR);
                            tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = (i < 32) ? Color.Blue : Color.Red;
                        }
                        if (i < 32) VisibleGPS++;else VisibleGLONASS++;
                        counter++;
                    }
                    if (tempsat[i].Signal_Status == 2)
                    {
                        try
                        {
                            tempchart.Series[0].Points[counter].SetValueXY(((i < 32) ? "GP" + i.ToString() : "GL" + (i - 32).ToString()), tempsat[i].SNR);
                            tempchart.Series[0].Points[counter].Color = (i < 32) ? Color.Green : Color.Yellow;
                        }
                        catch
                        {
                            tempchart.Series[0].Points.AddXY(((i < 32) ? "GP" + i.ToString() : "GL" + (i - 32).ToString()), tempsat[i].SNR);
                            tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = (i < 32) ? Color.Green : Color.Yellow;
                        }
                        if (i < 32) VisibleGPS++; else VisibleGLONASS++;
                        if (i < 32) UsedGPS++; else UsedGLONASS++;
                        counter++;
                    }
                }
                if (counter < tempchart.Series[0].Points.Count && counter != 0)
                {
                    for (int i = tempchart.Series[0].Points.Count - 1; i >= counter; i--)
                        tempchart.Series[0].Points.RemoveAt(i);
                }
                counter = 0;
            }
            //removing previous unneccesary datas
            updateLabels(data.Time,VisibleGPS/2,UsedGPS/2,VisibleGLONASS/2,UsedGLONASS/2,data.PDOP,data.Altitude,data.Longitude,data.Latitude);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playing)
            {
                if (position >= 1f)
                {
                    button1.Text = "Play";
                    playing = false;
                    this.Text = "MomentDetail";
                }
                PlotGraph(CacheData[(IndexCounter < 100)?IndexCounter: 99]);

                switch (playspeed)
                {
                    case PlaybackSpeed.NormalSpeed:
                        IndexCounter++;
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
                        IndexCounter += 2;
                        break;
                    case PlaybackSpeed.Quadrople:
                        IndexCounter += 4;
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
    }
}
