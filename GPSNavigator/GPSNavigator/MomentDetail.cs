using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPSNavigator.Classes;


namespace GPSNavigator
{
    public partial class MomentDetail : Form
    {
        public MomentDetail()
        {
            InitializeComponent();
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
        }

        public void UpdateData(GPSData data)
        {
            label2.Text = data.VisibleGPS.ToString();
            label4.Text = data.UsedGPS.ToString();
            label6.Text = data.VisibleGlonass.ToString();
            label8.Text = data.UsedGlonass.ToString();

            PlotGraph(data);
        }

        public void PlotGraph(GPSData data)
        {

            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            for (int i = 0; i < 32; i++)
            {
                if (data.GPS[i].Signal_Status == 2)
                    chart1.Series[0].Points.AddXY(data.GPS[i].SNR.ToString(), data.GPS[i].SNR);
            }

            for (int i = 0; i < 32; i++)
            {
                if (data.Glonass[i].Signal_Status == 2)
                    chart2.Series[0].Points.AddXY(data.Glonass[i].SNR.ToString(), data.Glonass[i].SNR);
            }
        }
    }
}
