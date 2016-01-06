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
using System.Windows.Forms.DataVisualization.Charting;
using GPS;


namespace GPSNavigator
{
    public partial class NorthDetail : Form
    {
        GPSData data;
        List<GPSData> CacheData = new List<GPSData>();
        List<GPSData> CacheData2 = new List<GPSData>();
        LogFileManager filemanager;
        float position;
        int IndexCounter = 0, SlowCounter = 0, VisibleGPS, VisibleGLONASS, UsedGPS, UsedGLONASS, Chart1Item, Chart2Item = -1, DataTimeOut = 100;
        double Xp = 0, Yp = 0, Zp = 0, AltP = 0, LatP = 0, LonP = 0;
        bool playing = false, reading = false, returned = false, realtime = false, Receiving = false, StartedCounting = false, ShowingGraph = false, ShowingControlPanel = false, juststarted = true;
        public bool paused = false,notshowdata = false;
        public int index;
        PlaybackSpeed playspeed = PlaybackSpeed.NormalSpeed;
        DateTime StartTime, EndTime,previousTime;
        Infragistics.UltraGauge.Resources.EllipseAnnotation DateLabel;
        Form1 Parentform;// = new Form1();
        SettingBuffer Settings = new SettingBuffer();
        Task<bool> updater;
        graphtype RealtimeGraphType = graphtype.X;
        optionfor clickeditem;

        public NorthDetail(LogFileManager manager)
        {
            filemanager = manager;
            InitializeComponent();
            toolStripSplitButton1.Visible = false;
            //this.Text = "MomentDetail (Log)";
            toolStripStatusLabel1.Visible = false;
            ControlPanel.Visible = true;
            absoluteRadio.Visible = false;
            relativeRadio.Visible = false;
            ControlPanelButton.Visible = false;
            GraphToggle.Visible = false;
            graphDataCombo.Visible = false;
            ClearButton.Visible = false;

            DateLabel = ultraGaugeClock.Annotations[0] as Infragistics.UltraGauge.Resources.EllipseAnnotation;
            chart1.Series[0].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            ReadCache(0f,true);
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

        public NorthDetail(Form1 Parent)
        {
            InitializeComponent();
            //ToggleGraph();
            toolStripSplitButton1.Visible = true;
            this.Text = "MomentDetail (RealTime)";
            ClearGraph();
            Parentform = Parent;
            toolStripStatusLabel1.Visible = true;
            ControlPanel.Visible = false;
            ControlPanelButton.Visible = true;
            GraphToggle.Visible = true;
            DateLabel = ultraGaugeClock.Annotations[0] as Infragistics.UltraGauge.Resources.EllipseAnnotation;
            chart1.Series[0].XValueType = ChartValueType.String;
            realtime = true;
            //HideSecondGraph();
            PositionTypeCombo.SelectedIndex = 2;
            GraphTooltip.SetToolTip(c1Chart1, "Shift+Click = Scroll, CTRL+Click = Scale, ALT+Click = Zoom, RightClick = Options");
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

            switch (data.AttitudeBuffer.AttitudeState)
            {
                case 0:
                    StatLabel.Text = "State: Waiting for Minimum Required Satellites";
                    StatLabel.ForeColor = Color.Yellow;
                    AmbiguityLabel.Text = "Remaining ambiguity: " + data.AttitudeBuffer.Ambiguity.ToString();
                    break;
                case 1:
                    StatLabel.Text = "State: Waiting for More Satellites";
                    StatLabel.ForeColor = Color.Yellow;
                    AmbiguityLabel.Text = "Remaining ambiguity: " + data.AttitudeBuffer.Ambiguity.ToString();
                    break;
                case 2:
                    StatLabel.Text = "State: Ambiguity Resolution";
                    StatLabel.ForeColor = Color.Yellow;
                    AmbiguityLabel.Text = "Remaining ambiguity: " + data.AttitudeBuffer.Ambiguity.ToString();
                    break;
                case 3:
                    StatLabel.Text = "State: Realtime Process";
                    StatLabel.ForeColor = Color.GreenYellow;
                    AmbiguityLabel.Text = "Remaining ambiguity: 0";
                    break;
            }

            Xp = data.X_Processed;
            Yp = data.Y_Processed;
            Zp = data.Z_Processed;
            AltP = data.Altitude_Processed;
            LonP = data.Longitude_Processed;
            LatP = data.Latitude_Processed;
            UpdateNorthfinderLabels(data);
                
                //clear_NorthFinder_Components();
            tempgpsdata.Time = vars.PacketTime;
            tempgpsdata.Dbuf = data;
            try
            {
                PlotGraph(tempgpsdata);
            }
            catch
            {
            }
        }

        public void UpdateNorthfinderLabels(SingleDataBuffer data)
        {
            //if (data.state == 1)
            //{
                if (data.AttitudeBuffer.AttitudeState == 3)                 //Real Time Process
                {
                    notshowdata = true;
                    headingIndicator1.SetHeadingIndicatorParameters((int)data.AttitudeBuffer.Azimuth);// attitudeInfoDataBuf.Azimuth[attitudeInfoDataBuf.counter]);
                    AzimuthLabel.Text = "Azimuth = " + data.AttitudeBuffer.Azimuth.ToString("F2");

                    //Elevtaion
                    attitudeIndicator1.SetAttitudeIndicatorParameters(data.AttitudeBuffer.Elevation, 0);//attitudeInfoDataBuf.Elevation[attitudeInfoDataBuf.counter], 0);
                    ElevationLabel.Text = "Elevtaion = " + data.AttitudeBuffer.Elevation.ToString("F2");


                    double X = data.AttitudeBuffer.X + Xp;
                    double Y = data.AttitudeBuffer.Y + Yp;
                    double Z = data.AttitudeBuffer.Z + Zp;

                    var point = Functions.Calculate_LatLongAlt_From_XYZ(X, Y, Z);

                    if (absoluteRadio.Checked)// tileItemLatLong2.Elements[4].Text != "Relative")
                    {
                        LatitudeValue.Text = point.Latitude.ToString("#0.000000");
                        LongitudeValue.Text = point.Longitude.ToString("#0.000000");
                        AltitudeValue.Text = point.Altitude.ToString("#0.00");
                        //Update_Latitude_Labels(tileItemLatLong2, point.Latitude);
                        //Update_Longitude_Labels(tileItemLatLong2, point.Longitude);
                        //Show_Altitude(tileItemLatLong2, point.Altitude);
                    }
                    else
                    {
                        XYZpoint res1 = Functions.Calculate_XYZ_From_LatLongAlt(LatP, point.Longitude, point.Altitude);
                        XYZpoint res = new XYZpoint();
                        res.x = Xp - res1.x;
                        res.y = Yp - res1.y;
                        res.z = Zp - res1.z;
                        double d = Math.Sqrt(Math.Pow(res.x, 2) + Math.Pow(res.y, 2) + Math.Pow(res.z, 2));
                        LatitudeValue.Text = d.ToString("F3") + "m";

                        res1 = Functions.Calculate_XYZ_From_LatLongAlt(point.Latitude, LonP, point.Altitude);
                        res = new XYZpoint();
                        res.x = Xp - res1.x;
                        res.y = Yp - res1.y;
                        res.z = Zp - res1.z;
                        d = Math.Sqrt(Math.Pow(res.x, 2) + Math.Pow(res.y, 2) + Math.Pow(res.z, 2));
                        LongitudeValue.Text = d.ToString("F3") + "m";

                        d = AltP - point.Altitude;
                        AltitudeValue.Text = d.ToString("F3") + "m";
                    }

                    //Distance
                    ((LinearGauge)ultraGaugeDistance.Gauges[0]).Scales[0].Markers[0].Value = data.AttitudeBuffer.Distance;
                    distanceLabel.Text = "Distance : " + data.AttitudeBuffer.Distance.ToString("F2");
                }
                else { notshowdata = false; }
                if (playing)
                {
                    headingIndicator1.SetHeadingIndicatorParameters((int)data.AttitudeBuffer.Azimuth);
                    AzimuthLabel.Text = "Azimuth = " + data.AttitudeBuffer.Azimuth.ToString("F2");
                    attitudeIndicator1.SetAttitudeIndicatorParameters(data.AttitudeBuffer.Elevation, 0);
                    ElevationLabel.Text = "Elevation = " + data.AttitudeBuffer.Elevation.ToString("F2");
                    ((LinearGauge)ultraGaugeDistance.Gauges[0]).Scales[0].Markers[0].Value = data.AttitudeBuffer.Distance;
                    distanceLabel.Text = "Distance : " + data.AttitudeBuffer.Distance.ToString("F2");
                }
            //}
        }

        public void UpdateRealtimeGraph(Globals vars, SingleDataBuffer data, int ChannelNum)
        {
            Receiving = true;
            DataTimeOut = 100;
            var tempgpsdata = new GPSData();
            tempgpsdata.Time = vars.PacketTime;
            tempgpsdata.Dbuf = data;
            try
            {
                if (ShowingGraph)
                    UpdateGraph(tempgpsdata, false,ChannelNum);
                else
                    UpdateGraph(tempgpsdata, true, ChannelNum);
            }
            catch { }
        }

        public void UpdateData(double xpos, DateTime time)
        {
            try
            {
                position = (float)xpos;
                data = filemanager.ReadGPSstatus(position);
                ReadCache(position, false);
                updateLabels(time, 0, 0, 0, 0, data);
                PlotGraph(data);
            }
            catch
            {
            }
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

        public void updateLabels(DateTime dt, int vgps, int ugps, int vglonass, int uglonass, GPSData data)// float PDOP, float Altitude, float Longitude, float Latitude,float Velocity)
        {
            if (vgps != 0 || ugps != 0 || vglonass != 0 || uglonass != 0)
            {
                label2.Text = vgps.ToString();
                label4.Text = ugps.ToString();
                label6.Text = vglonass.ToString();
                label8.Text = uglonass.ToString();
            }
            if (!StartedCounting && (vgps + vglonass) == 4)
            {
                StartedCounting = true;
                StartTime = DateTime.Now;
                StartTimeLabel.Text = StartTime.ToLongTimeString();
                EndTimeLabel.Text = "";
            }
            else if (StartedCounting && (ugps + uglonass) > 3)
            {
                StartedCounting = false;
                EndTime = DateTime.Now;
                EndTimeLabel.Text = EndTime.ToLongTimeString();
            }



            PDOPValue.Text = data.Dbuf.PDOP.ToString("#0.00");
            try
            {
                HDOPValue.Text = data.Dbuf.HDOP.ToString();
                TDOPValue.Text = data.Dbuf.TDOP.ToString();
                VDOPValue.Text = data.Dbuf.VDOP.ToString();
            }
            catch { }
            if (!notshowdata)
            {
                LatitudeValue.Text = data.Dbuf.Latitude.ToString("#0.000000");
                LongitudeValue.Text = data.Dbuf.Longitude.ToString("#0.000000");
                AltitudeValue.Text = data.Dbuf.Altitude.ToString("#0.00");
            }
            velLabel.Text = "Velocity: " + data.Dbuf.V.ToString("#0.000");
            label12.Text = "Time: " + dt.ToLongTimeString();
            DateLabel.Label.FormatString = dt.Month.ToString("D2") + "/" + dt.Day.ToString("D2") + "/" + dt.Year.ToString();
            SpeedGauge.Needles[0].Value = (float)data.Dbuf.V;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[0].Markers[0].Value = dt.Hour + (double)dt.Minute / 60;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[1].Markers[0].Value = dt.Minute;
            ((RadialGauge)ultraGaugeClock.Gauges[0]).Scales[2].Markers[0].Value = dt.Second;
            //label14.Text = "Position: " + position.ToString();
        }

        public void ReadCache(float position, bool firsttime)
        {
            if (firsttime)
                CacheData2 = filemanager.ReadGPSCache(position);
            else
            {
                AsyncCaller asynctask = new AsyncCaller(filemanager.ReadGPSCache);
                IAsyncResult asyncresult = asynctask.BeginInvoke(position, null, null);
                //if (asyncresult.IsCompleted)
                try
                {
                    CacheData2 = asynctask.EndInvoke(asyncresult);
                }
                catch
                {
                    throw new Exception("Cache Read Error");
                }
            }
            reading = false;
            returned = true;
        }

        public void PlotGraph(GPSData data)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart tempchart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            int counter = 0, showindex = -1;
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
                                //tempchart.Series[0].Points[counter].Color = Color.Blu;
                                //tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Emboss";
                                if (i >= 32)
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Wedge";
                                    tempchart.Series[0].Points[counter].Color = Color.DodgerBlue;
                                }
                                else
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                    tempchart.Series[0].Points[counter].Color = Color.Blue;
                                }
                            }
                            catch
                            {
                                tempchart.Series[0].Points.AddXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                if (i >= 32)
                                {
                                    tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].CustomProperties = "DrawingStyle = Wedge";
                                    tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = Color.DodgerBlue;
                                }
                                else
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                    tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = Color.Blue;
                                }
                            }
                            if (i < 32) VisibleGPS++; else VisibleGLONASS++;
                            counter++;
                        }
                        if (tempsat[i].Signal_Status == 2)
                        {
                            try
                            {
                                tempchart.Series[0].Points[counter].SetValueXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                if (i >= 32)
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Wedge";
                                    tempchart.Series[0].Points[counter].Color = Color.LawnGreen;
                                }
                                else
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                    tempchart.Series[0].Points[counter].Color = Color.Green;
                                }
                            }
                            catch
                            {
                                tempchart.Series[0].Points.AddXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), tempsat[i].SNR);
                                tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].Color = Color.Green;
                                if (i >= 32)
                                {
                                    tempchart.Series[0].Points[tempchart.Series[0].Points.Count - 1].CustomProperties = "DrawingStyle = Wedge";
                                    tempchart.Series[0].Points[counter].Color = Color.LawnGreen;
                                }
                                else
                                {
                                    tempchart.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                    tempchart.Series[0].Points[counter].Color = Color.Green;
                                }
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
            if (VisibleGPS + VisibleGLONASS > 14)
            {
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
            }
            else
            {
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            }
            updateLabels(data.Time, VisibleGPS, UsedGPS, VisibleGLONASS, UsedGLONASS,data);
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
                UpdateNorthfinderLabels(toplot.Dbuf);
                UpdateComboBoxes(toplot.GPS.Count);
                switch (playspeed)
                {
                    case PlaybackSpeed.NormalSpeed:
                        IndexCounter += 2;
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
                    ReadCache(position,false);
                    reading = true;
                }
            }
        }

        #region PlaybackFuncs
        private void button1_Click(object sender, EventArgs e)
        {
            if (playing)
            {
                this.Text = "MomentDetail (Log)";
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
            if (ShowingGraph)
            {
                c1Chart1.Location = new Point(131, 331);
                c1Chart1.Width = 732;
                c1Chart1.Height = 338;
            }
        }
        private void ShowSecondGraph()
        {
            chart2.Visible = true;
            label13.Visible = true;
            label13.BringToFront();
            if (ShowingGraph)
            {
                c1Chart1.Location = new Point(131, 520);
                c1Chart1.Width = 732;
                c1Chart1.Height = 200;
            }
            comboBox2.Visible = true;
        }

        public void UpdateGraph(GPSData data, bool NAN,int SerialNumber)
        {
            if (juststarted)
            {
                updater = UpdateGraphAsync(data, NAN,SerialNumber);
                juststarted = false;
            }
            else if (updater.IsCompleted)
                updater = UpdateGraphAsync(data, NAN, SerialNumber);
              
        }

        public Task<bool> UpdateGraphAsync(GPSData data, bool NAN,int SerialNumber)
        {

            return Task.Factory.StartNew(() =>
            {
                double toAdd = 0;
                int seriesIndex = SerialNumber - 1;
                #region typeswitch
                switch (RealtimeGraphType)
                {
                    case graphtype.A:
                        toAdd = data.Dbuf.A;
                        break;
                    case graphtype.Altitude:
                        toAdd = data.Dbuf.Altitude;
                        break;
                    case graphtype.Altitude_p:
                        toAdd = data.Dbuf.Altitude_Processed;
                        break;
                    case graphtype.Ax:
                        toAdd = data.Dbuf.Ax;
                        break;
                    case graphtype.Ay:
                        toAdd = data.Dbuf.Ay;
                        break;
                    case graphtype.Az:
                        toAdd = data.Dbuf.Az;
                        break;
                    case graphtype.Latitude:
                        toAdd = data.Dbuf.Latitude;
                        break;
                    case graphtype.Latitude_p:
                        toAdd = data.Dbuf.Latitude_Processed;
                        break;
                    case graphtype.Longitude:
                        toAdd = data.Dbuf.Longitude;
                        break;
                    case graphtype.Longitude_p:
                        toAdd = data.Dbuf.Longitude_Processed;
                        break;
                    case graphtype.PDOP:
                        toAdd = data.Dbuf.PDOP;
                        break;
                    case graphtype.State:
                        toAdd = data.Dbuf.state;
                        break;
                    case graphtype.Temperature:
                        toAdd = data.Dbuf.Temperature;
                        break;
                    case graphtype.UsedStats:
                        toAdd = data.Dbuf.NumOfUsedSats;
                        break;
                    case graphtype.VisibleStats:
                        toAdd = data.Dbuf.NumOfVisibleSats;
                        break;
                    case graphtype.V:
                        toAdd = data.Dbuf.V;
                        break;
                    case graphtype.V_p:
                        toAdd = data.Dbuf.V_Processed;
                        break;
                    case graphtype.Vx:
                        toAdd = data.Dbuf.Vx;
                        break;
                    case graphtype.Vx_p:
                        toAdd = data.Dbuf.Vx_Processed;
                        break;
                    case graphtype.Vy:
                        toAdd = data.Dbuf.Vy;
                        break;
                    case graphtype.Vy_p:
                        toAdd = data.Dbuf.Vy_Processed;
                        break;
                    case graphtype.Vz:
                        toAdd = data.Dbuf.Vz;
                        break;
                    case graphtype.Vz_p:
                        toAdd = data.Dbuf.Vz_Processed;
                        break;
                    case graphtype.X:
                        toAdd = data.Dbuf.X;
                        break;
                    case graphtype.X_p:
                        toAdd = data.Dbuf.X_Processed;
                        break;
                    case graphtype.Y:
                        toAdd = data.Dbuf.Y;
                        break;
                    case graphtype.Y_p:
                        toAdd = data.Dbuf.Y_Processed;
                        break;
                    case graphtype.Z:
                        toAdd = data.Dbuf.Z;
                        break;
                    case graphtype.Z_p:
                        toAdd = data.Dbuf.Z_Processed;
                        break;
                    case graphtype.Null:
                        toAdd = double.NaN;
                        break;
                    case graphtype.Azimuth:
                        toAdd = data.Dbuf.AttitudeBuffer.Azimuth;
                        break;
                    case graphtype.Elevation:
                        toAdd = data.Dbuf.AttitudeBuffer.Elevation;
                        break;
                }
                #endregion
                if (c1Chart1.ChartGroups[0].ChartData.SeriesList[seriesIndex].Y.Length > 180000)
                    ClearGraph();
                if (data.Time.Year < 3000 && data.Time.Year > 2000)
                {
                    if (!NAN)
                    {
                        switch (SerialNumber)
                        {
                            case 1:
                                if (Serial1check.Checked)
                                {
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[0].X.Add(data.Time);
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.Add(toAdd);
                                }
                                break;
                            case 2:
                                if (Serial2Check.Checked)
                                {
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[1].X.Add(data.Time);
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.Add(toAdd);
                                }
                                break;
                        }
                        previousTime = data.Time;
                    }
                    else if (previousTime.Year < 3000 && previousTime.Year > 2000)
                    {
                        switch (SerialNumber)
                        {
                            case 1:
                                if (Serial1check.Checked)
                                {
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[0].X.Add(previousTime);
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.Add(double.NaN);
                                }
                                break;
                            case 2:
                                if (Serial2Check.Checked)
                                {
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[1].X.Add(previousTime);
                                    c1Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.Add(double.NaN);
                                }
                                break;
                        }
                    }
                }
                return true;
            });
        }

        private void MomentDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (realtime)
                Parentform.checkBox2.Checked = false;
        }

        private void ToggleGraph()
        {
            GraphToggle.BackgroundImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            if (!ShowingGraph)
            {
                c1Chart1.Visible = true;
                graphDataCombo.Visible = true;
                ClearButton.Visible = true;
                ResetZoom.Visible = true;
                Serial1check.Visible = true;
                Serial2Check.Visible = true;
                //c1Chart1.BringToFront();
                if (!ChartVisibleCheck.Checked)
                {
                    c1Chart1.Location = new Point(131, 331);                   
                    c1Chart1.Width = 732;
                    c1Chart1.Height = 338;
                }
                else
                {
                    c1Chart1.Location = new Point(131, 520);
                    c1Chart1.Width = 732;
                    c1Chart1.Height = 200;
                }
                this.Height = 720;
            }
            else
            {
                //c1Chart1.SendToBack();
                c1Chart1.Visible = false;
                graphDataCombo.Visible = false;
                ClearButton.Visible = false;
                ResetZoom.Visible = false;
                Serial1check.Visible = false;
                Serial2Check.Visible = false;
                this.Height = 615;
            }
            ShowingGraph = !ShowingGraph;
        }

        private void ToggleControlPanel()
        {
            if (!ShowingControlPanel)
            {
                ControlPanelButton.Text = "<";
                this.Width = 1220;
            }
            else
            {
                ControlPanelButton.Text = ">";
                this.Width = 895;
            }
            ShowingControlPanel = !ShowingControlPanel;
        }

        private void GraphToggle_Click(object sender, EventArgs e)
        {
            ToggleGraph();
        }

        private void graphDataCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (graphDataCombo.SelectedIndex)
            {
                case 0:
                    RealtimeGraphType = graphtype.X;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.X";
                    break;
                case 1:
                    RealtimeGraphType = graphtype.X_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.X_Processed";
                    break;
                case 2:
                    RealtimeGraphType = graphtype.Y;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Y";
                    break;
                case 3:
                    RealtimeGraphType = graphtype.Y_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Y_Processed";
                    break;
                case 4:
                    RealtimeGraphType = graphtype.Z;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Z";
                    break;
                case 5:
                    RealtimeGraphType = graphtype.Z_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Z_Processed";
                    break;
                case 6:
                    RealtimeGraphType = graphtype.Latitude;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Latitude";
                    break;
                case 7:
                    RealtimeGraphType = graphtype.Latitude_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Latitude_Processed";
                    break;
                case 8:
                    RealtimeGraphType = graphtype.Longitude;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude";
                    break;
                case 9:
                    RealtimeGraphType = graphtype.Longitude_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude_Processed";
                    break;
                case 10:
                    RealtimeGraphType = graphtype.Altitude;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude";
                    break;
                case 11:
                    RealtimeGraphType = graphtype.Altitude_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude_Processed";
                    break;
                case 12:
                    RealtimeGraphType = graphtype.Vx;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vx";
                    break;
                case 13:
                    RealtimeGraphType = graphtype.Vx_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vx_Processed";
                    break;
                case 14:
                    RealtimeGraphType = graphtype.Vy;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vy";
                    break;
                case 15:
                    RealtimeGraphType = graphtype.Vy_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vy_Processed";
                    break;
                case 16:
                    RealtimeGraphType = graphtype.Vz;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vz";
                    break;
                case 17:
                    RealtimeGraphType = graphtype.Vz_p;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Vz_Processed";
                    break;
                case 18:
                    RealtimeGraphType = graphtype.V;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.V";
                    break;
                case 19:
                    RealtimeGraphType = graphtype.Ax;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Ax";
                    break;
                case 20:
                    RealtimeGraphType = graphtype.Ay;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Ay";
                    break;
                case 21:
                    RealtimeGraphType = graphtype.Az;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Az";
                    break;
                case 22:
                    RealtimeGraphType = graphtype.A;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.A";
                    break;
                case 23:
                    RealtimeGraphType = graphtype.Azimuth;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Azimuth";
                    break;
                case 24:
                    RealtimeGraphType = graphtype.Elevation;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Elevation";
                    break;
                case 25:
                    RealtimeGraphType = graphtype.PDOP;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.PDOP";
                    break;
                case 26:
                    RealtimeGraphType = graphtype.State;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.State";
                    break;
                case 27:
                    RealtimeGraphType = graphtype.Temperature;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.Temperature";
                    break;
                case 28:
                    RealtimeGraphType = graphtype.UsedStats;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.UsedStats";
                    break;
                case 29:
                    RealtimeGraphType = graphtype.VisibleStats;
                    c1Chart1.ChartArea.Axes[1].Text = "Buffer.VisibleSats";
                    break;
            }
            ClearGraph();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearGraph();
        }

        private void ClearGraph()
        {
            c1Chart1.ChartGroups[0].ChartData.SeriesList[0].X.Clear();
            c1Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.Clear();
            c1Chart1.ChartGroups[0].ChartData.SeriesList[1].X.Clear();
            c1Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.Clear();
        }

        private void ControlPanelButton_Click(object sender, EventArgs e)
        {
            ToggleControlPanel();
        }

        private void ApplySearch_Click(object sender, EventArgs e)
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
                Msg[index] = Functions.SEARCH_TYPE_CMD;
                index++;

                //Sat Type
                int SatType = 0;
                if (checkEditTypGPS.Checked)
                    SatType |= 1;
                if (checkEditTypGLONASS.Checked)
                    SatType |= 2;
                if (checkEditTypGalileo.Checked)
                    SatType |= 4;
                if (checkEditTypCompass.Checked)
                    SatType |= 8;
                Msg[index] = (char)SatType;
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                //second command:

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
                Msg[index] = Functions.SAT_NUMBER_CMD;
                index++;

                //Sat Type
                Msg[index] = (char)AllSatsMax.Value;// Settings.SatNum; //(char)Convert.ToInt16(radDDLTypAll.Text);
                index++;

                Msg[index] = (char)GPSMax.Value;//Convert.ToInt16(radDDLTypGPS.Text);
                index++;

                Msg[index] = (char)GlonassMax.Value;//Convert.ToInt16(radDDLTypGLONASS.Text);
                index++;

                Msg[index] = (char) GalileoMax.Value;//Convert.ToInt16(radDDLTypGalileo.Text);
                index++;

                Msg[index] = (char) CompassMax.Value;//Convert.ToInt16(radDDLTypCompass.Text);
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                //second command:

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
                Msg[index] = Functions.POSITIONING_TYPE_CMD;
                index++;

                //Pos Type
                Msg[index] = (char)(PositionTypeCombo.SelectedIndex + 1); // .FindStringExact(radDDLPosTyp.Text) + 1);
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                ApplySearch.Enabled = false;
                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    Application.DoEvents();
                }
                ApplySearch.Enabled = true;
        }

        private void ApplyThreshold_Click(object sender, EventArgs e)
        {
            Settings.GPSUseTh = Convert.ToInt16(GPSUseText.Text);
            Settings.GPSDisTh = Convert.ToInt16(GPSDisText.Text);
            Settings.GLONASSUseTh = Convert.ToInt16(GLONASSUserText.Text);
            Settings.GLONASSDisTh = Convert.ToInt16(GLONASSDisText.Text);
            Settings.PDOPTh = Convert.ToInt16(PDOPText.Text);
            Settings.RelyDisTh = Convert.ToInt16(RelyText.Text);
            Settings.SatDisErrTh = Convert.ToInt16(SatDistanceText.Text);
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
            Msg[index] = Functions.PDOP_THRESHOLD_CMD;
            index++;

            //PDOP Threshold
            Msg[index] = (char)(Settings.PDOPTh >> 8);
            index++;
            Msg[index] = (char)(Settings.PDOPTh & 0xff);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 2: 
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
            Msg[index] = Functions.GPS_USE_THRESHOLD_CMD;
            index++;

            //GPS Use Threshold
            Msg[index] = (char)(Settings.GPSUseTh * 4);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;


            //command 3:
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
            Msg[index] = Functions.GLONASS_USE_THRESHOLD_CMD;
            index++;

            //GLONASS Use Threshold
            Msg[index] = (char)(Settings.GLONASSUseTh * 4);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;


            //command 4:
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
            Msg[index] = Functions.GPS_DEASSIGN_THRESHOLD_CMD;
            index++;

            //GPS Deassign Threshold
            Msg[index] = (char)(Settings.GPSDisTh * 4);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 5:
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
            Msg[index] = Functions.GLONASS_DEASSIGN_THRESHOLD_CMD;
            index++;

            //GLONASS Deassign Threshold
            Msg[index] = (char)(Settings.GLONASSDisTh * 4);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 6:
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
            Msg[index] = Functions.RELIABILITY_DEASSIGN_THRESHOLD_CMD;
            index++;

            //Reliability Deassign Threshold
            Msg[index] = (char)(Settings.RelyDisTh);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 7:
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
            Msg[index] = Functions.SAT_DISTANCE_ERROR_THRESHOLD_CMD;
            index++;

            //Satellite Distance Error Threshold
            Msg[index] = (char)((Settings.SatDisErrTh * 10) >> 8);
            index++;
            Msg[index] = (char)((Settings.SatDisErrTh * 10) & 0xff);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;


            for (int i = 0; i < index; i++)
                byteMsg[i] = (byte)Msg[i];

            ApplyThreshold.Enabled = false;
            for (int i = 0; i < index; i++)
            {
                Parentform.Serial1_Write(byteMsg, i, 1);
                Application.DoEvents();
            }
            ApplyThreshold.Enabled = true;
        }

        private void ApplyCom_Click(object sender, EventArgs e)
        {
            Settings.BaudRate = Convert.ToInt32((string)BaudrateCombo.SelectedItem);
            Settings.RefreshRate = (int)RefreshRate.Value;
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
            Msg[index] = Functions.SERIAL_SETTING_CMD;
            index++;

            //Serial num
            Msg[index] = Convert.ToString(SerialCombo.SelectedIndex + 1)[0];
            index++;

            //Baud Rate
            Msg[index] = (char)((Settings.BaudRate >> 16) & 0xff);
            index++;

            Msg[index] = (char)((Settings.BaudRate >> 8) & 0xff);
            index++;

            Msg[index] = (char)(Settings.BaudRate & 0xff);
            index++;

            //Packet Types
            int PacketType = 0;
            if (checkEditPktNMEA.Checked)
                PacketType |= 4;
            if (checkEditPktBinary.Checked)
                PacketType |= 1;
            if (checkEditPktCompact.Checked)
                PacketType |= 2;
            if (checkEditPktGPSInfo.Checked)
                PacketType |= 32;
            if (checkEditPktGLONASSInfo.Checked)
                PacketType |= 64;
            if (checkEditPktGPSRaw.Checked)
                PacketType |= 8;
            if (checkEditPktGLONASSRaw.Checked)
                PacketType |= 16;

            Msg[index] = (char)(PacketType >> 8);
            index++;

            Msg[index] = (char)(PacketType & 0xff);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;
            //Command 2
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
            Msg[index] = Functions.REFRESH_RATE_CMD;
            index++;

            //Refresh Rate
            Msg[index] = (char)Settings.RefreshRate;
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            for (int i = 0; i < index; i++)
                byteMsg[i] = (byte)Msg[i];

            ApplyCom.Enabled = false;
            for (int i = 0; i < index; i++)
            {
                Parentform.Serial1_Write(byteMsg, i, 1);
                Application.DoEvents();
            }
            ApplyCom.Enabled = true;
        }

        private void ApplyMisc_Click(object sender, EventArgs e)
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
            Msg[index] = Functions.MAX_SPEED_CMD;
            index++;

            //MAX Speed
            Msg[index] = (char)(Convert.ToInt16(textEditMaxSpeed.Text) >> 8);
            index++;
            Msg[index] = (char)(Convert.ToInt16(textEditMaxSpeed.Text) & 0xff);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 2: 
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
            Msg[index] = Functions.MAX_ACCELERATION_CMD;
            index++;

            //MAX Acceleration
            Msg[index] = (char)((Convert.ToInt16(textEditMaxAcc.Text) * 10) >> 8);
            index++;
            Msg[index] = (char)((Convert.ToInt16(textEditMaxAcc.Text) * 10) & 0xff);
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 3: 
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
            Msg[index] = Functions.MASK_ANGLE_CMD;
            index++;

            //Mask Angle
            Msg[index] = (char)(Convert.ToInt32(textEditMaskAngle.Text));
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 4: 
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
            Msg[index] = Functions.GREEN_SAT_TYP_CMD;
            index++;

            //Green Satellite Type
            Msg[index] = (char)GreenSatCombo.SelectedIndex; //(Convert.ToInt16(radDDLGreenType.FindStringExact(radDDLGreenType.Text)));
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 5: 
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
            Msg[index] = Functions.TROPOSPHORIC_CORRECTION_CMD;
            index++;

            //Tropospheric Correction
            Msg[index] = (char)TropoCombo.SelectedIndex;//(Convert.ToInt16(radDDLTropo.FindStringExact(radDDLTropo.Text)));
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 6: 
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
            Msg[index] = Functions.AUTO_MAX_ANGLE_ATTITUDE_CMD;
            index++;

            //Automatic Max Angle Attitude
            Msg[index] = (char)AutoMaxCombo.SelectedIndex;//(Convert.ToInt16(radDDLAutoMaxAngle.FindStringExact(radDDLAutoMaxAngle.Text)));
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            //command 6: 
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
            Msg[index] = Functions.IONOSPHORIC_CORRECTION_CMD;
            index++;

            //Ionospheric Correction
            Msg[index] = (char)IonoCombo.SelectedIndex;//(Convert.ToInt16(radDDLIonospheric.FindStringExact(radDDLIonospheric.Text)));
            index++;

            //CRC
            Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
            index++;

            for (int i = 0; i < index; i++)
                byteMsg[i] = (byte)Msg[i];

            ApplyMisc.Enabled = false;
            for (int i = 0; i < index; i++)
            {
                Parentform.Serial1_Write(byteMsg, i, 1);
                Application.DoEvents();
            }
            ApplyMisc.Enabled = true;
        }

        private void Deassign_Satellites()
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
                Msg[index] = Functions.DEASSIGN_SATELLITES;
                index++;

                //PRNs
                UInt32 Sats = 0;
                for (int i = 0; i < listBoxGPS.SelectedIndices.Count; i++)
                    Sats += (UInt32)1 << listBoxGPS.SelectedIndices[i];
                Msg[index] = (char)((Sats >> 24) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 16) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 8) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 0) & 0xFF);
                index++;

                Sats = 0;
                for (int i = 0; i < listBoxGLONASS.SelectedIndices.Count; i++)
                    Sats += (UInt32)1 << listBoxGLONASS.SelectedIndices[i];
                Msg[index] = (char)((Sats >> 24) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 16) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 8) & 0xFF);
                index++;
                Msg[index] = (char)((Sats >> 0) & 0xFF);
                index++;

                //CRC
                Msg[index] = Functions.Calculate_Checksum_Char(Msg, index);
                index++;

                for (int i = 0; i < index; i++)
                    byteMsg[i] = (byte)Msg[i];

                //ApplyButtonsEnabling(false);
                for (int i = 0; i < index; i++)
                {
                    Parentform.Serial1_Write(byteMsg, i, 1);
                    //SelectedSerialPort.Write(byteMsg, i, 1);
                    Application.DoEvents();
                }
                //ApplyButtonsEnabling(true);
            }
            catch
            {
                //Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
            }
        }

        private void Deassign_Click(object sender, EventArgs e)
        {
            Deassign_Satellites();
        }

        private void DeassignAll_Click(object sender, EventArgs e)
        {
            listBoxGPS.SelectAll();
            listBoxGLONASS.SelectAll();
            Deassign_Satellites();
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            listBoxGLONASS.UnSelectAll();
            listBoxGPS.UnSelectAll();
        }

        private void ReadFlash_Click(object sender, EventArgs e)
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
                Msg[index] = Functions.READ_SETTING_CMD;
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
            catch
            {
            }
        }

        private void SaveFlash_Click(object sender, EventArgs e)
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
            catch
            {
            }
        }

        public void ChangeSettings(SettingBuffer buffer)
        {
            GPSMax.Value = buffer.GPSNum;
            GlonassMax.Value = buffer.GLONASSNum;
            GalileoMax.Value = buffer.GalileoNum;
            CompassMax.Value = buffer.CompassNum;
            AllSatsMax.Value = buffer.SatNum;

            PositionTypeCombo.SelectedIndex = buffer.PosType - 1;
            BaudrateCombo.SelectedIndex = BaudrateCombo.FindStringExact(buffer.BaudRate.ToString());
           // buffer.PacketType
            RefreshRate.Value = buffer.RefreshRate;
            PDOPText.Text = buffer.PDOPTh.ToString();
            GPSUseText.Text = buffer.GPSUseTh.ToString();
            GPSDisText.Text = buffer.GPSDisTh.ToString();
            GLONASSUserText.Text = buffer.GLONASSUseTh.ToString();
            GLONASSDisText.Text = buffer.GLONASSDisTh.ToString();
            RelyText.Text = buffer.RelyDisTh.ToString();
            SatDistanceText.Text = buffer.SatDisErrTh.ToString();
            textEditMaxSpeed.Text = buffer.MaxSpeed.ToString();
            textEditMaxAcc.Text = buffer.MaxAcc.ToString();
            textEditMaskAngle.Text = buffer.MaskAngle.ToString();
            GreenSatCombo.SelectedIndex = buffer.GreenSatType - 1;
            TropoCombo.SelectedIndex = buffer.TropoCor - 1;
            AutoMaxCombo.SelectedIndex = buffer.AutoMaxAngle - 1;
            IonoCombo.SelectedIndex = buffer.IonoCor - 1;

        }

        private void hotStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartModule('H');
        }

        private void warmStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartModule('W');
        }

        private void coldStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartModule('C');
        }

        private void StartModule(char mode)
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
                Msg[index] = Functions.START_CMD;
                index++;

                //Data
                Msg[index] = mode;
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
            catch
            {

            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            StartModule('H');
        }

        private void ResetZoom_Click(object sender, EventArgs e)
        {
            c1Chart1.ChartArea.AxisX.AutoMax = true;
            c1Chart1.ChartArea.AxisX.AutoMin = true;
            c1Chart1.ChartArea.AxisY.AutoMax = true;
            c1Chart1.ChartArea.AxisY.AutoMin = true;
            GraphTooltip.SetToolTip(c1Chart1, "Shift+Click = Scroll, CTRL+Click = Scale, ALT+Click = Zoom, RightClick = Options");
        }

        private void c1Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var t = System.Windows.Forms.Cursor.Position;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                clickeditem = optionfor.graph;
                GraphOptions.Show(t);
            }
        }

        private void GraphOptions_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            GraphOptions.Close(ToolStripDropDownCloseReason.ItemClicked);
            switch (e.ClickedItem.Text)
            {
                case "Save To Image":
                    ImageSaveDialog.Filter = "Jpeg File (*.jpeg)|*.jpeg|Bitmap File (*.bmp)|*.bmp|PNG File (*.png)|*.PNG";
                    ImageSaveDialog.ShowDialog();
                    break;
                case "Export Chart Data":
                    DataExporter.Filter = "GPS Navigator Graph Data (*.ggd)|*.ggd";
                    DataExporter.ShowDialog();
                    break;
                case "Import Chart Data":
                    DataImporter.Filter = "GPS Navigator Graph Data (*.ggd)|*.ggd";
                    DataImporter.ShowDialog();
                    break;

            }
        }

        private void ImageSaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            switch (ImageSaveDialog.FilterIndex)
            {
                case 1:
                    switch (clickeditem)
                    {
                        case optionfor.chart1:
                            chart1.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Jpeg);
                            break;
                        case optionfor.chart2:
                            chart2.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Jpeg);
                            break;
                        case optionfor.graph:
                            c1Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                    }

                    break;
                case 2:
                    switch (clickeditem)
                    {
                        case optionfor.chart1:
                            chart1.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Bmp);
                            break;
                        case optionfor.chart2:
                            chart2.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Bmp);
                            break;
                        case optionfor.graph:
                            c1Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                    }

                    break;
                case 3:
                    switch (clickeditem)
                    {
                        case optionfor.chart1:
                            chart1.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Png);
                            break;
                        case optionfor.chart2:
                            chart2.SaveImage(ImageSaveDialog.FileName, ChartImageFormat.Png);
                            break;
                        case optionfor.graph:
                            c1Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                    break;
            }
            //c1Chart1.SaveImage(ImageSaveDialog.FileName, ImageSaveDialog.ex
        }

        private void DataExporter_FileOk(object sender, CancelEventArgs e)
        {
            switch (clickeditem)
            {
                case optionfor.chart1:
                    chart1.Serializer.Save(DataExporter.FileName);
                    break;
                case optionfor.chart2:
                    chart2.Serializer.Save(DataExporter.FileName);
                    break;
                case optionfor.graph:
                    c1Chart1.SaveChartToFile(DataExporter.FileName);
                    break;
            }
        }

        private void DataImporter_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                switch (clickeditem)
                {
                    case optionfor.chart1:
                        chart1.Serializer.Load(DataImporter.FileName);
                        break;
                    case optionfor.chart2:
                        chart2.Serializer.Load(DataImporter.FileName);
                        break;
                    case optionfor.graph:
                        c1Chart1.LoadChartFromFile(DataImporter.FileName);
                        break;
                }

            }
            catch
            {
                throw new Exception("Couldnt load file");
            }
        }

        private void SaveToImage_Click(object sender, EventArgs e)
        {

        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var t = System.Windows.Forms.Cursor.Position;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                clickeditem = optionfor.chart1;
                GraphOptions.Show(t);
            }
        }

        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            var t = System.Windows.Forms.Cursor.Position;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                clickeditem = optionfor.chart2;
                GraphOptions.Show(t);
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            double TH = Convert.ToDouble(textEditDistanceTH.Text);
            ((NumericAxis)((LinearGauge)ultraGaugeDistance.Gauges[0]).Scales[0].Axis).StartValue = Convert.ToDouble(textEditDistance.Text) - TH;
            ((NumericAxis)((LinearGauge)ultraGaugeDistance.Gauges[0]).Scales[0].Axis).EndValue = Convert.ToDouble(textEditDistance.Text) + TH;
            ((NumericAxis)((LinearGauge)ultraGaugeDistance.Gauges[0]).Scales[0].Axis).TickmarkInterval = TH * 2 / 100.0;

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
                Msg[index] = Functions.ATTITUDE_INFO_CMD;
                index++;

                //Distance
                int d = (int)(Double.Parse(textEditDistance.Text) * 1000);
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;

                //Tol
                d = (int)(Double.Parse(textEditDistanceTH.Text) * 1000);
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
                index++;
                Msg[index] = (char)(d & 0xFF);
                d /= 256;
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
                Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
            }
        }

        private void saveToFlashButton_Click(object sender, EventArgs e)
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
                Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
            }
        }

        private void Serial1check_CheckedChanged(object sender, EventArgs e)
        {
            c1Chart1.ChartGroups[0].ChartData.SeriesList[0].Display = Serial1check.Checked ? C1.Win.C1Chart.SeriesDisplayEnum.Show : C1.Win.C1Chart.SeriesDisplayEnum.Hide;
        }

        private void Serial2Check_CheckedChanged(object sender, EventArgs e)
        {
            c1Chart1.ChartGroups[0].ChartData.SeriesList[1].Display = Serial2Check.Checked ? C1.Win.C1Chart.SeriesDisplayEnum.Show : C1.Win.C1Chart.SeriesDisplayEnum.Hide;
        }

    }
}
