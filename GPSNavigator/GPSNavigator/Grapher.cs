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
        public enum graphtype { X, Y, Z, Vx, Vy, Vz, Ax, Ay, Az, Latitude, Longitude, Altitude };
        private graphtype selectedtype = graphtype.X;

        private double[] ylist = new double[1000];
        private double[] xlist = new double[1000];

        public Grapher(DataBuffer buffer, long Length,LogFileManager Filemanager)
        {
            InitializeComponent();
            filemanager = Filemanager;
            dbuffer = buffer;
            length = Length;
            bufferlength = length / 100;
            fmax = length;
            offset = 0;
        }

        private void Grapher_Load(object sender, EventArgs e)
        {
            LoadData();
            max = dbuffer.Ax.Count;
            delta = (fmax - fmin) / 1000f;
            zoom = (1000f / length) >= 0.1f ? (1000f / length) : 0.1f;
            hScrollBar2.Maximum = (int)(length - delta);
            hScrollBar2.SmallChange = (int)delta;
            hScrollBar2.LargeChange = (int)delta * 10;
            hScrollBar1.Minimum = (int)zoom * 100;
            hScrollBar1.Value = hScrollBar1.Minimum;
            comboBox1.SelectedIndex = 0;
        }

        public void PlotGraph()
        {
            int counter=0;
            float i = min;
            double temp = 0;
            for (counter = 0; counter < 1000; counter++)
            {
                if (counter >= dbuffer.X.Count)
                {
                    for (int j = counter; j < 1000; j++)
                    {
                        ylist[j] = temp;
                        xlist[j] = (double)i;
                        i += delta;
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
                    }
                }
                ylist[counter] = temp;
                xlist[counter] = (double)i;
                i += delta;
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
            var v = hScrollBar1.Value;
            Stopwatch s = Stopwatch.StartNew();
            while (s.ElapsedMilliseconds < 400)
            {
            }
            if (hScrollBar1.Value == v)
            {
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
                    break;
                case 1:
                    selectedtype = graphtype.Y;
                    break;
                case 2:
                    selectedtype = graphtype.Z;
                    break;
                case 3:
                    selectedtype = graphtype.Latitude;
                    break;
                case 4:
                    selectedtype = graphtype.Altitude;
                    break;
                case 5:
                    selectedtype = graphtype.Longitude;
                    break;
                case 6:
                    selectedtype = graphtype.Vx;
                    break;
                case 7:
                    selectedtype = graphtype.Vy;
                    break;
                case 8:
                    selectedtype = graphtype.Vz;
                    break;
                case 9:
                    selectedtype = graphtype.Ax;
                    break;
                case 10:
                    selectedtype = graphtype.Ay;
                    break;
                case 11:
                    selectedtype = graphtype.Az;
                    break;
            }
            PlotGraph();
        }

        public void LoadData()
        {
            xlist = new double[1000];
            ylist = new double[1000];
            filemanager.start = fmin;
            filemanager.delta = (fmax-fmin) / 1000f;
            delta = (fmax - fmin) / 1000f;
            dbuffer = filemanager.Readbuffer();
            PlotGraph();
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            offset = hScrollBar2.Value;
            fmin = min + offset;
            fmax = fmin + bufferlength * (101 - hScrollBar1.Value);
            LoadData();
        }
    }
}
