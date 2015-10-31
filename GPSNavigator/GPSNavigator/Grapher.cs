using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using GPSNavigator.Classes;
using GPSNavigator.Source;
using C1.Win.C1Chart;

namespace GPSNavigator
{
    public partial class Grapher : Form
    {
        public int min = 0, max = 1000;
        public long fmin = 0, fmax, offset, bufferlength;
        public float zoom = 0.01f;
        public long length;
        public float delta;
        public DataBuffer dbuffer;
        public LogFileManager filemanager;
        public PointStyle ps;
        public enum graphtype { X, Y, Z, Vx, Vy, Vz, Ax, Ay, Az, Latitude, Longitude, Altitude,PDOP,State,Temperature,UsedStats,VisibleStats};
        private graphtype selectedtype = graphtype.X;

        private const int Databuffercount = 500;
        private int scroll1=-1;
        private double[] ylist = new double[Databuffercount];
        private double[] xlist = new double[Databuffercount];

        public Grapher(DataBuffer buffer, long Length,LogFileManager Filemanager)
        {
            InitializeComponent();
            Chart1.MouseMove += new MouseEventHandler(Chart1_MouseMove);
            Chart1.MouseClick += new MouseEventHandler(Chart1_MouseClick);
            filemanager = Filemanager;
            dbuffer = buffer;
            length = Length;
            bufferlength = length / 100;
            fmax = length;
            offset = 0;
        }

        void Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            
            var xpos = xlist[ps.PointIndex] * 7000;
            if (xpos < 0)
                xpos = 0;
            offset = (long)(xpos - delta/2);
            fmin = min + offset;
            hScrollBar2.Value = (int)fmin;
            hScrollBar1.Value = 100;
            fmax = fmin + bufferlength * (101 - hScrollBar1.Value);
            LoadData();
            label1.Text = "Zoom: " + (100 - hScrollBar1.Value).ToString() + "%";
            int percent = (int)(((float)hScrollBar2.Value / (float)hScrollBar2.Maximum) * 100);
            label2.Text = "Position: " + percent.ToString() + "%";
        }

        void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            ChartGroup grp = Chart1.ChartGroups.Group0;
            ps = grp.ChartData.PointStylesList[0];
            // if both are changed to valid values, then something needs to be highlighted
            int seriesIndex = -1, pointIndex = -1;


            // check if the mouse is in the PlotArea.
            if (Chart1.ChartRegionFromCoord(e.X, e.Y) == ChartRegionEnum.PlotArea)
            {

                int dist = 0;

                // find the closest XCoord.
                if (grp.CoordToDataIndex(e.X, e.Y, CoordinateFocusEnum.XCoord, ref seriesIndex, ref pointIndex, ref dist))
                {
                    // if there are multiple series in the chart, then find the closest series.
                    if (grp.CoordToSeriesIndex(e.X, e.Y, PlotElementEnum.Series, ref seriesIndex, ref dist))
                    {
                        ps.PointIndex = pointIndex;
                        ps.SeriesIndex = seriesIndex;
                        label3.Text = xlist[pointIndex].ToString();
                        label4.Text = ylist[pointIndex].ToString();
                    }
                }

                if (seriesIndex == -1 || pointIndex == -1)
                {
                    ps.PointIndex = -1;
                    ps.SeriesIndex = -1;
                }
            }
        }

        private void Grapher_Load(object sender, EventArgs e)
        {
            LoadData();

            initgrap();

            max = dbuffer.Ax.Count;
            delta = (fmax - fmin) / (float)(Databuffercount) ;
            zoom = (1000f / length) >= 0.1f ? (1000f / length) : 0.1f;
            hScrollBar2.Maximum = (int)(length - delta);
            hScrollBar2.SmallChange = (int)delta;
            hScrollBar2.LargeChange = (int)delta * 10;
            hScrollBar1.Minimum = (int)zoom * 100;
            hScrollBar1.Value = hScrollBar1.Minimum;
            comboBox1.SelectedIndex = 0;
        }

        void initgrap()
        {
            ChartData cd = Chart1.ChartGroups.Group0.ChartData;
            ChartDataSeriesCollection cdsc = cd.SeriesList;
            foreach (ChartDataSeries cds in cdsc)
            {
                cds.SymbolStyle.Shape = SymbolShapeEnum.Dot;
                cds.SymbolStyle.Size = 0;
            }

            PointStyle ps = Chart1.ChartGroups.Group0.ChartData.PointStylesList.AddNewPointStyle();
            ps.SymbolStyle.Shape = SymbolShapeEnum.Dot;
            ps.SymbolStyle.Size = 10;
            ps.SymbolStyle.Color = Color.Red;
            ps.PointIndex = -1;
            ps.SeriesIndex = -1;
            ps.Label = "trackingBall";
        }

        public void PlotGraph()
        {
            int counter=0;
            float i = fmin/7000f;
            double temp = 0;
            var dt = delta / 7000f;
            for (counter = 0; counter < Databuffercount; counter++)
            {
                if (counter >= dbuffer.X.Count)
                {
                    for (int j = counter; j < Databuffercount; j++)
                    {
                        ylist[j] = temp;
                        xlist[j] = (double)i;
                        i += dt;
                    }
                    break;
                }
                var index = dbuffer.statcounter[counter];
                if (index != -1)
                {
                    switch (selectedtype)
                    {
                        case graphtype.X:
                            temp = dbuffer.X[index];
                            break;
                        case graphtype.Y:
                            temp = dbuffer.Y[index];
                            break;
                        case graphtype.Z:
                            temp = dbuffer.Z[index];
                            break;
                        case graphtype.Vx:
                            temp = dbuffer.Vx[index];
                            break;
                        case graphtype.Vy:
                            temp = dbuffer.Vy[index];
                            break;
                        case graphtype.Vz:
                            temp = dbuffer.Vz[index];
                            break;
                        case graphtype.Ax:
                            temp = dbuffer.Ax[index];
                            break;
                        case graphtype.Ay:
                            temp = dbuffer.Ay[index];
                            break;
                        case graphtype.Az:
                            temp = dbuffer.Az[index];
                            break;
                        case graphtype.Latitude:
                            temp = dbuffer.Latitude[index];
                            break;
                        case graphtype.Longitude:
                            temp = dbuffer.Longitude[index];
                            break;
                        case graphtype.Altitude:
                            temp = dbuffer.Altitude[index];
                            break;
                        case graphtype.PDOP:
                            temp = dbuffer.PDOP[index];
                            break;
                        case graphtype.State:
                            temp = dbuffer.state[index];
                            break;
                        case graphtype.Temperature:
                            temp = dbuffer.Temperature[index];
                            break;
                        case graphtype.UsedStats:
                            temp = dbuffer.NumOfUsedSats[index];
                            break;
                        case graphtype.VisibleStats:
                            temp = dbuffer.NumOfVisibleSats[index];
                            break;
                    }
                }
                
                ylist[counter] = temp;
                xlist[counter] = (double)i;
                i += dt;
            }
           // if (counter < 1000)
            //{
            //    ylist[counter] = 0;
             //   xlist[counter] = max;
           // }
            Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(xlist);
            Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.CopyDataIn(ylist);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (hScrollBar1.Value != scroll1)
            {
                scroll1 = hScrollBar1.Value;
                fmin = min + offset;
                fmax = fmin + bufferlength * (101 - hScrollBar1.Value);
                delta = ((max - min) / 100000f) * (101 - hScrollBar1.Value);
                label1.Text = "Zoom: " + (100 - hScrollBar1.Value).ToString() + "%";
                LoadData();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    selectedtype = graphtype.X;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.X";
                    break;
                case 1:
                    selectedtype = graphtype.Y;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Y";
                    break;
                case 2:
                    selectedtype = graphtype.Z;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Z";
                    break;
                case 3:
                    selectedtype = graphtype.Latitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Latitude";
                    break;
                case 5:
                    selectedtype = graphtype.Altitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude";
                    break;
                case 4:
                    selectedtype = graphtype.Longitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Longtitude";
                    break;
                case 6:
                    selectedtype = graphtype.Vx;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vx";
                    break;
                case 7:
                    selectedtype = graphtype.Vy;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vy";
                    break;
                case 8:
                    selectedtype = graphtype.Vz;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vz";
                    break;
                case 9:
                    selectedtype = graphtype.Ax;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ax";
                    break;
                case 10:
                    selectedtype = graphtype.Ay;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ay";
                    break;
                case 11:
                    selectedtype = graphtype.Az;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Az";
                    break;
                case 12:
                    selectedtype = graphtype.PDOP;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.PDOP";
                    break;
                case 13:
                    selectedtype = graphtype.State;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.State";
                    break;
                case 14:
                    selectedtype = graphtype.Temperature;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Temperature";
                    break;
                case 15:
                    selectedtype = graphtype.UsedStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.UsedStats";
                    break;
                case 16:
                    selectedtype = graphtype.VisibleStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.VisibleSats";
                    break;
            }
            PlotGraph();
        }

        public void LoadData()
        {
            xlist = new double[Databuffercount];
            ylist = new double[Databuffercount];
            filemanager.start = fmin;
            filemanager.delta = (fmax-fmin) / (float)Databuffercount;
            delta = (fmax - fmin) / (float)Databuffercount;
            //Stopwatch s = Stopwatch.StartNew();
            dbuffer = filemanager.Readbuffer();
            //MessageBox.Show(s.ElapsedMilliseconds.ToString());
            PlotGraph();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            if (hScrollBar2.Value != offset)
            {
                offset = hScrollBar2.Value;
                fmin = min + offset;
                fmax = fmin + bufferlength * (101 - hScrollBar1.Value);
                int percent = (int)(((float)hScrollBar2.Value / (float)hScrollBar2.Maximum) * 100);
                label2.Text = "Position: " + percent.ToString() + "%";
                LoadData();
            }
        }
    }
}
