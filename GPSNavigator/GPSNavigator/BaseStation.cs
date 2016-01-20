using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPSNavigator.Classes;
using GPSNavigator.Source;

namespace GPSNavigator
{
    public partial class BaseStation : Form
    {
        public BaseStationInfo data;
        public bool xyz = true;
        public GEOpoint LatLong;
        public double x, y, z;

        public BaseStation()
        {
            InitializeComponent();
        }

        public void ShowBaseStation(BaseStationInfo info)
        {
            NameLabel.Text = "BTS#" + info.stationNumber.ToString();
            UpdateGUI(info);
        }
        public void UpdateGUI(BaseStationInfo toUpdate)
        {

            try
            {
                PlotGraph(toUpdate.SNRs);
                x = toUpdate.x; y = toUpdate.y; z = toUpdate.z;
                LatLong = Functions.Calculate_LatLongAlt_From_XYZ(toUpdate.x, toUpdate.y, toUpdate.z);
                UpdatePosition();

                digGgVoltage.Text = toUpdate.voltage.ToString();
                digGgCurrent.Text = toUpdate.current.ToString();
                digitalGauge2.Text = toUpdate.stability.ToString();
                digitalGauge1.Text = toUpdate.lastLocktoGPS.ToString();

                linearScaleLevelComponent1.Value = (float)(toUpdate.humidity);
                linearScaleComponent2.Value = (float)toUpdate.batteryCharge;

            }
            catch
            {
                //when the document is closed
            }
        }

        public void PlotGraph(double[] SNRInfo)
        {
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < SNRInfo.Length; i++)
                chart1.Series[0].Points.AddXY((double)i + 1,SNRInfo[i]);

        }

        public void UpdatePosition()
        {
            if (xyz)
            {
                PositionValue1.Text = x.ToString();
                PositionValue2.Text = y.ToString();
                PositionValue3.Text = z.ToString();
            }
            else
            {
                PositionValue1.Text = LatLong.Latitude.ToString("#0.000000");
                PositionValue2.Text = LatLong.Longitude.ToString("#0.000000");
                PositionValue3.Text = LatLong.Altitude.ToString("#0.00");
            }
        }

        private void ChangePosTypeButton_Click(object sender, EventArgs e)
        {
            if (xyz)
            {
                xyz = false;
                PositionLabel1.Text = "Lattitude"; PositionLabel2.Text = "Longitude"; PositionLabel3.Text = "Altitude";
            }
            else
            {
                xyz = true;
                PositionLabel1.Text = "X"; PositionLabel2.Text = "Y"; PositionLabel3.Text = "Z";
            }
        }


    }
}
