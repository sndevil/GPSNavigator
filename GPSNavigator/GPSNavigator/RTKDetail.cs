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
        RanSeries r;
        Random rnd;
        double[] xData = new double[buffersize];
        double[] yData = new double[buffersize];
        double[] zData = new double[buffersize];

        int pointcount = 0;
        double altavg = 0, longavg = 0, latavg = 0;
        double firstalt = -1, firstlong = -1, firstlat = -1;
        bool ReferenceSet = false;
        double rotation = 45;

        double refalt=0, reflong=0, reflat= 0;

        public RTKDetail(Form1 Parent)
        {
            InitializeComponent();

            chart1.ViewPortChanged += new WinViewPortEventHandler(chart1_ViewPortChanged);

            for (int i = 0; i < buffersize; i++)
                xData[i] = yData[i] = zData[i] = double.NaN;
            Createchart(chart1);
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
            c.setPlotRegion(320, 195, 300,300,150);//360, 360, 270);

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
            chart1.updateViewPort(true, false);

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
            latitudevaltext.Text = data.Latitude.ToString();
            longitudevalText.Text = data.Longitude.ToString();
            altitudevalText.Text = data.Altitude.ToString();
            
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
            }
            catch
            {
                MessageBox.Show("Input Error");
            }
        }



        public void SendCommand(char[] cmd)
        {

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

    }
}
