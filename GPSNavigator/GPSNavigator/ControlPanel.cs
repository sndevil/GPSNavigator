using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GPSNavigator.Source;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class ControlPanel : Form
    {
        public Form1 Parentform;
        public SettingBuffer Settings;

        public ControlPanel(Form1 Parent)
        {
            InitializeComponent();
            FirstUserInit();
            Parentform = Parent;
        }

        public void FirstUserInit()
        {
            PositionTypeCombo.SelectedIndex =2;
            Settings = new SettingBuffer();
            Settings.GPSNum = 12;
            Settings.GLONASSNum = 12;
            Settings.GalileoNum = 12;
            Settings.CompassNum = 12;
            Settings.SatNum = 14;
            Settings.PosType = 0;

        }

        private void ApplySearch_Click(object sender, EventArgs e)
        {
           // try
           // {
            Settings.GPSNum = (int)GPSMax.Value;
            Settings.GLONASSNum = (int)GlonassMax.Value;
            Settings.GalileoNum = (int)GalileoMax.Value;
            Settings.CompassNum = (int)CompassMax.Value;
            Settings.SatNum = (int)AllSatsMax.Value;
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
                Msg[index] = (char) Settings.SatNum; //(char)Convert.ToInt16(radDDLTypAll.Text);
                index++;

                Msg[index] = (char) Settings.GPSNum;//Convert.ToInt16(radDDLTypGPS.Text);
                index++;

                Msg[index] = (char) Settings.GLONASSNum;//Convert.ToInt16(radDDLTypGLONASS.Text);
                index++;

                Msg[index] = (char) Settings.GalileoNum;//Convert.ToInt16(radDDLTypGalileo.Text);
                index++;

                Msg[index] = (char) Settings.CompassNum;//Convert.ToInt16(radDDLTypCompass.Text);
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
                    //SelectedSerialPort.Write(byteMsg, i, 1);
                    Thread.Sleep(20);
                    Application.DoEvents();
                }
                ApplySearch.Enabled = true;
           // }
           // catch (Exception ex)
           // {
                //Telerik.WinControls.RadMessageBox.Show(ex.Message, "Error in sending command");
           //     MessageBox.Show("Sending Error");
            //}
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
                Thread.Sleep(20);
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
                Parentform.Serial1_Write(byteMsg, i,1);              
                Thread.Sleep(20);
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
            Msg[index] = (char)GreenSatCombo.SelectedIndex ; //(Convert.ToInt16(radDDLGreenType.FindStringExact(radDDLGreenType.Text)));
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
                Thread.Sleep(20);
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
                    Thread.Sleep(20);
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
    }
}
