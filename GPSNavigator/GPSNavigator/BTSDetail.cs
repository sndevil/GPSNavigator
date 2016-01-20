using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GPSNavigator.Classes;
using GPSNavigator.Source;
using Infragistics.UltraGauge.Resources;
using Telerik.WinControls.UI;


namespace GPSNavigator
{
    public partial class BTSDetail : Form
    {
        public List<BaseStationInfo> BaseStationList;// = new List<BaseStationInfo>();
        public List<BaseStation> BaseStationFormList;

        public List<Color> RowColor = new List<Color>();
        public List<int> BlinkingList = new List<int>();
        public List<int> BlinkingList2 = new List<int>();
        public DateTime datetime;

        Infragistics.UltraGauge.Resources.EllipseAnnotation DateLabel;

        bool cycle = true;
        Random rnd = new Random();

        //private BaseStation BaseStationForm = new BaseStation();
        public Form1 Parentform;

        public BTSDetail(Form1 parent)
        {
            Parentform = parent;
            InitializeComponent();

            DateLabel = ultraGaugeClock.Annotations[0] as Infragistics.UltraGauge.Resources.EllipseAnnotation;


            BaseStationList = new List<BaseStationInfo>();
            BaseStationFormList = new List<BaseStation>();
            SingleDataBuffer d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();            
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 1;
            d.BaseStationBuffer.humidity = 0.5;
            d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            d.BaseStationBuffer.ltrHealth = 34;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stability = 1;
            d.BaseStationBuffer.stationNumber = 3;
            d.BaseStationBuffer.temperature = 23;
            d.BaseStationBuffer.voltage = 12;
            d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            UpdateInfo(d);
            d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 1;
            d.BaseStationBuffer.humidity = 0.5;
            d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            d.BaseStationBuffer.ltrHealth = 34;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stationNumber = 4;
            d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            UpdateInfo(d);
            d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 1;
            d.BaseStationBuffer.humidity = 0.5;
            d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            d.BaseStationBuffer.ltrHealth = 34;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stationNumber = 5;
            d.datetime = DateTime.Now;
            d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            UpdateInfo(d);


            //radGridView1.Rows.Add(new object[] { 1, "1", "2", "3", "4", "5", "6", "7", "View Details" });
        }

        public void UpdateInfo(SingleDataBuffer databuffer)
        {
            datetime = databuffer.datetime;
            ShowTime(datetime);
            //databuffer.BaseStationBuffer.stationNumber
            int index = -1;
            //foreach (BaseStationInfo i in BaseStationList)
            //{
            //    if (i.stationNumber == databuffer.BaseStationBuffer.stationNumber) 
            //        index =
            //}
            for (int i = 0; i < BaseStationList.Count;i++)
            {
                if (BaseStationList[i].stationNumber == databuffer.BaseStationBuffer.stationNumber)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                //Not found. making new row for it
                AddBTS(databuffer.BaseStationBuffer);
            }
            else
            {
                UpdateBTSInfo(databuffer, index);
            }

        }

        public void AddBTS(BaseStationInfo toAdd)
        {
            toAdd.rownumber = radGridView1.Rows.Count;
            BaseStationList.Add(toAdd);
            var basestationstate = toAdd.ltrHealth / 16;
            string str;
            if (basestationstate == 1)
                str = "Internal GPS";
            else if (basestationstate == 2)
                str = "External GPS";
            else
                str = "Manual";
            radGridView1.Rows.Add(new object[] {toAdd.stationNumber, "BTS #" + toAdd.stationNumber.ToString(),toAdd.stationMACNumber.ToString(),toAdd.voltage.ToString("F2"),toAdd.current.ToString("F2")
                    ,toAdd.stability.ToString(), toAdd.lastLocktoGPS.ToString(),toAdd.rssiBaseStation.ToString(),toAdd.rssiCenterStation.ToString(), toAdd.temperature.ToString()
                    ,toAdd.batteryCharge.ToString(), str,"View Details"});
            var idx = radGridView1.Rows.Count - 1;
            GridViewRowInfo item = radGridView1.Rows[idx];
            AddBlinkingCell(idx);

            if ((toAdd.ltrHealth & 0xF) != 0)
            {
                item.Cells[0].Value = toAdd.stationNumber;
                item.Cells[1].Value = "BTS#" + toAdd.stationNumber.ToString();
                item.Cells[2].Value = toAdd.stationMACNumber.ToString();
                item.Cells[3].Value = toAdd.voltage;
                item.Cells[4].Value = toAdd.current;
                item.Cells[5].Value = toAdd.stability;
                item.Cells[6].Value = toAdd.lastLocktoGPS;
                item.Cells[7].Value = toAdd.rssiBaseStation;
                item.Cells[8].Value = toAdd.rssiCenterStation;
                item.Cells[9].Value = toAdd.temperature;
                item.Cells[10].Value = toAdd.batteryCharge;
                item.Cells[11].Value = str;
                item.Cells[12].Value = "View Details";

                foreach (GridViewCellInfo cell in item.Cells)
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.DrawFill = true;
                    cell.Style.BackColor = Color.Lime;
                }
            }
            else
            {
                item.Cells[0].Value = toAdd.stationNumber;
                item.Cells[1].Value = "BTS#" + toAdd.stationNumber.ToString();
                item.Cells[2].Value = toAdd.stationMACNumber.ToString();
                item.Cells[3].Value = toAdd.voltage;
                item.Cells[4].Value = toAdd.current;
                item.Cells[5].Value = toAdd.stability;
                item.Cells[6].Value = toAdd.lastLocktoGPS;
                item.Cells[7].Value = toAdd.rssiBaseStation;
                item.Cells[8].Value = toAdd.rssiCenterStation;
                item.Cells[9].Value = toAdd.temperature;
                item.Cells[10].Value = toAdd.batteryCharge;
                item.Cells[11].Value = str;
                item.Cells[12].Value = "Data Not Valid";

                foreach (GridViewCellInfo cell in item.Cells)
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.DrawFill = true;
                    cell.Style.BackColor = Color.Red;
                }
            }



        }

        public void UpdateBTSInfo(SingleDataBuffer databuffer, int index)
        {
            BaseStationList[index] = databuffer.BaseStationBuffer;
            GridViewRowInfo item = radGridView1.Rows[index];
            AddBlinkingCell(index);
            var basestationstate = databuffer.BaseStationBuffer.ltrHealth / 16;
            string str;
            if (basestationstate == 1)
                str = "Internal GPS";
            else if (basestationstate == 2)
                str = "External GPS";
            else
                str = "Manual";
            if ((databuffer.BaseStationBuffer.ltrHealth & 0xF) != 0)
            {
                item.Cells[0].Value = databuffer.BaseStationBuffer.stationNumber;
                item.Cells[1].Value = "BTS#" + databuffer.BaseStationBuffer.stationNumber.ToString();
                item.Cells[2].Value = databuffer.BaseStationBuffer.stationMACNumber.ToString();
                item.Cells[3].Value = databuffer.BaseStationBuffer.voltage;
                item.Cells[4].Value = databuffer.BaseStationBuffer.current;
                item.Cells[5].Value = databuffer.BaseStationBuffer.stability;
                item.Cells[6].Value = databuffer.BaseStationBuffer.lastLocktoGPS;
                item.Cells[7].Value = databuffer.BaseStationBuffer.rssiBaseStation;
                item.Cells[8].Value = databuffer.BaseStationBuffer.rssiCenterStation;
                item.Cells[9].Value = databuffer.BaseStationBuffer.temperature;
                item.Cells[10].Value = databuffer.BaseStationBuffer.batteryCharge;
                item.Cells[11].Value = str;
                item.Cells[12].Value = "View Details";

                foreach (GridViewCellInfo cell in item.Cells)
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.DrawFill = true;
                    cell.Style.BackColor = Color.Lime;
                }

                var formidx = BaseStationFormList.FindIndex(x => x.data.stationNumber == databuffer.BaseStationBuffer.stationNumber);                
                if (formidx >= 0)
                    BaseStationFormList[formidx].UpdateGUI(databuffer.BaseStationBuffer);
            }
            else
            {
                item.Cells[0].Value = databuffer.BaseStationBuffer.stationNumber;
                item.Cells[1].Value = "BTS#" + databuffer.BaseStationBuffer.stationNumber.ToString();
                item.Cells[2].Value = databuffer.BaseStationBuffer.stationMACNumber.ToString();
                item.Cells[3].Value = databuffer.BaseStationBuffer.voltage;
                item.Cells[4].Value = databuffer.BaseStationBuffer.current;
                item.Cells[5].Value = databuffer.BaseStationBuffer.stability;
                item.Cells[6].Value = databuffer.BaseStationBuffer.lastLocktoGPS;
                item.Cells[7].Value = databuffer.BaseStationBuffer.rssiBaseStation;
                item.Cells[8].Value = databuffer.BaseStationBuffer.rssiCenterStation;
                item.Cells[9].Value = databuffer.BaseStationBuffer.temperature;
                item.Cells[10].Value = databuffer.BaseStationBuffer.batteryCharge;
                item.Cells[11].Value = str;
                item.Cells[12].Value = "Data Not Valid";

                foreach (GridViewCellInfo cell in item.Cells)
                {
                    cell.Style.CustomizeFill = true;
                    cell.Style.DrawFill = true;
                    cell.Style.BackColor = Color.Red;
                }
            }

        }

        private void AddBlinkingCell(int RowNumber)
        {
            if (BlinkingList.Exists(x => (x == RowNumber)))
            {

            }
            else
            {
                BlinkingList.Add(RowNumber);                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SingleDataBuffer d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 1;
            d.BaseStationBuffer.humidity = 0.5;
            d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            d.BaseStationBuffer.ltrHealth = 34;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stability = 1;
            d.BaseStationBuffer.stationNumber = 3;
            d.BaseStationBuffer.temperature = 23;
            d.BaseStationBuffer.voltage = 12;
            d.BaseStationBuffer.SNRs = new double[] { 10, 10, 10, 10, 10, 10 };
            UpdateInfo(d);
            d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 100;
            d.BaseStationBuffer.humidity = 1.5;
            d.BaseStationBuffer.lastLocktoGPS = 0;
            d.BaseStationBuffer.ltrHealth = 16;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stationNumber = 4;
            UpdateInfo(d);
            d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = 1;
            d.BaseStationBuffer.current = 100;
            d.BaseStationBuffer.humidity = 1.5;
            d.BaseStationBuffer.lastLocktoGPS = 0;
            d.BaseStationBuffer.ltrHealth = 16;
            d.BaseStationBuffer.rssiBaseStation = 1;
            d.BaseStationBuffer.rssiCenterStation = 2;
            d.BaseStationBuffer.stationNumber = 5;
            UpdateInfo(d);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SingleDataBuffer d = new SingleDataBuffer();
            d.hasBaseStationInfo = true;
            d.BaseStationBuffer = new BaseStationInfo();
            d.BaseStationBuffer.batteryCharge = rnd.Next(50);
            d.BaseStationBuffer.current = rnd.Next(50);
            d.BaseStationBuffer.humidity = rnd.Next(50);
            d.BaseStationBuffer.lastLocktoGPS = rnd.Next(50);
            d.BaseStationBuffer.ltrHealth = 34;
            d.BaseStationBuffer.rssiBaseStation = rnd.Next(50);
            d.BaseStationBuffer.rssiCenterStation = rnd.Next(50);
            d.BaseStationBuffer.stability = rnd.Next(50);
            d.BaseStationBuffer.stationNumber = 3;
            d.BaseStationBuffer.temperature = rnd.Next(50);
            d.BaseStationBuffer.voltage = rnd.Next(50);
            d.BaseStationBuffer.x = rnd.Next(10000);
            d.BaseStationBuffer.y = rnd.Next(10000);
            d.BaseStationBuffer.z = rnd.Next(10000);
            d.datetime = DateTime.Now;
            d.BaseStationBuffer.SNRs = new double[] { rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50) };
            UpdateInfo(d);
                        
            if (cycle)
            {
                for (int i = BlinkingList.Count-1; i >= 0; i--)
                {
                    RowColor.Add(radGridView1.Rows[BlinkingList[i]].Cells[0].Style.BackColor);
                    radGridView1.Rows[BlinkingList[i]].Cells[0].Style.BackColor = Color.White;
                    BlinkingList2.Add(BlinkingList[i]);
                    BlinkingList.Remove(i);
                }
                cycle = false;
            }
            else
            {
                for (int i = BlinkingList2.Count-1; i >= 0; i--)
                {
                    radGridView1.Rows[BlinkingList2[i]].Cells[0].Style.BackColor = RowColor[i];
                    BlinkingList2.RemoveAt(i);
                    RowColor.RemoveAt(i);
                }
                cycle = true;
            }
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 12 && e.Row.Cells[1].Style.BackColor != Color.Red)
            {
                var stationnumber = Convert.ToInt32(e.Row.Cells[0].Value);
                var idx = BaseStationList.FindIndex(x => x.stationNumber == stationnumber);
                ShowInfo(BaseStationList[idx]);
                //Row.Cells[0].Style.BackColor.ToString());
            }
        }

        public void ShowInfo(BaseStationInfo basestation)
        {
            var name = "BTS#" + basestation.stationNumber.ToString();

            int idx = -1;
            for (int i = 0; i < BaseStationFormList.Count; i++)
                if (BaseStationFormList[i].data.stationNumber == basestation.stationNumber)
                    idx = i;
            int formidx = -1;
            for (int i = 0; i < Parentform.radDock1.DocumentManager.DocumentArray.Length; i++)
            {
                if (Parentform.radDock1.DocumentManager.DocumentArray[i].Text == name)
                {
        
                    formidx = i;
                }
            }
            if (idx == -1)
            {
                //not found. make a new page for it
                var BaseStationForm = new BaseStation();
                BaseStationForm.ShowBaseStation(basestation);
                BaseStationForm.Dock = DockStyle.None;
                BaseStationForm.TopLevel = false;
                BaseStationForm.Show();
                BaseStationForm.data = basestation;
                BaseStationFormList.Add(BaseStationForm);
                var NewDockWindow = new Telerik.WinControls.UI.Docking.DocumentWindow(name);
                NewDockWindow.AutoScroll = true;
                NewDockWindow.Controls.Add(BaseStationForm);
                Parentform.radDock1.AddDocument(NewDockWindow);
            }
            else if (formidx == -1)
            {
                var BaseStationForm = new BaseStation();
                BaseStationForm.ShowBaseStation(basestation);
                BaseStationForm.Dock = DockStyle.None;
                BaseStationForm.TopLevel = false;
                BaseStationForm.Show();
                BaseStationForm.data = basestation;
                BaseStationFormList[idx] = BaseStationForm;
                var NewDockWindow = new Telerik.WinControls.UI.Docking.DocumentWindow(name);
                NewDockWindow.AutoScroll = true;
                NewDockWindow.Controls.Add(BaseStationFormList[idx]);
                Parentform.radDock1.AddDocument(NewDockWindow);
                //Parentform.radDock1.ad
            }
            else
            {
                BaseStationFormList[idx].UpdateGUI(basestation);
                while (Parentform.radDock1.DocumentManager.ActiveDocument.Text != name)
                {
                    Parentform.radDock1.DocumentManager.ActivateNextDocument();
                }
            }
        }

        public void ShowTime(DateTime toshow)
        {
            label12.Text = "Time: " + toshow.ToLongTimeString();
            DateLabel.Label.FormatString = toshow.Month.ToString("D2") + "/" + toshow.Day.ToString("D2") + "/" + toshow.Year.ToString();
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[0].Markers[0].Value = toshow.Hour + (double)toshow.Minute / 60;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[1].Markers[0].Value = toshow.Minute;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[2].Markers[0].Value = toshow.Second;
        }
    }
}
