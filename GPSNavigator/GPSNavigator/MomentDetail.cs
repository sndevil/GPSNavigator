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
        int IndexCounter = 0;
        bool playing = false, reading = false,returned = false;
        public MomentDetail()
        {
            InitializeComponent();
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
        }

        public void UpdateData(double xpos, DateTime time, LogFileManager manager)
        {
            filemanager = manager;
            position = (float)xpos;
            data = filemanager.ReadGPSstatus(position);
            ReadCache(position);
            label2.Text = data.VisibleGPS.ToString();
            label4.Text = data.UsedGPS.ToString();
            label6.Text = data.VisibleGlonass.ToString();
            label8.Text = data.UsedGlonass.ToString();
            label12.Text = "Time: " + time.ToString();
            PlotGraph(data);
            CacheData = CacheData2;
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

            //chart1.Series[0].Points.Clear();
            //chart2.Series[0].Points.Clear();
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
                    chart2.Series[0].Points.AddXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                    chart2.Series[0].Points[counter].Color = Color.Blue;
                    counter++;
                }
                if (data.Glonass[i].Signal_Status == 2)
                {
                    chart2.Series[0].Points.AddXY(i.ToString() + "(" + data.Glonass[i].SNR.ToString() + ")", data.Glonass[i].SNR);
                    chart2.Series[0].Points[counter].Color = Color.Green;
                    counter++;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playing)
            {
                PlotGraph(CacheData[(IndexCounter < 100)?IndexCounter: 99]);
                label12.Text = "Time: " + CacheData[(IndexCounter < 100) ? IndexCounter : 99].Time.ToString();
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
                    label14.Text = "Position: " + position.ToString();
                    ReadCache(position);
                    reading = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                button1.Text = "Play";
                playing = false;
            }
            else
            {
                button1.Text = "Pause";
                playing = true;
            }
        }
    }
}
