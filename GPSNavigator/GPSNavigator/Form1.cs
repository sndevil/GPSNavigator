using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using GPSNavigator.Source;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class Form1 : Form
    {

        #region definitions
        bool isRecording = true;
        bool isPlaying = true;
        List<string> Field = new List<string>();
        DataBuffer buffer = new DataBuffer();
        BinaryRawDataBuffer rbuffer = new BinaryRawDataBuffer();
        AttitudeInformation abuffer = new AttitudeInformation();
        List<Satellite> GPSSat = new List<Satellite>();
        List<Satellite> GLONASSsat = new List<Satellite>();
        List<byte[]> licenses = new List<byte[]>();

        #endregion
        public Form1()
        {
            InitializeComponent();
            try
            {
                serialPort1.Open();
            }
            catch
            {
                MessageBox.Show("Serial Port Not Found!");
            }
            for (int i = 0; i < 32; i++)
            {
                GPSSat.Add(new Satellite());
                GLONASSsat.Add(new Satellite());
                if (i < 16)
                    licenses.Add(new byte[16]);
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (isPlaying)
            {
                string input;
                try
                {
                    input = serialPort1.ReadLine();
                }
                catch
                {
                    input = "  ";
                }

               // handle_packet(input);


                //  If Logging was enabled, received message should be written to file


                //  if the data packet was a valid packet, it should be proccessed
                // 
                // try
                //{
                

                //}
                //catch (Exception ex)
                //{
                //     MessageBox.Show(ex.Message,"Error in processing NMEA data");
                //}


                WriteText(DateTime.Now + "  :  " + input);
            }
        }

        delegate void SetTextCallback(string text);

        private void WriteText(string text)
        {
            if (this.logger.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.logger.Text = text + "\r\n \r\n" + this.logger.Text;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                isPlaying = false;
                button2.Text = "Play";
            }
            else
            {
                isPlaying = true;
                button2.Text = "Pause";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void exit()
        {
            isPlaying = false;
            button2.Text = "Play";

            serialPort1.Close();
            this.Close();
            Application.Exit();
        }

        private void openLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opendialog.FileName = "";
            opendialog.Filter = "GLF File|*.glf|All Files|*.*";
            opendialog.ShowDialog();
        }

        public void OpenLogFile(string path)
        {
            FileStream temp = new FileStream(path, FileMode.OpenOrCreate);

            while (temp.Position < temp.Length)
            {
                if (temp.ReadByte() == '~')
                {
                    int msgType = temp.ReadByte();

                    int msgSize = Functions.checkMsgSize(msgType);
                    if (msgSize == -1)          //packet not valid
                        continue;

                    if (temp.Position + msgSize - 2 > temp.Length)
                        break;

                    byte[] byt = new byte[msgSize];
                    byt[0] = (byte)'~';
                    byt[1] = (byte)msgType;
                    temp.Read(byt, 2, msgSize - 2);
                    handle_packet(byt);
                    //Process_Received_BinaryBytes(byt, radioGroupDevice.SelectedIndex);
                }
            }
        }

        private void opendialog_FileOk(object sender, CancelEventArgs e)
        {
            OpenLogFile(opendialog.FileName);
        }

        public void handle_packet(byte[] packet)
        {
                var key = packet[1];
                if (key == Functions.BIN_FULL)
                    Functions.Process_Binary_Message_Full(packet, 1, buffer, GPSSat, GLONASSsat);
                else if (key == Functions.BIN_FULL_PLUS)
                    Functions.Process_Binary_Message_Full(packet, 1, buffer, GPSSat, GLONASSsat);
                else if (key == Functions.BIN_COMPACT)
                    Functions.Process_Binary_Message_Compact(packet, 1, buffer, GPSSat);
                else if (key == Functions.BIN_GPS_SUPPLEMENT)
                    Functions.Process_Binary_Message_SupplementGPS(packet, 1, GPSSat);
                else if (key == Functions.BIN_DEBUG)
                    MessageBox.Show(Functions.Process_Binary_Message_Debug(packet));
                else if (key == Functions.BIN_RAW_DATA)
                    Functions.Process_Binary_Message_RawData(packet, 1, rbuffer);
                else if (key == Functions.BIN_LICENCE)
                    Functions.Process_Binary_Message_Licence(packet, 1, licenses);
                else if (key == Functions.BIN_SETTING)
                    Functions.Process_Binary_Message_Setting(packet, 1);
                else if (key == Functions.BIN_ATTITUDE_INFO)
                    Functions.Process_Binary_Message_Attitude_Info(packet, 1, buffer, abuffer);
                else
                    WriteText("Couldnt Find the matching processor");

                WriteText(DateTime.Now + "  :  " + Encoding.UTF8.GetString(packet));
                var t = logger.Text;
        }
        public void handle_packet(string packet)
        {
            if (packet.StartsWith("$GP"))
                Functions.Process_Packet(packet, Field);
            else
            {
                byte[] d = Encoding.UTF8.GetBytes(packet);
                handle_packet(d);
            }

        }


        
    }
}
