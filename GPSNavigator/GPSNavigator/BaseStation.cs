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
        public bool ackRecievedFlag,ShowingSettings = false;
        public int BasestationNumber;
        public BTSDetail Parentform;

        public BaseStation(BTSDetail parent)
        {
            InitializeComponent();
            PositionLabel1.Click += new EventHandler(ClickListener_Click);
            PositionLabel2.Click += new EventHandler(ClickListener_Click);
            PositionLabel3.Click += new EventHandler(ClickListener_Click);
            PositionValue1.Click += new EventHandler(ClickListener_Click);
            PositionValue2.Click += new EventHandler(ClickListener_Click);
            PositionValue3.Click += new EventHandler(ClickListener_Click);
            Parentform = parent;
        }

        public void ShowBaseStation(BaseStationInfo info)
        {
            NameLabel.Text = "BTS#" + info.stationNumber.ToString();
            BasestationNumber = info.stationNumber;
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
                BatteryLevelLabel.Text = toUpdate.batteryCharge.ToString() + "%";

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


        private void ClickListener_Click(object sender, EventArgs e)
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

        private void ToggleSettings_Click(object sender, EventArgs e)
        {
            if (ShowingSettings)
            {
                this.Width = 800;
                ToggleSettings.Text = ">";
            }
            else
            {
                this.Width = 1000;
                ToggleSettings.Text = "<";
            }
            ShowingSettings = !ShowingSettings;
        }

        private void SetPositionBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendChangePosCommand(BasestationNumber, Convert.ToDouble(LatTextbox.Text), Convert.ToDouble(LongTextbox.Text), Convert.ToDouble(AltTextbox.Text));
        }

        private void SetPositionModeBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendChangePosModeCommand(BasestationNumber, PosModeCombo.SelectedIndex);
        }

        private void SetTimeBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendChangeTimeCommand(BasestationNumber, timeEdit.Time, dateEdit.DateTime,checkEditSetGPSTime.Checked);
        }

        private void SetRangeBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendChangeRangeCommand(BasestationNumber, Convert.ToDouble(RangeTextbox.Text));
        }

        private void SetNumberBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendChangeNumberCommand(BasestationNumber, Convert.ToInt32(NumberTextbox.Text));
        }

        private void GetStatusBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendGetStatusCommand(BasestationNumber);
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendResetCommand(BasestationNumber);
        }

        private void TurnOnBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendTurnOnCommand(BasestationNumber);
        }

        private void TurnOffBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendTurnOffCommand(BasestationNumber);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Parentform.SendSaveSettingsCommand(BasestationNumber);
        }

    }
}
