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
using C1.Win.C1Chart;

namespace GPSNavigator
{
    public partial class Grapher : Form
    {
        public int min = 0, max = 1000;
        public float zoom = 0.01f;
        public long length;
        public float delta;
        public DataBuffer dbuffer;
        public enum graphtype { X, Y, Z, Vx, Vy, Vz, Ax, Ay, Az, Latitude, Longitude, Altitude };
        private graphtype selectedtype = graphtype.X;

        private double[] ylist = new double[1000];
        private double[] xlist = new double[1000];

        public Grapher(DataBuffer buffer)
        {
            InitializeComponent();
            dbuffer = buffer;
            length = dbuffer.Altitude.Count;
            max = (int)length;
            delta = (max - min) / 1000f;
            zoom = (1000f / length) >= 0.1f ? (1000f/length) : 0.1f;
            hScrollBar1.Minimum = (int)zoom * 100;
            hScrollBar1.Value = hScrollBar1.Minimum;
            comboBox1.SelectedIndex = 0;
        }

        private void Grapher_Load(object sender, EventArgs e)
        {
            PlotGraph();
        }

        public void PlotGraph()
        {
            int counter=0;
            for (float i = min; i < max; i+=delta)
            {
                if (counter >= 1000)
                    break;
                double temp=0;
                switch (selectedtype)
                {
                    case graphtype.X:
                        temp = dbuffer.X[(int)i];
                        break;
                    case graphtype.Y:
                        temp = dbuffer.Y[(int)i];
                        break;
                    case graphtype.Z:
                        temp = dbuffer.Z[(int)i];
                        break;
                    case graphtype.Vx:
                        temp = dbuffer.Vx[(int)i];
                        break;
                    case graphtype.Vy:
                        temp = dbuffer.Vy[(int)i];
                        break;
                    case graphtype.Vz:
                        temp = dbuffer.Vz[(int)i];
                        break;
                    case graphtype.Ax:
                        temp = dbuffer.Ax[(int)i];
                        break;
                    case graphtype.Ay:
                        temp = dbuffer.Ay[(int)i];
                        break;
                    case graphtype.Az:
                        temp = dbuffer.Az[(int)i];
                        break;
                    case graphtype.Latitude:
                        temp = dbuffer.Latitude[(int)i];
                        break;
                    case graphtype.Longitude:
                        temp = dbuffer.Longitude[(int)i];
                        break;
                    case graphtype.Altitude:
                        temp = dbuffer.Altitude[(int)i];
                        break;
                }
                    ylist[counter] = temp;
                    xlist[counter] = (double)i;
                    counter++;
            }

            Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(xlist);
            Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.CopyDataIn(ylist);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            delta = ((max - min) / 100000f) * (100-hScrollBar1.Value);
            PlotGraph();
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
    }
}
