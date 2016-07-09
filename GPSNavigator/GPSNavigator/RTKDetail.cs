using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChartDirector;

namespace GPSNavigator
{
    public partial class RTKDetail : Form
    {


        public RTKDetail(Form1 Parent)
        {
            InitializeComponent();

        }


        public void Createchart(double[] xData,double[] yData, double[] zData)
        {

            // Create a ThreeDScatterChart object of size 720 x 600 pixels
            ThreeDScatterChart c = new ThreeDScatterChart(640, 390);
            // Add a title to the chart using 20 points Times New Roman Italic font
            c.addTitle("Position Chart ", "Microsoft Sans Serif", 20);

            // Set the center of the plot region at (350, 280), and set width x depth x height to
            // 360 x 360 x 270 pixels
            c.setPlotRegion(320, 195, 190,190,200);//360, 360, 270);

            // Add a scatter group to the chart using 11 pixels glass sphere symbols, in which the
            // color depends on the z value of the symbol
            c.addScatterGroup(xData, yData, zData, "", Chart.GlassSphere2Shape, 11,
                Chart.SameAsMainColor);

            // Add a color axis (the legend) in which the left center is anchored at (645, 270). Set
            // the length to 200 pixels and the labels on the right side.
            c.setColorAxis(570, 195, Chart.Left, 100, Chart.Right);

            // Set the x, y and z axis titles using 10 points Arial Bold font
            c.xAxis().setTitle("X-Axis", "Microsoft Sans Serif", 10);
            c.yAxis().setTitle("Y-Axis", "Microsoft Sans Serif", 10);
            c.zAxis().setTitle("Z-Axis", "Microsoft Sans Serif", 10);

            c.setBackground(14474460);

            chart1.clearAllRanges();
            // Output the chart            
            chart1.Chart = c;            

            //include tool tip for the chart
            chart1.ImageMap = c.getHTMLImageMap("clickable", "",
                "title='(x={x|p}, y={y|p}, z={z|p}'");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RanSeries r = new RanSeries(0);
            Random a = new Random(System.DateTime.Now.Millisecond);            
            double[] xData = r.getSeries2(100, a.Next(-100,100), -10, 10);
            double[] yData = r.getSeries2(100, a.Next(-100,100), -10, 10);
            double[] zData = r.getSeries2(100, a.Next(-100,100), -10, 10);
            Createchart(xData, yData, zData);
            chart1.updateViewPort(true, false);

        }

    }
}
