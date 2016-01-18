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

        public List<Color> RowColor = new List<Color>();
        public List<int> BlinkingList = new List<int>();
        public List<int> BlinkingList2 = new List<int>();

        bool cycle = true;

        public BTSDetail(Form1 parent)
        {
            InitializeComponent();
            BaseStationList = new List<BaseStationInfo>();
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
            UpdateInfo(d);


            //radGridView1.Rows.Add(new object[] { 1, "1", "2", "3", "4", "5", "6", "7", "View Details" });
        }

        public void UpdateInfo(SingleDataBuffer databuffer)
        {           
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
                UpdateBTSInfo(index);
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

        public void UpdateBTSInfo(int index)
        {

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
            d.BaseStationBuffer.ltrHealth = 16;
            d.BaseStationBuffer.stationNumber = 4;
            UpdateInfo(d);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
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
                AddBlinkingCell(0);
            }
        }
    }
}
