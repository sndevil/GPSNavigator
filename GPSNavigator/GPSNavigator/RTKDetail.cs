using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChartDirector;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class RTKDetail : Form
    {

        const int buffersize = 500;
        double[] xData = new double[buffersize];
        double[] yData = new double[buffersize];
        double[] zData = new double[buffersize];

        int pointcount = 0;
        double altavg = 0, longavg = 0, latavg = 0;
        double firstalt = -1, firstlong = -1, firstlat = -1;
        double rotation = 45;

        bool mousedown=false;
        int refx=0;

        double refalt=0, reflong=0, reflat= 0;
        Form1 parent;

        bool datareceived = false;
        public bool paused = false;

        long starttime;
        Random r;

        public RTKDetail(Form1 Parent)
        {
            InitializeComponent();
            r = new Random(DateTime.Now.Millisecond);
            chart1.ViewPortChanged += new WinViewPortEventHandler(chart1_ViewPortChanged);
            chart1.MouseDown += new MouseEventHandler(chart1_MouseDown);
            chart1.MouseUp += new MouseEventHandler(chart1_MouseUp);
            chart1.MouseMove += new MouseEventHandler(chart1_MouseMove);

            for (int i = 0; i < buffersize; i++)
                xData[i] = yData[i] = zData[i] = double.NaN;
            Createchart(chart1);
            parent = Parent;
            starttime = 0;
        }

        void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedown)
            {
                rotation = e.X - refx;
                chart1.updateViewPort(true, false);
            }

        }

        void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }

        void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
            refx = e.X - (int)rotation;           
        }

        void chart1_ViewPortChanged(object sender, WinViewPortEventArgs e)
        {
            Createchart(chart1);
        }

        public void Createchart(WinChartViewer viewer)
        {
            // Create a ThreeDScatterChart object of size 720 x 600 pixels
            ThreeDScatterChart c = new ThreeDScatterChart(640, 390);
            // Add a title to the chart using 20 points Times New Roman Italic font
            c.addTitle("Position", "Microsoft Sans Serif", 16);
            c.setAntiAlias(true);
            // Set the center of the plot region at (350, 280), and set width x depth x height to
            // 360 x 360 x 270 pixels
            c.setPlotRegion(320, 195, 300,300,150);

            // Add a scatter group to the chart using 11 pixels glass sphere symbols, in which the
            // color depends on the z value of the symbol
            c.addScatterGroup(xData, yData, zData, "", Chart.GlassSphere2Shape, 11,
                Chart.SameAsMainColor);                        
            // Add a color axis (the legend) in which the left center is anchored at (645, 270). Set
            // the length to 200 pixels and the labels on the right side.
            c.setColorAxis(570, 195, Chart.Left, 100, Chart.Right);
            c.setViewAngle(20, rotation);
            // Set the x, y and z axis titles using 10 points Arial Bold font
            c.xAxis().setTitle("Latitude", "Microsoft Sans Serif", 10);
            c.yAxis().setTitle("Longitude", "Microsoft Sans Serif", 10);
            c.zAxis().setTitle("Altitude", "Microsoft Sans Serif", 10);           
            c.setBackground(14474460);
       
            // Output the chart            
            viewer.Chart = c;            

            //include tool tip for the chart
            viewer.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='(lat={x}, long={y}, alt={z}'");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //chart2.Series[0].Points.AddXY(r.Next(0,100), r.Next(0,100));
            //chart1.updateViewPort(true, false);

            if (!paused)
            {
                if (datareceived)
                {
                    DataStatusLabel.Text = "Receiving Data";
                    DataStatusLabel.BackColor = Color.Lime;
                    datareceived = false;
                }
                else
                {
                    DataStatusLabel.Text = "No Data";
                    DataStatusLabel.BackColor = Color.Salmon;
                }
            }

            starttime+=1;
            AverateTimerLabel.Text = string.Format("{0,2:00}:{1,2:00}:{2,2:00}", (int)starttime / 3600, (int)((starttime / 60) % 60), (int)(starttime % 60));

        }

        private void manualRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (manualRadio.Checked)
            {
                SetBtn.Enabled = true;
                latitudevaltext.Enabled = true;
                longitudevalText.Enabled = true;
                altitudevalText.Enabled = true;
            }
            else
            {
                SetBtn.Enabled = false;
                latitudevaltext.Enabled = false;
                longitudevalText.Enabled = false;
                altitudevalText.Enabled = false;
            }
        }

        public void UpdateData(Globals vars,SingleDataBuffer data,int Serialnumber)
        {
            if (!(data.Latitude == data.Longitude && data.Latitude == data.Altitude && data.Latitude == 0))
            {
                if (pointcount != buffersize)
                {
                    pointcount++;
                    latavg = (latavg * (pointcount - 1) + data.Latitude) / pointcount;
                    longavg = (longavg * (pointcount - 1) + data.Longitude) / pointcount;
                    altavg = (altavg * (pointcount - 1) + data.Altitude) / pointcount;
                }
                else
                {
                    latavg = ((latavg * (buffersize - 1)) + data.Latitude) / buffersize;
                    longavg = ((longavg * (buffersize - 1)) + data.Longitude) / buffersize;
                    altavg = ((altavg * (buffersize - 1)) + data.Altitude) / buffersize;
                }

                latitudeavgText.Text = latavg.ToString();
                longitudeavgText.Text = longavg.ToString();
                altitudeavgText.Text = altavg.ToString();

                if (firstalt == -1)
                {
                    firstlat = data.Latitude;
                    firstlong = data.Longitude;
                    firstalt = data.Altitude;
                }

                //AddPosition(data.Latitude - firstlat, data.Longitude - firstlong, data.Altitude - firstalt);
                AddPosition(latavg - firstlat, longavg - firstlong, altavg - firstalt);
            }
            datareceived = true;
            chart1.updateViewPort(true, false);

        }

        public void UpdateGraph(Globals vars, SingleDataBuffer data, int Serialnumber)
        {
            if (vars.SatsUpdated)
            {
                int counter = 0;
                int status;
                double SNR;
                if (vars.GPSlist.Count < 1)
                {
                    var tempsat = new Satellite[32];
                    for (int j = 0; j < 32; j++)
                        tempsat[j] = new Satellite();
                    vars.GPSlist.Add(tempsat);
                }
                if (vars.GLONASSlist.Count < 1)
                {
                    var tempsat1 = new Satellite[32];
                    for (int j = 0; j < 32; j++)
                        tempsat1[j] = new Satellite();
                    vars.GLONASSlist.Add(tempsat1);
                }

                for (int i = 0; i < 64; i++)
                {
                    if (i < 32 && vars.GPSlist[0][i] == null)
                        continue;
                    else if (i >= 32 && vars.GLONASSlist[0][i - 32] == null)
                        continue;
                    status = (i < 32) ? vars.GPSlist[0][i].Signal_Status : vars.GLONASSlist[0][i - 32].Signal_Status;
                    if (status != 2)
                        continue;
                    SNR = (i < 32) ? vars.GPSlist[0][i].SNR : vars.GLONASSlist[0][i - 32].SNR;

                    if (SNR > 0)
                    {
                        try
                        {
                            chart2.Series[0].Points[counter].SetValueXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), SNR);
                            if (i >= 32)
                            {
                                chart2.Series[0].Points[counter].CustomProperties = "DrawingStyle = Wedge";
                                chart2.Series[0].Points[counter].Color = Color.LawnGreen;
                            }
                            else
                            {
                                chart2.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                chart2.Series[0].Points[counter].Color = Color.Green;
                            }
                        }
                        catch
                        {
                            chart2.Series[0].Points.AddXY(((i < 32) ? "P" + (i + 1).ToString() : "L" + (i - 31).ToString()), SNR);
                            chart2.Series[0].Points[chart2.Series[0].Points.Count - 1].Color = Color.Green;
                            if (i >= 32)
                            {
                                chart2.Series[0].Points[chart2.Series[0].Points.Count - 1].CustomProperties = "DrawingStyle = Wedge";
                                chart2.Series[0].Points[counter].Color = Color.LawnGreen;
                            }
                            else
                            {
                                chart2.Series[0].Points[counter].CustomProperties = "DrawingStyle = Default";
                                chart2.Series[0].Points[counter].Color = Color.Green;
                            }
                        }
                        counter++;
                    }

                }
                if (counter < chart2.Series[0].Points.Count)
                {
                    for (int k = chart2.Series[0].Points.Count - 1; k >= counter; k--)
                        chart2.Series[0].Points.RemoveAt(k);
                }

                if (counter > 14)
                {
                    chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                }
                else if (chart2.ChartAreas[0].AxisX.LabelStyle.Angle != 0)
                {
                    chart2.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
                }
                vars.SatsUpdated = false;
            }
        }

        private void AddPosition(double lat, double longi, double alt)
        {
            for (int i = buffersize-1; i > 0; i--)
            {
                xData[i] = xData[i - 1];
                yData[i] = yData[i - 1];
                zData[i] = zData[i - 1];
            }

            xData[0] = lat;
            yData[0] = longi;
            zData[0] = alt;
        }

        private void ResetAvgBtn_Click(object sender, EventArgs e)
        {
            pointcount = 0;
            latavg = 0;
            longavg = 0;
            altavg = 0;
            starttime = 0;
            latitudeavgText.Text = "";
            longitudeavgText.Text = "";
            altitudeavgText.Text = "";
        }

        private void SetReferenceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                reflat = double.Parse(latitudeavgText.Text);
                reflong = double.Parse(longitudeavgText.Text);
                refalt = double.Parse(altitudeavgText.Text);

                latitudevaltext.Text = reflat.ToString();
                longitudevalText.Text = reflong.ToString();
                altitudevalText.Text = refalt.ToString();
            }
            catch
            {
                MessageBox.Show("Input Error");
            }
        }

        private void SetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                reflat = double.Parse(latitudevaltext.Text);
                reflong = double.Parse(longitudevalText.Text);
                refalt = double.Parse(altitudevalText.Text);
                char[] cmd = ("$JRTK,1," + reflat.ToString() + "," + reflong.ToString() + "," + refalt.ToString()+"\r\n").ToCharArray();
                SendCommand(cmd);
            }
            catch
            {
                MessageBox.Show("Input Error");
            }
        }

        public void SendCommand(char[] cmd)
        {
            byte[] bmsg = new byte[cmd.Length];
            for (int i = 0; i < cmd.Length; i++)
                bmsg[i] = (byte)cmd[i];

            parent.Serial1_Write(bmsg, 0, cmd.Length);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            firstalt = firstlong = firstlat = -1;
            for (int i = 0; i < buffersize; i++)
                xData[i] = yData[i] = zData[i] = double.NaN;
            chart1.updateViewPort(true, false);
        }

        private void RotLeftBtn_Click(object sender, EventArgs e)
        {
            rotation -= 10;
            chart1.updateViewPort(true, false);
        }

        private void RotRightBtn_Click(object sender, EventArgs e)
        {
            rotation += 10;
            chart1.updateViewPort(true, false);
        }

        private void GetBtn_Click(object sender, EventArgs e)
        {
            char[] cmd = ("$JASC,GPGSV,1\r\n").ToCharArray();
            SendCommand(cmd);
            // Get Command
        }

        private void ReAverageBtn_Click(object sender, EventArgs e)
        {
            // Re-average Command
        }

    }
}
