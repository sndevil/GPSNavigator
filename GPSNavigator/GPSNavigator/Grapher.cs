﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows;
using GPSNavigator.Classes;
using GPSNavigator.Source;
using GPSNavigator;
using C1.Win.C1Chart;
using Telerik.WinControls.UI.Docking;

namespace GPSNavigator
{
    public partial class Grapher : Form
    {
        public int min = 0, max = 1000,index;
        public long offset, bufferlength;
        public float zoom = 0.01f,fmin = 0.0f, fmax = 1f,zoommin,zoommax;
        public double xpos = 0.0;
        public long length;
        public float delta;
        public bool UserchangedRanges = true , mousedown = false;
       // public DataBuffer dbuffer;
        public LogFileManager filemanager;
        public PointStyle ps;
        private GraphData temp;
        public MomentDetail DetailForm;
        public NorthDetail NorthDetailForm;
        public RTKDetail RTKDetailForm;
        public Form1 parentForm;

        private graphtype selectedtype = graphtype.X;
        private DateTime emptytime = new DateTime();

        private double[] ylist = new double[Globals.Databuffercount];
        private double[] xlist = new double[Globals.Databuffercount];
        private DateTime[] tlist = new DateTime[Globals.Databuffercount];

        public AppModes Logtype;

        public Grapher(LogFileManager Filemanager, Form1 Parent,int Index,string OriginalPath, AppModes Mode)
        {
            Logtype = Mode;
            parentForm = Parent;
            filemanager = Filemanager;
            index = Index;
            InitializeComponent();
            InitializeCombobox();

            CreateDetailForm();
            this.Dock = DockStyle.Fill;
            label5.Parent = FileAdressLabel.Parent = Chart1;
            FileAdressLabel.BackColor = Color.Transparent;
            FileAdressLabel.Text = OriginalPath;
            Chart1.MouseDown += new MouseEventHandler(Chart1_MouseDown);
            Chart1.MouseUp += new MouseEventHandler(Chart1_MouseUp);
            Chart1.MouseMove += new MouseEventHandler(Chart1_MouseMove);
            Chart1.MouseClick += new MouseEventHandler(Chart1_MouseClick);
            rangecontrol.ValueChanged += new EventHandler(rangecontrol_ValueChanged);
            textBox1.TextChanged += new EventHandler(text_Changed);
            textBox2.TextChanged += new EventHandler(text_Changed);
            rangecontrol.Properties.Maximum = 1000;
            rangecontrol.Properties.Minimum = 0;
            rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 1000);
        }

        public void InitializeCombobox()
        {
            this.comboBox1.Items.AddRange(new object[] {
            "X",
            "X_Processed",
            "Y",
            "Y_Processed",
            "Z",
            "Z_Processed",
            "Latitude",
            "Latitude_Processed",
            "Longitude",
            "Longitude_Processed",
            "Altitude",
            "Altitude_Processed",
            "Vx",
            "Vx_Processed",
            "Vy",
            "Vy_Processed",
            "Vz",
            "Vz_Processed",
            "V",
            "Ax",
            "Ay",
            "Az",
            "A",
            "PDOP",
            "State",
            "Temperature",
            "UsedSats",
            "VisibleSats"});
            if (Logtype == AppModes.NorthFinder)
            {
                this.comboBox1.Items.AddRange(new object[] { "Azimuth", "Elevation", "Distance" });
            }
        }

        void CreateDetailForm()
        {
            DocumentWindow NewDockWindow;
            switch (Logtype)
            {
                case AppModes.RTK:
                case AppModes.GPS:
                    DetailForm = new MomentDetail(filemanager);
                    DetailForm.Dock = DockStyle.None;
                    DetailForm.TopLevel = false;
                    DetailForm.Show();
                    NewDockWindow = new DocumentWindow("Moment Details (Log) " + index.ToString());
                    NewDockWindow.AutoScroll = true;
                    NewDockWindow.Controls.Add(DetailForm);
                    parentForm.AddDocumentControl(NewDockWindow);
                    parentForm.detaillist.Add(DetailForm);
                    break;
                case AppModes.NorthFinder:
                    NorthDetailForm = new NorthDetail(filemanager);
                    NorthDetailForm.Dock = DockStyle.None;
                    NorthDetailForm.TopLevel = false;
                    NorthDetailForm.Show();
                    NewDockWindow = new DocumentWindow("Moment Details (Log) " + index.ToString());
                    NewDockWindow.AutoScroll = true;
                    NewDockWindow.Controls.Add(NorthDetailForm);
                    parentForm.AddDocumentControl(NewDockWindow);
                    parentForm.northDetailList.Add(NorthDetailForm);
                    break;
            }

        }

        void Chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && mousedown)
            {
                mousedown = false;
                zoommax = (float)xpos;
                if (Math.Abs(zoommax - zoommin) > 0.0001f)
                {
                    if (zoommax < zoommin)
                    {
                        fmax = zoommin;
                        fmin = zoommax;
                    }
                    else
                    {
                        fmax = zoommax;
                        fmin = zoommin;
                    }
                    LoadData();
                    if (Control.ModifierKeys == Keys.Alt)
                        for (int i = 0; i < parentForm.radDock1.DocumentManager.DocumentArray.Length; i++)
                        {
                            if (parentForm.radDock1.DocumentManager.DocumentArray[i].Text == "Moment Details (Log) " + index.ToString())
                                parentForm.radDock1.ActivateWindow(parentForm.radDock1.DocumentManager.DocumentArray[i]);
                        }
                    else if (Control.ModifierKeys == Keys.Control)
                    {
                        var length = Chart1.ChartGroups[0].ChartData[0].X.Length;
                        Chart1.ChartGroups[0].ChartData[3].X.Clear();
                        Chart1.ChartGroups[0].ChartData[3].Y.Clear();
                        var counter = 0;
                        double avg = 0;
                        var max = 50;
                        for (int i = 0; i < length; i++)
                        {
                            avg += (double)Chart1.ChartGroups[0].ChartData[0].Y[i];
                            if (counter++ >= max)
                            {
                                avg /= max+1;
                                //for (int j = i - max; j < i; j++)
                                //{
                                    Chart1.ChartGroups[0].ChartData[3].X.Add(Chart1.ChartGroups[0].ChartData[0].X[i-max/2]);
                                    Chart1.ChartGroups[0].ChartData[3].Y.Add(avg);
                                //}
                                counter = 0;
                                avg = 0;
                            }

                        }                        

                    }

                }
            }
        }

        void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mousedown = true;
                zoommin = (float)xpos;
            }

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
                xpos = xlist[ps.PointIndex];
                fmin = (float)xpos - 0.005f;
                if (fmin < 0f)
                    fmin = 0f;
                fmax = fmin + 0.01f;
                if (Logtype == AppModes.GPS)
                    DetailForm.UpdateData(xpos, tlist[ps.PointIndex]);
                else if (Logtype == AppModes.NorthFinder)
                    NorthDetailForm.UpdateData(xpos, tlist[ps.PointIndex]);
                else if (Logtype == AppModes.RTK)
                    DetailForm.UpdateData(xpos, tlist[ps.PointIndex]);
                UserchangedRanges = false;
                textBox2.Text = fmin.ToString();
                UserchangedRanges = false;
                textBox1.Text = fmax.ToString();
                rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange((int)(fmin*1000), (int)(fmax*1000));
                var found = false;
                if (Control.ModifierKeys == Keys.Alt)
                {
                    for (int i = 0; i < parentForm.radDock1.DocumentManager.DocumentArray.Length; i++)
                    {
                        if (parentForm.radDock1.DocumentManager.DocumentArray[i].Text == "Moment Details (Log) " + index.ToString())
                        {
                            parentForm.radDock1.ActivateWindow(parentForm.radDock1.DocumentManager.DocumentArray[i]);
                            found = true;
                        }
                    }
                    if (!found)
                        CreateDetailForm();
                }

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
            if (!mousedown)
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
                        xpos = xlist[pointIndex];
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

        public void PlotGraph(GraphData data)
        {
            Chart1.ChartGroups[0].ChartData[3].X.Clear();
            Chart1.ChartGroups[0].ChartData[3].Y.Clear();
            xlist = data.x;
            ylist = data.y;
            tlist = data.date;

            double[] time = new double[Globals.Databuffercount];
            for (int i = 0; i < Globals.Databuffercount; i++)
                time[i] = (data.date[i] == emptytime) ? double.NaN : (double)tlist[i].Ticks;

            if (fmax - fmin > 0.02f && (data.hasStateExt || selectedtype != graphtype.State) && selectedtype != graphtype.Temperature && selectedtype != graphtype.UsedStats && selectedtype != graphtype.VisibleStats)
            {
                double[] max = Functions.FindMaxes(data.max);
                double[] min = Functions.FindMins(data.min);
                Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(data.date);
                Chart1.ChartGroups[0].ChartData.SeriesList[0].Y.CopyDataIn(data.y);
                if (MaxCheck.Checked)
                {
                    Chart1.ChartGroups[0].ChartData.SeriesList[1].X.CopyDataIn(data.date);//time
                    Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.CopyDataIn(max);
                }
                else
                {
                    Chart1.ChartGroups[0].ChartData.SeriesList[1].X.Clear();
                    Chart1.ChartGroups[0].ChartData.SeriesList[1].Y.Clear();
                }
                if (MinCheck.Checked)
                {
                    Chart1.ChartGroups[0].ChartData.SeriesList[2].X.CopyDataIn(data.date);//time
                    Chart1.ChartGroups[0].ChartData.SeriesList[2].Y.CopyDataIn(min);
                }
                else
                {
                    Chart1.ChartGroups[0].ChartData.SeriesList[2].X.Clear();
                    Chart1.ChartGroups[0].ChartData.SeriesList[2].Y.Clear();
                }
            }
            else
            {
                Chart1.ChartGroups[0].ChartData.SeriesList[0].X.CopyDataIn(data.date);
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
                    selectedtype = graphtype.Longitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude";
                    break;
                case 9:
                    selectedtype = graphtype.Longitude_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Longitude_Processed";
                    break;
                case 10:
                    selectedtype = graphtype.Altitude;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude";
                    break;
                case 11:
                    selectedtype = graphtype.Altitude_p;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Altitude_Processed";
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
                    selectedtype = graphtype.V;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.V";
                    break;
                case 19:
                    selectedtype = graphtype.Ax;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ax";
                    break;
                case 20:
                    selectedtype = graphtype.Ay;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Ay";
                    break;
                case 21:
                    selectedtype = graphtype.Az;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Az";
                    break;
                case 22:
                    selectedtype = graphtype.A;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.A";
                    break;
                case 23:
                    selectedtype = graphtype.PDOP;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.PDOP";
                    break;
                case 24:
                    selectedtype = graphtype.State;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.State";
                    break;
                case 25:
                    selectedtype = graphtype.Temperature;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Temperature";
                    break;
                case 26:
                    selectedtype = graphtype.UsedStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.UsedStats";
                    break;
                case 27:
                    selectedtype = graphtype.VisibleStats;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.VisibleSats";
                    break;
                case 28:
                    selectedtype = graphtype.Azimuth;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Azimuth";
                    break;
                case 29:
                    selectedtype = graphtype.Elevation;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Elevation";
                    break;
                case 30:
                    selectedtype = graphtype.Distance;
                    Chart1.ChartArea.Axes[1].Text = "Buffer.Distance";
                    break;
            }
            LoadData();
        }

        public void LoadData()
        {
            temp = filemanager.Readbuffer(selectedtype, fmin, fmax, Globals.Databuffercount);
            PlotGraph(temp);
        }

        private void Grapher_FormClosed(object sender, FormClosedEventArgs e)
        {
            filemanager.Close();
            parentForm.folderManager.removefolder(filemanager.filepath);
        }

        private void MinCheck_CheckedChanged(object sender, EventArgs e)
        {
            PlotGraph(temp);
        }

        private void MaxCheck_CheckedChanged(object sender, EventArgs e)
        {
            PlotGraph(temp);
        }

        private void SaveImage_Click(object sender, EventArgs e)
        {
            ImageSaveDialog.Filter = "Jpeg File (*.jpeg)|*.jpeg|Bitmap File (*.bmp)|*.bmp|PNG File (*.png)|*.PNG";
            ImageSaveDialog.ShowDialog();
        }

        private void ExportData_Click(object sender, EventArgs e)
        {
            DataExportDialog.Filter = "GPS Navigator Graph Data (*.ggd)|*.ggd|Matlab Data (*.Dat)|*.dat";            
            DataExportDialog.ShowDialog();
        }

        private void ImportData_Click(object sender, EventArgs e)
        {
            DataImportDialog.Filter = "GPS Navigator Graph Data (*.ggd)|*.ggd";
            DataImportDialog.ShowDialog();
        }

        private void ImageSaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                switch (ImageSaveDialog.FilterIndex)
                {
                    case 1:
                        Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        Chart1.SaveImage(ImageSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
            }
            catch
            {
                throw new Exception("Couldnt Save Image");
            }
        }

        private void DataExportDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (DataExportDialog.FilterIndex != 2)
                Chart1.SaveChartToFile(DataExportDialog.FileName);
            else
            {
                FileStream f = new FileStream(DataExportDialog.FileName, FileMode.Create, FileAccess.Write);
                byte[] output = Encoding.UTF8.GetBytes("x,y\n");
                f.Write(output,0,output.Length);
                for (int i = 0; i < Chart1.ChartGroups[0].ChartData.SeriesList[0].X.Length; i++)
                {
                    var y = Chart1.ChartGroups[0].ChartData.SeriesList[0].Y[i];
                    
                    string s = i.ToString()+","+y.ToString()+"\n";
                    f.Write(Encoding.UTF8.GetBytes(s),0,s.Length);
                }
                f.Close();

            }

        }

        private void DataImportDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                Chart1.LoadChartFromFile(DataImportDialog.FileName);
            }
            catch
            {
                throw new Exception("Couldnt Open File");
            }

        }

        public void CloseFiles()
        {
            filemanager.Close();
        }
    }
}
