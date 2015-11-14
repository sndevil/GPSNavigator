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


namespace GPSNavigator
{
    public partial class MomentDetail : Form
    {
        GPSData data;
        List<GPSData> CacheData = new List<GPSData>();
        List<GPSData> CacheData2 = new List<GPSData>();
        LogFileManager filemanager;
        float position;
        int IndexCounter = 0, VisibleGPS,VisibleGLONASS,UsedGPS,UsedGLONASS;
        bool playing = false, reading = false,returned = false;
        public MomentDetail(LogFileManager manager)
        {
            filemanager = manager;
            InitializeComponent();
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            ReadCache(0f);
            data = CacheData2[0];
            PlotGraph(data);
            CacheData = CacheData2;
        }

        public void UpdateData(double xpos, DateTime time)
        {
            position = (float)xpos;
            data = filemanager.ReadGPSstatus(position);
            ReadCache(position);
            updateLabels(time,0,0,0,0);
            PlotGraph(data);
            CacheData = CacheData2;
        }

        public void updateLabels(DateTime dt,int vgps,int ugps,int vglonass,int uglonass)
        {
            if (vgps != 0 || ugps != 0 || vglonass != 0 || uglonass != 0)
            {
                label2.Text = vgps.ToString();
                label4.Text = ugps.ToString();
                label6.Text = vglonass.ToString();
                label8.Text = uglonass.ToString();
            }
            label12.Text = "Time: " + dt.ToString();
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
            
            int counter = 0;
            for (int i = 0; i < 32; i++)
            {
                if (data.GPS[i].Signal_Status == 1)
                {
                    try
                    {
                        chart1.Series[0].Points[counter].SetValueXY(i.ToString() + "(" + data.GPS[i].SNR.ToString() + ")", data.GPS[i].SNR);
                        chart1.Series[0].Points[counter].Color = Color.Blue;
                    }
                    catch
                    {
                        chart1.Series[0].Points.AddXY(i.ToString() + "(" + data.GPS[i].SNR.ToString() + ")", data.GPS[i].SNR);
                        chart1.Series[0].Points[chart1.Series[0].Points.Count-1].Color = Color.Blue;
                    }
                    VisibleGPS++;
                    counter++;
                }
                if (data.GPS[i].Signal_Status == 2)
                {
                    try
                    {
                        chart1.Series[0].Points[counter].SetValueXY(i.ToString() + "(" + data.GPS[i].SNR.ToString() + ")", data.GPS[i].SNR);
                        chart1.Series[0].Points[counter].Color = Color.Green;
                    }
                    catch
                    {
                        chart1.Series[0].Points.AddXY(i.ToString() + "(" + data.GPS[i].SNR.ToString() + ")", data.GPS[i].SNR);
                        chart1.Series[0].Points[chart1.Series[0].Points.Count-1].Color = Color.Green;
                    }
                    VisibleGPS++;
                    UsedGPS++;
                    counter++;
                }
            }
            //removing previous unneccesary datas
            if (counter < chart1.Series[0].Points.Count && counter != 0)
            {
                for (int i = chart1.Series[0].Points.Count-1; i >= counter; i--)
                    chart1.Series[0].Points.RemoveAt(i);
            }
            counter = 0;
            for (int i = 0; i < 32; i++)
            {
                if (data.Glonass[i].Signal_Status == 1)
                {
                    try
                    {
                        chart2.Series[0].Points[counter].SetValueXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                        chart2.Series[0].Points[counter].Color = Color.Blue;
                    }
                    catch
                    {
                        chart2.Series[0].Points.AddXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                        chart2.Series[0].Points[chart1.Series[0].Points.Count - 1].Color = Color.Blue;
                    }
                    VisibleGLONASS++;
                    counter++;
                }
                if (data.Glonass[i].Signal_Status == 2)
                {
                    try
                    {
                        chart2.Series[0].Points[counter].SetValueXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                        chart2.Series[0].Points[counter].Color = Color.Green;
                    }
                    catch
                    {
                        chart2.Series[0].Points.AddXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                        chart2.Series[0].Points[chart1.Series[0].Points.Count - 1].Color = Color.Green;
                    }
                    VisibleGLONASS++;
                    UsedGLONASS++;
                    counter++;
                }
            }
            //removing previous unneccesary datas
            if (counter < chart2.Series[0].Points.Count && counter != 0)
            {
                for (int i = chart2.Series[0].Points.Count - 1; i >= counter; i--)
                    chart2.Series[0].Points.RemoveAt(i);
            }
            updateLabels(data.Time,VisibleGPS,UsedGPS,VisibleGLONASS,UsedGLONASS);
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
                if (IndexCounter++ >= 100 && returned)
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
    }
}
