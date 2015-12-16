using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class Skyview : Form
    {
        Telerik.WinControls.UI.RadLabel[] GPS_satpos_label = new Telerik.WinControls.UI.RadLabel[32];
        Telerik.WinControls.UI.RadLabel[] GLONASS_satpos_label = new Telerik.WinControls.UI.RadLabel[32];

        Satellite[] GPS = new Satellite[32];
        Satellite[] GLONASS = new Satellite[32];
        Globals vars;

        DevExpress.Utils.ToolTipTitleItem GPStoolTipTitle1Item = new DevExpress.Utils.ToolTipTitleItem();
        DevExpress.Utils.ToolTipTitleItem[] GPStoolTipTitle2Item = new DevExpress.Utils.ToolTipTitleItem[32];
        DevExpress.Utils.ToolTipItem[] GPStoolTipItem = new DevExpress.Utils.ToolTipItem[32];
        DevExpress.Utils.SuperToolTip[] GPSsuperToolTip = new DevExpress.Utils.SuperToolTip[32];
        DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem = new DevExpress.Utils.ToolTipSeparatorItem();

        DevExpress.Utils.ToolTipTitleItem GLONASStoolTipTitle1Item = new DevExpress.Utils.ToolTipTitleItem();
        DevExpress.Utils.ToolTipTitleItem[] GLONASStoolTipTitle2Item = new DevExpress.Utils.ToolTipTitleItem[32];
        DevExpress.Utils.ToolTipItem[] GLONASStoolTipItem = new DevExpress.Utils.ToolTipItem[32];
        DevExpress.Utils.SuperToolTip[] GLONASSsuperToolTip = new DevExpress.Utils.SuperToolTip[32];

        private const double Elevation_OutOfRange = 91;
        private const double Azimuth_OutOfRange = 361;

        public Skyview(Globals globalvars)
        {
            InitializeComponent();
            init_Labels();
            vars = globalvars;
        }

        public void UpdateView(Globals globalvars)//SingleDataBuffer data)
        {
            vars = globalvars;
            if (vars.GPSlist.Count > 0)
            {
                Update_GPS_Sat_Position(vars.GPSlist[0]);
                Update_GLONASS_Sat_Position(vars.GLONASSlist[0]);
            }
        }


        private void Show_GPS_Sat_Pos(Satellite[] sats, int Num)
        {
            if (sats[Num].Signal_Status == 2)
                GPS_satpos_label[Num].BackColor = Color.FromArgb(155, 187, 89);       //Green
            else if (sats[Num].Signal_Status == 1)
                GPS_satpos_label[Num].BackColor = Color.FromArgb(79, 129, 189);       //Blue
            else
                GPS_satpos_label[Num].BackColor = Color.FromArgb(192, 80, 77);        //Red

            GPS_satpos_label[Num].Visible = true;

            double label_left_origin_set = ultraPictureBoxSkyView.Width / 2 - GPS_satpos_label[Num].Width / 2;          //set label center to sky view center
            double label_top_origin_set = ultraPictureBoxSkyView.Height / 2 - GPS_satpos_label[Num].Height / 2;

            GPS_satpos_label[Num].Left = (int)(label_left_origin_set + ((ultraPictureBoxSkyView.Width - 40) / 2 * (90 - sats[Num].Elevation) / 90) * Math.Sin(sats[Num].Azimuth * Math.PI / 180));
            GPS_satpos_label[Num].Top = (int)(label_top_origin_set - ((ultraPictureBoxSkyView.Height - 40) / 2 * (90 - sats[Num].Elevation) / 90) * Math.Cos(sats[Num].Azimuth * Math.PI / 180));

            Set_GPS_sat_label_tooltip(Num);
        }

        private void Show_GLONASS_Sat_Pos(Satellite[] sats, int Num)
        {
            if (sats[Num].Signal_Status == 2)
                GLONASS_satpos_label[Num].BackColor = Color.FromArgb(155, 187, 89);       //Green
            else if (sats[Num].Signal_Status == 1)
                GLONASS_satpos_label[Num].BackColor = Color.FromArgb(79, 129, 189);       //Blue
            else
                GLONASS_satpos_label[Num].BackColor = Color.FromArgb(192, 80, 77);        //Red

            GLONASS_satpos_label[Num].Visible = true;

            double label_left_origin_set = ultraPictureBoxSkyView.Width / 2 - GLONASS_satpos_label[Num].Width / 2;          //set label center to sky view center
            double label_top_origin_set = ultraPictureBoxSkyView.Height / 2 - GLONASS_satpos_label[Num].Height / 2;

            GLONASS_satpos_label[Num].Left = (int)(label_left_origin_set + ((ultraPictureBoxSkyView.Width - 40) / 2 * (90 - sats[Num].Elevation) / 90) * Math.Sin(sats[Num].Azimuth * Math.PI / 180));
            GLONASS_satpos_label[Num].Top = (int)(label_top_origin_set - ((ultraPictureBoxSkyView.Height - 40) / 2 * (90 - sats[Num].Elevation) / 90) * Math.Cos(sats[Num].Azimuth * Math.PI / 180));

            Set_GLONASS_sat_label_tooltip(Num);
        }

        private void Update_GPS_Sat_Position(Satellite[] sats)
        {
            for (int i = 0; i < 32; i++)
                if (sats[i].Elevation != Elevation_OutOfRange && sats[i].Azimuth != Azimuth_OutOfRange)
                    Show_GPS_Sat_Pos(sats, i);
                else
                    GPS_satpos_label[i].Visible = false;
        }

        private void Update_GLONASS_Sat_Position(Satellite[] sats)
        {
            for (int i = 0; i < 28; i++)
                if (sats[i].Elevation != Elevation_OutOfRange && sats[i].Azimuth != Azimuth_OutOfRange)
                    Show_GLONASS_Sat_Pos(sats, i);
                else
                    GLONASS_satpos_label[i].Visible = false;
        }

        private void Set_GPS_sat_label_tooltip(int Num)
        {
            string status;

            if (vars.GPSlist[0][Num].Signal_Status == 2)
                status = "Used in navigation";
            else if (vars.GPSlist[0][Num].Signal_Status == 1)
                status = "Signal Available";
            else
                status = "Signal Not Available";

            GPStoolTipItem[Num].Text = "Elevation = " + vars.GPSlist[0][Num].Elevation.ToString() + "\r\nAzimuth = " + vars.GPSlist[0][Num].Azimuth.ToString() + "\r\nSNR = " + vars.GPSlist[0][Num].SNR.ToString();
            GPStoolTipTitle2Item[Num].Text = "Status : " + status;
        }

        private void Set_GLONASS_sat_label_tooltip(int Num)
        {
            string status;

            if (vars.GLONASSlist[0][Num].Signal_Status == 2)
                status = "Used in navigation";
            else if (vars.GLONASSlist[0][Num].Signal_Status == 1)
                status = "Signal Available";
            else
                status = "Signal Not Available";

            GLONASStoolTipItem[Num].Text = "Elevation = " + vars.GLONASSlist[0][Num].Elevation.ToString() + "\r\nAzimuth = " + vars.GLONASSlist[0][Num].Azimuth.ToString() + "\r\nSNR = " + vars.GLONASSlist[0][Num].SNR.ToString();
            GLONASStoolTipTitle2Item[Num].Text = "Status : " + status;
        }

        private void init_Labels()
        {
            //GPS
            for (int i = 0; i < 32; i++)
            {
                GPS_satpos_label[i] = new Telerik.WinControls.UI.RadLabel();
                GPS_satpos_label[i].Parent = ultraPictureBoxSkyView;
                GPS_satpos_label[i].Text = "P" + (i + 1).ToString();
                GPS_satpos_label[i].ForeColor = Color.White;
                GPS_satpos_label[i].Visible = false;

                Create_GPS_sat_label_tooltips(i);
            }

            //GLONASS
            for (int i = 0; i < 28; i++)
            {
                GLONASS_satpos_label[i] = new Telerik.WinControls.UI.RadLabel();
                GLONASS_satpos_label[i].Parent = ultraPictureBoxSkyView;
                GLONASS_satpos_label[i].Text = "L" + (i + 1).ToString();
                GLONASS_satpos_label[i].ForeColor = Color.White;
                GLONASS_satpos_label[i].Visible = false;

                Create_GLONASS_sat_label_tooltips(i);
            }

            //Date_Label = ultraGaugeClock.Annotations[0] as EllipseAnnotation;
            //Date_Label_NorthFinder = ultraGaugeClock_NorthFinder.Annotations[0] as EllipseAnnotation;
        }

        private void Create_GPS_sat_label_tooltips(int i)
        {
            GPStoolTipTitle2Item[i] = new DevExpress.Utils.ToolTipTitleItem();
            GPStoolTipItem[i] = new DevExpress.Utils.ToolTipItem();
            GPSsuperToolTip[i] = new DevExpress.Utils.SuperToolTip();

            GPStoolTipTitle1Item.Text = "GPS Satellite";
            GPStoolTipItem[i].Appearance.Image = GPSNavigator.Properties.Resources.satellite;
            GPStoolTipItem[i].Appearance.Options.UseImage = true;
            GPStoolTipItem[i].Image = GPSNavigator.Properties.Resources.satellite;
            GPStoolTipItem[i].LeftIndent = 6;
            GPStoolTipTitle2Item[i].LeftIndent = 6;
            GPSsuperToolTip[i].Items.Add(GPStoolTipTitle1Item);
            GPSsuperToolTip[i].Items.Add(GPStoolTipItem[i]);
            GPSsuperToolTip[i].Items.Add(toolTipSeparatorItem);
            GPSsuperToolTip[i].Items.Add(GPStoolTipTitle2Item[i]);
            this.toolTipController1.SetSuperTip(GPS_satpos_label[i], GPSsuperToolTip[i]);
        }

        private void Create_GLONASS_sat_label_tooltips(int i)
        {
            GLONASStoolTipTitle2Item[i] = new DevExpress.Utils.ToolTipTitleItem();
            GLONASStoolTipItem[i] = new DevExpress.Utils.ToolTipItem();
            GLONASSsuperToolTip[i] = new DevExpress.Utils.SuperToolTip();

            GLONASStoolTipTitle1Item.Text = "GLONASS Satellite";
            GLONASStoolTipItem[i].Appearance.Image = GPSNavigator.Properties.Resources.satellite;
            GLONASStoolTipItem[i].Appearance.Options.UseImage = true;
            GLONASStoolTipItem[i].Image = GPSNavigator.Properties.Resources.satellite;
            GLONASStoolTipItem[i].LeftIndent = 6;
            GLONASStoolTipTitle2Item[i].LeftIndent = 6;
            GLONASSsuperToolTip[i].Items.Add(GLONASStoolTipTitle1Item);
            GLONASSsuperToolTip[i].Items.Add(GLONASStoolTipItem[i]);
            GLONASSsuperToolTip[i].Items.Add(toolTipSeparatorItem);
            GLONASSsuperToolTip[i].Items.Add(GLONASStoolTipTitle2Item[i]);
            this.toolTipController1.SetSuperTip(GLONASS_satpos_label[i], GLONASSsuperToolTip[i]);
        }

    }
}
