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
        public List<BaseStationInfo> BaseStationList = new List<BaseStationInfo>();
        public List<BaseStation> BaseStationFormList = new List<BaseStation>();

        public List<Color> RowColor = new List<Color>();
        public List<int> BlinkingList = new List<int>();
        public List<int> BlinkingList2 = new List<int>();
        public DateTime datetime;
        public bool ackRecivedFlag = true;
        public bool searching = false;
        public int datatimeout = -1;
        public bool alive = false;

        int[,] conditionThreshoulds =
        {
            {5,10,15,20},  // No
            {5,10,15,20},  // MAC
            {5,10,15,20},  // Name
            {10,11,13,14},  // Voltage
            {1,2,3,4},  // Current
            {0,1,2,3},  // Stability
            {50,100,150,200},  // Last Locked
            {-100,-90,-40,-30},  // RSSI Base Station
            {-100,-90,-40,-30},  // RSSI Central Station
            {-20,10,55,85},  // Temprature
            {40,70,100,120}   // Battery
        };
        int ackTimeOutCounter = 0;//, maxDetectedBaseStationNumber = 10;


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


            //BaseStationList = new List<BaseStationInfo>();
            //BaseStationFormList = new List<BaseStation>();
            //SingleDataBuffer d = new SingleDataBuffer();
            //d.hasBaseStationInfo = true;
            //d.BaseStationBuffer = new BaseStationInfo();            
            //d.BaseStationBuffer.batteryCharge = 1;
            //d.BaseStationBuffer.current = 1;
            //d.BaseStationBuffer.humidity = 0.5;
            //d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            //d.BaseStationBuffer.ltrHealth = 34;
            //d.BaseStationBuffer.rssiBaseStation = 1;
            //d.BaseStationBuffer.rssiCenterStation = 2;
            //d.BaseStationBuffer.stability = 1;
            //d.BaseStationBuffer.stationNumber = 3;
            //d.BaseStationBuffer.temperature = 23;
            //d.BaseStationBuffer.voltage = 12;
            //d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            //UpdateInfo(d);
            //d = new SingleDataBuffer();
            //d.hasBaseStationInfo = true;
            //d.BaseStationBuffer = new BaseStationInfo();
            //d.BaseStationBuffer.batteryCharge = 1;
            //d.BaseStationBuffer.current = 1;
            //d.BaseStationBuffer.humidity = 0.5;
            //d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            //d.BaseStationBuffer.ltrHealth = 34;
            //d.BaseStationBuffer.rssiBaseStation = 1;
            //d.BaseStationBuffer.rssiCenterStation = 2;
            //d.BaseStationBuffer.stationNumber = 4;
            //d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            //UpdateInfo(d);
            //d = new SingleDataBuffer();
            //d.hasBaseStationInfo = true;
            //d.BaseStationBuffer = new BaseStationInfo();
            //d.BaseStationBuffer.batteryCharge = 1;
            //d.BaseStationBuffer.current = 1;
            //d.BaseStationBuffer.humidity = 0.5;
            //d.BaseStationBuffer.lastLocktoGPS = 1524.2;
            //d.BaseStationBuffer.ltrHealth = 34;
            //d.BaseStationBuffer.rssiBaseStation = 1;
            //d.BaseStationBuffer.rssiCenterStation = 2;
            //d.BaseStationBuffer.stationNumber = 5;
            //d.datetime = DateTime.Now;
            //d.BaseStationBuffer.SNRs = new double[] { 32, 54, 36, 53, 48, 49 };
            //UpdateInfo(d);


            //radGridView1.Rows.Add(new object[] { 1, "1", "2", "3", "4", "5", "6", "7", "View Details" });
        }

        public void UpdateInfo(SingleDataBuffer databuffer)
        {
            datetime = databuffer.datetime;
            Showtime(datetime);
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
            toAdd.connectionAcount = Functions.MAX_CONNECTION_ACOUNT;
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
            databuffer.BaseStationBuffer.connectionAcount = Functions.MAX_CONNECTION_ACOUNT;
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

                item.Cells[0].Style.CustomizeFill = true;
                item.Cells[0].Style.DrawFill = true;
                item.Cells[0].Style.BackColor = Color.Lime;
                item.Cells[1].Style.CustomizeFill = true;
                item.Cells[1].Style.DrawFill = true;
                item.Cells[1].Style.BackColor = Color.Lime;
                item.Cells[11].Style.CustomizeFill = true;
                item.Cells[11].Style.DrawFill = true;
                item.Cells[11].Style.BackColor = Color.Lime;
                item.Cells[12].Style.CustomizeFill = true;
                item.Cells[12].Style.DrawFill = true;
                item.Cells[12].Style.BackColor = Color.Lime;

                for (int i = 2; i < 11; i++)
                {
                    item.Cells[i].Style.CustomizeFill = true;
                    item.Cells[i].Style.DrawFill = true;
                    var val = (int)Convert.ToDouble(item.Cells[i].Value);
                    if ((val >= conditionThreshoulds[index, 1]) && (val <= conditionThreshoulds[index, 2]))
                        item.Cells[i].Style.BackColor = Color.Lime;
                    else if (((val >= conditionThreshoulds[index, 0]) && (val < conditionThreshoulds[index, 1])) ||
                            ((val > conditionThreshoulds[index, 2]) && (val <= conditionThreshoulds[index, 3])))
                        item.Cells[i].Style.BackColor = Color.Yellow;
                    else
                        item.Cells[i].Style.BackColor = Color.DarkOrange;
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
                var formidx = BaseStationFormList.FindIndex(x => x.data.stationNumber == databuffer.BaseStationBuffer.stationNumber);
                if (formidx >= 0)
                    BaseStationFormList[formidx].UpdateGUI(databuffer.BaseStationBuffer);
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


        public void SendRessetingAutomaticSearchCommand(int number, int min, int max, char delayinterval)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.AUTOMATIC_SEARCH_ENABLE_CMD;

                index++;

                //minimum earch index
                Msg[index] = (char)min;
                index++;

                //maximum search index
                Msg[index] = (char)max;
                index++;

                //Search Mode ('R' for resseting and 'A' for appending)
                Msg[index] = 'R';
                index++;

                //Search time interval
                Msg[index] = delayinterval;
                index++;

                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];



                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendAppendingAutomaticSearchCommand(int number, int min, int max, char delayinterval)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.AUTOMATIC_SEARCH_ENABLE_CMD;

                index++;

                //minimum earch index
                Msg[index] = (char)min;
                index++;

                //maximum search index
                Msg[index] = (char)max;
                index++;

                //Search Mode ('R' for resseting and 'A' for appending)
                Msg[index] = 'A';
                index++;

                //Search time interval
                Msg[index] = delayinterval;
                index++;

                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendCancelSearchCommand(int number)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.AUTOMATIC_SEARCH_DIABLE_CMD;
                index++;

                //minimum earch index
                Msg[index] = (char)0;
                index++;

                //maximum search index
                Msg[index] = (char)15;
                index++;

                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //SingleDataBuffer d = new SingleDataBuffer();
            //d.hasBaseStationInfo = true;
            //d.BaseStationBuffer = new BaseStationInfo();
            //d.BaseStationBuffer.batteryCharge = rnd.Next(50);
            //d.BaseStationBuffer.current = rnd.Next(50);
            //d.BaseStationBuffer.humidity = rnd.Next(50);
            //d.BaseStationBuffer.lastLocktoGPS = rnd.Next(50);
            //d.BaseStationBuffer.ltrHealth = 34;
            //d.BaseStationBuffer.rssiBaseStation = rnd.Next(50);
            //d.BaseStationBuffer.rssiCenterStation = rnd.Next(50);
            //d.BaseStationBuffer.stability = rnd.Next(50);
            //d.BaseStationBuffer.stationNumber = 3;
            //d.BaseStationBuffer.temperature = rnd.Next(50);
            //d.BaseStationBuffer.voltage = rnd.Next(50);
            //d.BaseStationBuffer.x = rnd.Next(10000);
            //d.BaseStationBuffer.y = rnd.Next(10000);
            //d.BaseStationBuffer.z = rnd.Next(10000);
            //d.datetime = DateTime.Now;
            //d.BaseStationBuffer.SNRs = new double[] { rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50), rnd.Next(50) };
            //UpdateInfo(d);
            if (datatimeout > -1)
            {
                datatimeout--;
                if (!alive)
                {
                    alive = true;
                    toolStripStatusLabel1.BackColor = Color.Lime;
                    toolStripStatusLabel1.Text = "Receiving Data";
                }
            }
            else
            {
                if (alive)
                {
                    alive = false;
                    toolStripStatusLabel1.BackColor = Color.Salmon;
                    toolStripStatusLabel1.Text = "No Data";
                    Parentform.WriteText("");
                }
            }

            if (cycle)
            {
                for (int i = BlinkingList.Count - 1; i >= 0; i--)
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
                for (int i = BlinkingList2.Count - 1; i >= 0; i--)
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
            if (e.RowIndex >= 0 && e.ColumnIndex == 12)// && e.Row.Cells[1].Style.BackColor != Color.Red)
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
                var BaseStationForm = new BaseStation(this);
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
                var BaseStationForm = new BaseStation(this);
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

        delegate void SetTextCallback(DateTime toshow);

        private void Showtime(DateTime toshow)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Showtime);
                    this.Invoke(d, new object[] { toshow });
                }
                else
                {
                    label12.Text = "Time: " + toshow.ToLongTimeString();
                    DateLabel.Label.FormatString = toshow.Month.ToString("D2") + "/" + toshow.Day.ToString("D2") + "/" + toshow.Year.ToString();
                    ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[0].Markers[0].Value = toshow.Hour + (double)toshow.Minute / 60;
                    ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[1].Markers[0].Value = toshow.Minute;
                    ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[2].Markers[0].Value = toshow.Second;
                }
            }
            catch { }
        }

        public void SendChangePosCommand(int number,double lat, double longi,double alt)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;
                Int32 result;
                char[] xyz = new char[4];

                XYZpoint xyzpos = new XYZpoint();
                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.BASE_STATION_SET_POS_CMD;
                index++;
                //set base station number

                Msg[index] = (char)number;
                index++;
                xyzpos = Functions.Calculate_XYZ_From_LatLongAlt(lat, longi, alt);

                result = (Int32)(xyzpos.x * 100);

                for (int i = 0; i < 4; i++)
                {
                    Msg[index] = (char)(result & 0xFF);
                    result >>= 8;
                    index++;
                }

                result = (Int32)(xyzpos.y * 100);

                for (int i = 0; i < 4; i++)
                {
                    Msg[index] = (char)(result & 0xFF);
                    result >>= 8;
                    index++;
                }

                result = (Int32)(xyzpos.z * 100);

                for (int i = 0; i < 4; i++)
                {
                    Msg[index] = (char)(result & 0xFF);
                    result >>= 8;
                    index++;
                }

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }
            ackRecivedFlag = false;   
        }

        public void SendChangePosModeCommand(int number, int ind)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index=0;
                char[] xyz = new char[4];
                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.CHANGE_BASE_STATION_POSITIONING_MODE_CMD;
                index++;
                //set base station number

                Msg[index] = (char)number;
                index++;

                Msg[index] = (char)ind;
                index++;


                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);                   
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }
            ackRecivedFlag = false;
        }

        public void SendChangeNumberCommand(int currentnumber, int afterchange)
        {
            DialogResult messageBoxShow = MessageBox.Show("Are You Sure Want to Change BaseStation Number?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (messageBoxShow == DialogResult.Yes)
            {
                try
                {
                    char[] Msg = new char[60];
                    byte[] byteMsg = new byte[60];
                    int index = 0;
                    //command 1: 
                    //Header
                    Msg[index] = Functions.MSG_Header[0];
                    index++;
                    Msg[index] = Functions.MSG_Header[1];
                    index++;
                    Msg[index] = Functions.MSG_Header[2];
                    index++;
                    Msg[index] = Functions.MSG_Header[3];
                    index++;

                    //CMD
                    Msg[index] = Functions.BASE_STATION_CHANGE_NUMBER_CMD;
                    index++;
                    //set base station number

                    Msg[index] = (char)currentnumber;
                    index++;

                    //set new baseStation Number
                    Msg[index] = (char)afterchange;
                    index++;

                    //CRC
                    Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                    index++;

                    for (int i = 0; i < index; i++)
                        byteMsg[i] = (byte)Msg[i];

                    for (int i = 0; i < index; i++)
                    {
                        Parentform.Serial1_Write(byteMsg, i, 1);
                        Thread.Sleep(20);
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error: " + ex.Message);
                }
                ackRecivedFlag = false;
            }
        }

        public void SendChangeRangeCommand(int number, double range)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;


                //command 1: 
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.SET_RANGE_OFFSET_CMD;
                index++;
                //set base station number

                Msg[index] = (char)number;
                index++;
                try
                {
                    int offset = (int)(range * 100);
                    for (int i = 0; i < 3; i++)
                    {
                        Msg[index] = (char)(offset % 256);
                        index++;
                        offset /= 256;
                    }

                }
                catch
                {

                }


                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);                   
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }
            ackRecivedFlag = false;
        }

        public void SendChangeTimeCommand(int number, DateTime time, DateTime date,bool fromGPS)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.BASE_STATION_SET_TIME_CMD;
                index++;

                //set base station number
                Msg[index] = (char)number;
                index++;
                //Set GPS Time
                //Msg[index] = (char)(formBaseStationSetTime.checkEditSetGPSTime.Checked ? 1 : 0);
                //index++;

                //Year
                Msg[index] = (char)(date.Year >> 8);
                index++;
                Msg[index] = (char)(date.Year & 0xff);
                index++;

                Msg[index] = (char)(date.Month);
                index++;

                Msg[index] = (char)(date.Day);
                index++;

                Msg[index] = (char)(time.Hour);
                index++;

                Msg[index] = (char)(time.Minute);
                index++;

                Msg[index] = (char)(time.Second);
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);                    
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }
            ackRecivedFlag = false;
        }

        public void SendResetCommand(int number)
        {
            DialogResult result = MessageBox.Show("Are you sure to Reset BTS#" + number.ToString() + "?", "BaseStation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    char[] Msg = new char[60];
                    byte[] byteMsg = new byte[60];
                    int index = 0;

                    //command 1: 
                    //Header
                    Msg[index] = Functions.MSG_Header[0];
                    index++;
                    Msg[index] = Functions.MSG_Header[1];
                    index++;
                    Msg[index] = Functions.MSG_Header[2];
                    index++;
                    Msg[index] = Functions.MSG_Header[3];
                    index++;
                    Msg[index] = Functions.BASE_STATION_RESET_LTR_CMD;
                    //ackRecivedFlag = false;

                    index++;

                    Msg[index] = (char)number;
                    index++;
                    Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                    index++;

                    for (int i = 0; i < index; i++)
                        byteMsg[i] = (byte)Msg[i];

                    for (int i = 0; i < index; i++)
                    {
                        Parentform.Serial1_Write(byteMsg, i, 1);
                        Thread.Sleep(20);
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
                }
                ackRecivedFlag = false;
            }
        }

        public void SendTurnOnCommand(int number)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.TURN_ON_OFF_LTR_CMD;
                index++;

                Msg[index] = (char)number;
                index++;

                Msg[index] = '1';
                index++;
                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error: " + ex.Message);
            }
            ackRecivedFlag = false;
        }

        public void SendTurnOffCommand(int number)
        {
            DialogResult result = MessageBox.Show("Are you sure to turn off BTS#" + number.ToString()+"?","BaseStation",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    char[] Msg = new char[60];
                    byte[] byteMsg = new byte[60];
                    int index = 0;

                    //Header
                    Msg[index] = Functions.MSG_Header[0];
                    index++;
                    Msg[index] = Functions.MSG_Header[1];
                    index++;
                    Msg[index] = Functions.MSG_Header[2];
                    index++;
                    Msg[index] = Functions.MSG_Header[3];
                    index++;

                    //CMD
                    Msg[index] = Functions.TURN_ON_OFF_LTR_CMD;
                    index++;

                    Msg[index] = (char)number;
                    index++;

                    Msg[index] = '0';
                    index++;
                    //CRC
                    Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                    index++;

                    for (int i = 0; i < index; i++)
                        byteMsg[i] = (byte)Msg[i];

                    for (int i = 0; i < index; i++)
                    {
                        Parentform.Serial1_Write(byteMsg, i, 1);
                        Thread.Sleep(20);
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error: " + ex.Message);
                }
                ackRecivedFlag = false;
            }
        }

        public void SendSaveSettingsCommand(int number)
        {
            try
            {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;

                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;

                //CMD
                Msg[index] = Functions.SAVE_SETTING_CMD;
                index++;

                Msg[index] = (char)number;
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    //SelectedSerialPort.Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
            }
            ackRecivedFlag = false;
        }

        public void SendGetStatusCommand(int number)
        {
             try
             {
                char[] Msg = new char[60];
                byte[] byteMsg = new byte[60];
                int index = 0;
                    
                //Header
                Msg[index] = Functions.MSG_Header[0];
                index++;
                Msg[index] = Functions.MSG_Header[1];
                index++;
                Msg[index] = Functions.MSG_Header[2];
                index++;
                Msg[index] = Functions.MSG_Header[3];
                index++;
                Msg[index] = Functions.BASE_STATION_GET_STATUS_CMD;
                index++;

                Msg[index] = (char)number;
                index++;
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);                       
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
             }
             catch (Exception ex)
             {
                 Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
             }


        }

        private void tmrDetectDisconnectedStation_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < BaseStationList.Count; i++)
            {
                BaseStationList[i].connectionAcount--;
                if (BaseStationList[i].connectionAcount <= 0)        // Remove
                {
                    var number = BaseStationList[i].stationNumber;
                    int index = -1;
                    for (int j = 0; j < radGridView1.RowCount; j++)
                        if (Convert.ToInt16(radGridView1.Rows[j].Cells[0].Value) == number)
                            index = j;
                    if (index != -1)
                        radGridView1.Rows.RemoveAt(index);
                    index = -1;
                    for (int j = 0; j < BaseStationFormList.Count; j++)
                        if (BaseStationFormList[j].BasestationNumber == number)
                            index = j;
                    if (index != -1)
                    {
                        for (int j = 0; j < Parentform.radDock1.DocumentManager.DocumentArray.Length; j++)
                        {
                            if (Parentform.radDock1.DocumentManager.DocumentArray[j].Text.Contains("BTS#" + number.ToString()))
                            {
                                Parentform.radDock1.DocumentManager.DocumentArray[j].Close();
                            }
                        }
                        BaseStationFormList[index].Close();
                        BaseStationFormList.RemoveAt(index);
                    }
                    BaseStationList.RemoveAt(i);

                    // formBaseStationGetStatus.comboBox2.Items.RemoveAt(index);
                }
            }
            ackTimeOutCounter++;
            if (!ackRecivedFlag)
            {
                if (ackTimeOutCounter > 5)
                {
                    ackTimeOutCounter = 0;
                    ackRecivedFlag = true;
                    MessageBox.Show("ACK Signal Not Received", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                ackTimeOutCounter = 0;

        }


        private void AutoSearchBtn_Click(object sender, EventArgs e)
        {
            if (DelayText.Text == "")
                DelayText.Text = "250";
            int delay;
            try
            {
                delay = Convert.ToInt32(DelayText.Text);
            }
            catch
            {
                MessageBox.Show("Input Error");
                return;
            }
            DelayRange r = DelayRange.ms;
            if (sRadio.Checked)
                r = DelayRange.s;
            else if (mRadio.Checked)
                r = DelayRange.m;
            else if (hRadio.Checked)
                r = DelayRange.h;
            var delaychar = Functions.MakeTimeIntervalByte(delay, r);
            SendRessetingAutomaticSearchCommand(1, 1, 10, delaychar);
        }

        private void ManSearchBtn_Click(object sender, EventArgs e)
        {
            //SendAppendingAutomaticSearchCommand(1, 1, 2, 1000);
            if (DelayText.Text == "")
                DelayText.Text = "250";
            int delay;
            try
            {
                delay = Convert.ToInt32(DelayText.Text);
            }
            catch
            {
                MessageBox.Show("Input Error");
                return;
            }
            DelayRange r = DelayRange.ms;
            if (sRadio.Checked)
                r = DelayRange.s;
            else if (mRadio.Checked)
                r = DelayRange.m;
            else if (hRadio.Checked)
                r = DelayRange.h;
            var delaychar = Functions.MakeTimeIntervalByte(delay, r);
            if (ManualSearchText.Text != "")
            {
                var list = ProcessString(ManualSearchText.Text);
                bool first = true;
                foreach (SearchRange range in list)
                {
                    if (first)
                    {
                        SendRessetingAutomaticSearchCommand(1, range.start, range.end, delaychar);
                        first = false;
                    }
                    else
                        SendAppendingAutomaticSearchCommand(1, range.start, range.end, delaychar);
                }
            }
        }

        private List<SearchRange> ProcessString(string input)
        {
            List<SearchRange> outlist = new List<SearchRange>();

            List<string> substrs = new List<string>();

            List<int> starts = new List<int>();
            List<int> ends = new List<int>();
            int start = 0;
            for (int i = 0; i < input.Length; i++)
                if (input[i] == ',')
                {
                    starts.Add(start);
                    ends.Add(i - 1);
                    start = i + 1;
                }

            if (start < input.Length)
            {
                starts.Add(start);
                ends.Add(input.Length-1);
            }
            if (starts.Count == 0)
            {
                starts.Add(0);
                ends.Add(input.Length);
            }

            for (int i = 0; i < starts.Count; i++)
            {
                string tmp = new string(input.ToCharArray(starts[i],ends[i]-starts[i]+1));
                substrs.Add(tmp);
            }

            foreach (string s in substrs)
            {
                SearchRange temp = new SearchRange();
                if (s.Contains('-'))
                {
                    var dashidx = s.IndexOf('-');
                    int strt = Convert.ToInt32(new string(s.ToCharArray(0, dashidx)));
                    int end = Convert.ToInt32(new string(s.ToCharArray(dashidx + 1, s.Length - dashidx - 1)));
                    temp.start = strt;
                    temp.end = end;
                    temp.length = end - start + 1;
                }
                else
                {
                    temp.start = temp.end = Convert.ToInt32(s);
                    temp.length = 1;
                }
                outlist.Add(temp);
            }


            return outlist;
        }

        private void BTSDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        public void CancelSearches()
        {
            try
            {
                SendCancelSearchCommand(0);
            }
            catch
            {
            }
        }

        private void CancelSearchBtn_Click(object sender, EventArgs e)
        {
            CancelSearches();
        }
    }
}
