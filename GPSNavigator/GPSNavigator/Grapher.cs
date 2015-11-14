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
using GPSNavigator;
using C1.Win.C1Chart;

namespace GPSNavigator
{
    public partial class Grapher : Form
    {
        public int min = 0, max = 1000;
        public long offset, bufferlength;
        public float zoom = 0.01f,fmin = 0.0f, fmax = 1f;
        public long length;
        public float delta;
        public bool UserchangedRanges = true;
       // public DataBuffer dbuffer;
        public LogFileManager filemanager;
        public PointStyle ps;

        public MomentDetail DetailForm;

        private graphtype selectedtype = graphtype.X;
        private DateTime emptytime = new DateTime();

        private double[] ylist = new double[Globals.Databuffercount];
        private double[] xlist = new double[Globals.Databuffercount];
        private DateTime[] tlist = new DateTime[Globals.Databuffercount];

        public Grapher(LogFileManager Filemanager)
        {
            filemanager = Filemanager;
            DetailForm = new MomentDetail(filemanager);
            DetailForm.Show();
            DetailForm.BringToFront();
            InitializeComponent();
            Chart1.MouseMove += new MouseEventHandler(Chart1_MouseMove);
            Chart1.MouseClick += new MouseEventHandler(Chart1_MouseClick);
            rangecontrol.ValueChanged += new EventHandler(rangecontrol_ValueChanged);
            textBox1.TextChanged += new EventHandler(text_Changed);
            textBox2.TextChanged += new EventHandler(text_Changed);
            rangecontrol.Properties.Maximum = 1000;
            rangecontrol.Properties.Minimum = 0;
            rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 1000);
        }

        void text_Changed(object sender, EventArgs e)
        {
            try
            {
                if (UserchangedRanges)
                {
                    var tmin = float.Parse(textBox2.Text);
                    var tmax = float.Parse(textBox1.Text);
                    if (tmax < tmin && tmax <= 1f && tmax >= 0f && tmin <= 1f && tmin >= 0f)
                        MessageBox.Show("Inputs not in a right format");
                    else
                    {
                        fmin = (tmin <= 1f && tmin >= 0f) ? tmin : fmin;
                        fmax = (tmax <= 1f && tmax >= 0f) ? tmax : fmax;
                        //rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange((int)(fmin * 1000), (int)(fmax * 1000));
                        LoadData();
                    }
                }
                else
                {
                    UserchangedRanges = true;
                }
            }
            catch
            {
            }
        }

        void rangecontrol_ValueChanged(object sender, EventArgs e)
        {
            fmin = rangecontrol.Value.Minimum / 1000f;
            fmax = rangecontrol.Value.Maximum / 1000f;
            UserchangedRanges = false;
            textBox2.Text = fmin.ToString();
            UserchangedRanges = false;
            textBox1.Text = fmax.ToString();
            LoadData();
        }


        void Chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (ps.PointIndex == -1)
                ps.PointIndex = 0;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var xpos = xlist[ps.PointIndex];
                fmin = (float)xpos - 0.005f;
                if (fmin < 0f)
                    fmin = 0f;
                fmax = fmin + 0.01f;
                DetailForm.UpdateData(xpos,tlist[ps.PointIndex]);
                UserchangedRanges = false;
                textBox2.Text = fmin.ToString();
                UserchangedRanges = false;
                textBox1.Text = fmax.ToString();
                rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange((int)(fmin*1000), (int)(fmax*1000));
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                fmin = 0f;
                offset = 0;
                UserchangedRanges = false;
                textBox2.Text = "0";
                UserchangedRanges = false;
                textBox1.Text = "1";
                fmax = 1f;
                rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 1000);
            }
            LoadData();
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
                        label3.Text = tlist[pointIndex].ToString();
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

        //Legacy
        /*
        public void PlotGraph()
        {
            int counter=0;
            float i = fmin/7000f;
            double temp = 0;
            var dt = delta / 7000f;
            for (counter = 0; counter < Globals.Databuffercount; counter++)
            {
                if (counter >= dbuffer.X.Count)
                {
                    for (int j = counter; j < Globals.Databuffercount; j++)
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
        */

        public void PlotGraph(GraphData data)
        {
            xlist = data.x;
            ylist = data.y;
            tlist = data.date;

            double[] time = new double[Globals.Databuffercount];
            for (int i = 0; i < Globals.Databuffercount; i++)
                time[i] = (data.date[i] == emptytime) ? double.NaN : (double)tlist[i].Ticks;

            if (fmax - fmin > 0.02f && selectedtype != graphtype.State && selectedtype != graphtype.Temperature && selectedtype != graphtype.UsedStats && selectedtype != graphtype.VisibleStats)
            {
                double[] max = Functions.FindMaxes(data.max);
                double[] min = Functions.FindMins(data.min);
                Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(time);
                Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.CopyDataIn(data.y);
                Chart1.ChartGroups[0].ChartData.SeriesList[1].X.CopyDataIn(time);
                Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.CopyDataIn(max);
                Chart1.ChartGroups[0].ChartData.SeriesList[2].X.CopyDataIn(time);
                Chart1.ChartGroups[0].ChartData.SeriesList[2].Y.CopyDataIn(min);
            }
            else
            {
                Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(time);
                Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.CopyDataIn(data.y);
                Chart1.ChartGroups[0].ChartData.SeriesList[1].X.Clear();
                Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.Clear();
                Chart1.ChartGroups[0].ChartData.SeriesList[2].X.Clear();
                Chart1.ChartGroups[0].ChartData.SeriesList[2].Y.Clear();
            }
        }

        public void PlotSingleDataGraph(List<double> buffer)
        {
            int counter = 0;
            float i = 0;
            double temp = 0;
            var dt = buffer.Count / Globals.Databuffercount;
            for (counter = 0; counter < Globals.Databuffercount; counter++)
            {

                temp = buffer[(int)i];

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    selectedtype = graphtype.X;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.X";
                    break;
                case 1:
                    selectedtype = graphtype.X_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.X_Processed";
                    break;
                case 2:
                    selectedtype = graphtype.Y;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Y";
                    break;
                case 3:
                    selectedtype = graphtype.Y_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Y_Processed";
                    break;
                case 4:
                    selectedtype = graphtype.Z;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Z";
                    break;
                case 5:
                    selectedtype = graphtype.Z_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Z_Processed";
                    break;
                case 6:
                    selectedtype = graphtype.Latitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Latitude";
                    break;
                case 7:
                    selectedtype = graphtype.Latitude_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Latitude_Processed";
                    break;
                case 8:
                    selectedtype = graphtype.Altitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude";
                    break;
                case 9:
                    selectedtype = graphtype.Altitude_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude_Processed";
                    break;
                case 10:
                    selectedtype = graphtype.Longitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude";
                    break;
                case 11:
                    selectedtype = graphtype.Longitude_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude_Processed";
                    break;
                case 12:
                    selectedtype = graphtype.Vx;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vx";
                    break;
                case 13:
                    selectedtype = graphtype.Vx_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vx_Processed";
                    break;
                case 14:
                    selectedtype = graphtype.Vy;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vy";
                    break;
                case 15:
                    selectedtype = graphtype.Vy_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vy_Processed";
                    break;
                case 16:
                    selectedtype = graphtype.Vz;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vz";
                    break;
                case 17:
                    selectedtype = graphtype.Vz_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Vz_Processed";
                    break;
                case 18:
                    selectedtype = graphtype.Ax;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ax";
                    break;
                case 19:
                    selectedtype = graphtype.Ay;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ay";
                    break;
                case 20:
                    selectedtype = graphtype.Az;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Az";
                    break;
                case 21:
                    selectedtype = graphtype.A;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.A";
                    break;
                case 22:
                    selectedtype = graphtype.PDOP;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.PDOP";
                    break;
                case 23:
                    selectedtype = graphtype.State;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.State";
                    break;
                case 24:
                    selectedtype = graphtype.Temperature;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Temperature";
                    break;
                case 25:
                    selectedtype = graphtype.UsedStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.UsedStats";
                    break;
                case 26:
                    selectedtype = graphtype.VisibleStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.VisibleSats";
                    break;
            }
            LoadData();
        }

        public void LoadData()
        {
            var t = filemanager.Readbuffer(selectedtype, fmin, fmax, Globals.Databuffercount);
            PlotGraph(t);
        }
    }
}
