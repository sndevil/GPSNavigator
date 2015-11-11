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
        bool isRecording = false;
        bool isPlaying = true;
        bool gotdata = false;
        Globals vars = new Globals();
        FileStream temp;
        Logger log;
        int serialcounter = 0,packetcounter = 0, timeoutCounter = 0,MaxTimeout = 5;
        ExtremumHandler exthandler = new ExtremumHandler();
        string message = "";
        #endregion

        public Form1()
        {
            InitializeComponent();
            /*
            var NAN = 16.1234;
            var byt = BitConverter.GetBytes(NAN);
            var p = BitConverter.ToDouble(byt, 0);
            var t = Functions.CopyByteWithOffset(byt, 4);
            var re = Functions.BytetoFloat(t);
            MessageBox.Show(Encoding.UTF8.GetString(BitConverter.GetBytes(NAN)));
            */
            try
            {
                serialPort1.Open();
            }
            catch
            {
                MessageBox.Show("Serial Port Not Found!");
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            timeoutCounter = 0;
            gotdata = true;
            SingleDataBuffer dbuf;
            byte[] byt = new byte[10];
            if (isPlaying)
            {
                if (isRecording)
                {
                    var header = serialPort1.ReadByte();
                    if (header == '~')
                    {
                        int msgType = serialPort1.ReadByte();

                        int msgSize = Functions.checkMsgSize(msgType);
                        if (msgSize != -1)
                        {

                            byt = new byte[msgSize];
                            byt[0] = (byte)'~';
                            byt[1] = (byte)msgType;
                            serialPort1.Read(byt, 2, msgSize - 2);
                            
                            dbuf = Functions.handle_packet(byt, vars);

                            #region Extremum_Ifs
                           // message += dbuf.X + "  |  ";
                            if (exthandler.ExtremumStarted)
                            {
                                if (exthandler.ExtremeCounter++ >= 100)
                                {
                                    exthandler.ExtremeCounter = 0;
                                    dbuf.WriteExtreme = true;
                                    dbuf.BAMax = exthandler.BAMax;
                                    dbuf.BAMin = exthandler.BAmin;
                                    dbuf.BAxMax = exthandler.BAxMax;
                                    dbuf.BAxMin = exthandler.BAxMin;
                                    dbuf.BAyMax = exthandler.BAyMax;
                                    dbuf.BAyMin = exthandler.BAyMin;
                                    dbuf.BAzMax = exthandler.BAzMax;
                                    dbuf.BAzMin = exthandler.BAzMin;
                                    dbuf.BVMax = exthandler.BVMax;
                                    dbuf.BVMin = exthandler.BVMin;
                                    dbuf.BVxMax = exthandler.BVxMax;
                                    dbuf.BVxMin = exthandler.BVxMin;
                                    dbuf.BVyMax = exthandler.BVyMax;
                                    dbuf.BVyMin = exthandler.BVyMin;
                                    dbuf.BVzMax = exthandler.BVzMax;
                                    dbuf.BVzMin = exthandler.BVzMin;
                                    dbuf.BXMax = exthandler.BXMax;
                                    dbuf.BXMin = exthandler.BXMin;
                                    dbuf.BYMax = exthandler.BYMax;
                                    dbuf.BYMin = exthandler.BYMin;
                                    dbuf.BZMax = exthandler.BZMax;
                                    dbuf.BZMin = exthandler.BZMin;
                                    dbuf.BPDOPMax = exthandler.BPDOPMax;
                                    dbuf.BPDOPMin = exthandler.BPDOPMin;
                                    dbuf.BAltitudeMax = exthandler.BAltitudeMax;
                                    dbuf.BAltitudeMin = exthandler.BAltitudeMin;
                                    dbuf.BLatitudeMax = exthandler.BLatitudeMax;
                                    dbuf.BLatitudeMin = exthandler.BLatitudeMin;
                                    dbuf.BLongitudeMax = exthandler.BLongitudeMax;
                                    dbuf.BLongitudeMin = exthandler.BLongitudeMin;
                                    exthandler.ExtremumStarted = false;
                                }
                                if (dbuf.A > exthandler.AMax){ exthandler.AMax = dbuf.A; exthandler.BAMax = dbuf.BA;}
                                if (dbuf.A < exthandler.Amin){ exthandler.Amin = dbuf.A; exthandler.BAmin = dbuf.BA;}
                                if (dbuf.V > exthandler.VMax) { exthandler.VMax = dbuf.V; exthandler.BVMax = dbuf.BV; }
                                if (dbuf.V < exthandler.VMin) { exthandler.VMin = dbuf.V; exthandler.BVMin = dbuf.BV; }
                                if (dbuf.Altitude > exthandler.AltitudeMax){ exthandler.AltitudeMax = dbuf.Altitude; exthandler.BAltitudeMax = dbuf.BAltitude;}
                                if (dbuf.Altitude < exthandler.AltitudeMin){ exthandler.AltitudeMin = dbuf.Altitude; exthandler.BAltitudeMin = dbuf.BAltitude;}
                                if (dbuf.Latitude > exthandler.LatitudeMax) { exthandler.LatitudeMax = dbuf.Latitude; exthandler.BLatitudeMax = dbuf.BLatitude; }
                                if (dbuf.Latitude < exthandler.LatitudeMin) { exthandler.LatitudeMin = dbuf.Latitude; exthandler.BLatitudeMin = dbuf.BLatitude; }
                                if (dbuf.Longitude > exthandler.LongitudeMax) { exthandler.LongitudeMax = dbuf.Longitude; exthandler.BLongitudeMax = dbuf.BLongitude; }
                                if (dbuf.Longitude < exthandler.LongitudeMin) { exthandler.LongitudeMin = dbuf.Longitude; exthandler.BLongitudeMin = dbuf.BLongitude; }
                                if (dbuf.Ax > exthandler.AxMax) { exthandler.AxMax = dbuf.Ax; exthandler.BAxMax = dbuf.BAx; }
                                if (dbuf.Ax < exthandler.AxMin) { exthandler.AxMin = dbuf.Ax; exthandler.BAxMin = dbuf.BAx; }
                                if (dbuf.Ay > exthandler.AyMax) { exthandler.AyMax = dbuf.Ay; exthandler.BAyMax = dbuf.BAy; }
                                if (dbuf.Ay < exthandler.AyMin) { exthandler.AyMin = dbuf.Ay; exthandler.BAyMin = dbuf.BAy; }
                                if (dbuf.Az > exthandler.AzMax) { exthandler.AzMax = dbuf.Az; exthandler.BAzMax = dbuf.BAz; }
                                if (dbuf.Az < exthandler.AzMin) { exthandler.AzMin = dbuf.Az; exthandler.BAzMin = dbuf.BAz; }
                                if (dbuf.Vx > exthandler.VxMax) { exthandler.VxMax = dbuf.Vx; exthandler.BVxMax = dbuf.BVx; }
                                if (dbuf.Vx < exthandler.VxMin) { exthandler.VxMin = dbuf.Vx; exthandler.BVxMin = dbuf.BVx; }
                                if (dbuf.Vy > exthandler.VyMax) { exthandler.VyMax = dbuf.Vy; exthandler.BVyMax = dbuf.BVy; }
                                if (dbuf.Vy < exthandler.VyMin) { exthandler.VyMin = dbuf.Vy; exthandler.BVyMin = dbuf.BVy; }
                                if (dbuf.Vz > exthandler.VzMax) { exthandler.VzMax = dbuf.Vz; exthandler.BVzMax = dbuf.BVz; }
                                if (dbuf.Vz < exthandler.VzMin) { exthandler.VzMin = dbuf.Vz; exthandler.BVzMin = dbuf.BVz; }
                                if (dbuf.X > exthandler.XMax) { exthandler.XMax = dbuf.X; exthandler.BXMax = dbuf.BX; }
                                if (dbuf.X < exthandler.XMin) { exthandler.XMin = dbuf.X; exthandler.BXMin = dbuf.BX; }
                                if (dbuf.Y > exthandler.YMax) { exthandler.YMax = dbuf.Y; exthandler.BYMax = dbuf.BY; }
                                if (dbuf.Y < exthandler.YMin) { exthandler.YMin = dbuf.Y; exthandler.BYMin = dbuf.BY; }
                                if (dbuf.Z > exthandler.ZMax) { exthandler.ZMax = dbuf.Z; exthandler.BZMax = dbuf.BZ; }
                                if (dbuf.Z < exthandler.ZMin) { exthandler.ZMin = dbuf.Z; exthandler.BZMin = dbuf.BZ; }
                                if (dbuf.PDOP > exthandler.PDOPMax) { exthandler.PDOPMax = dbuf.PDOP; exthandler.BPDOPMax = dbuf.BPDOP; }
                                if (dbuf.PDOP < exthandler.PDOPMin) { exthandler.PDOPMin = dbuf.PDOP; exthandler.BPDOPMin = dbuf.BPDOP; }
                            }
                            else
                            {
                                exthandler.ExtremumStarted = true;
                                exthandler.AltitudeMax = exthandler.AltitudeMin = dbuf.Altitude;
                                exthandler.BAltitudeMax = exthandler.BAltitudeMin = dbuf.BAltitude;
                                exthandler.AMax = exthandler.Amin = dbuf.A;
                                exthandler.BAMax = exthandler.BAmin = dbuf.BA;
                                exthandler.AxMax = exthandler.AxMin = dbuf.Ax;
                                exthandler.BAxMax = exthandler.BAxMin = dbuf.BAx;
                                exthandler.AyMax = exthandler.AyMin = dbuf.Ay;
                                exthandler.BAyMax = exthandler.BAyMin = dbuf.BAy;
                                exthandler.AzMax = exthandler.AzMin = dbuf.Az;
                                exthandler.BAzMax = exthandler.BAzMin = dbuf.BAz;
                                exthandler.LatitudeMax = exthandler.LatitudeMin = dbuf.Latitude;
                                exthandler.BLatitudeMax = exthandler.BLatitudeMin = dbuf.BLatitude;
                                exthandler.LongitudeMax = exthandler.LongitudeMin = dbuf.Longitude;
                                exthandler.BLongitudeMax = exthandler.BLongitudeMin = dbuf.BLongitude;
                                exthandler.PDOPMax = exthandler.PDOPMin = dbuf.PDOP;
                                exthandler.BPDOPMax = exthandler.BPDOPMin = dbuf.BPDOP;
                                exthandler.VMax = exthandler.VMin = dbuf.V;
                                exthandler.BVMax = exthandler.BVMin = dbuf.BV;
                                exthandler.VxMax = exthandler.VxMin = dbuf.Vx;
                                exthandler.BVxMax = exthandler.BVxMin = dbuf.BVx;
                                exthandler.VyMax = exthandler.VyMin = dbuf.Vy;
                                exthandler.BVyMax = exthandler.BVyMin = dbuf.BVy;
                                exthandler.VzMax = exthandler.VzMin = dbuf.Vz;
                                exthandler.BVzMax = exthandler.BVzMin = dbuf.BVz;
                                exthandler.XMax = exthandler.XMin = dbuf.X;
                                exthandler.BXMax = exthandler.BXMin = dbuf.BX;
                                exthandler.YMax = exthandler.YMin = dbuf.Y;
                                exthandler.BYMax = exthandler.BYMin = dbuf.BY;
                                exthandler.ZMax = exthandler.ZMin = dbuf.Z;
                                exthandler.BZMax = exthandler.BZMin = dbuf.BZ;
                                exthandler.ExtremeCounter++;
                            }
                            #endregion

                            log.Writebuffer(dbuf);
                            //log.WriteTimebuffer(dbuf);
                        }
                        //WriteText(DateTime.Now. + "  :  " + header);
                        if (DateTime.Now.Second != serialcounter)
                        {
                            serialcounter = DateTime.Now.Second;
                            WriteText(packetcounter.ToString() + " Packet Per Second\r\n" + Encoding.UTF8.GetString(byt));
                            packetcounter = 0;
                        }
                        else
                            packetcounter++;

                    }
                    else
                        WriteText("Receiving Data");
                    //vars.buffer.
                }
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
                this.logger.Text = text;
            }
        }
        private void Enable_Timer()
        {
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
            folderdialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderdialog.ShowDialog();
            if (folderdialog.SelectedPath != "")
            {
                OpenLogFile(folderdialog.SelectedPath);
            }
        }

        public void OpenLogFile(string path)
        {
            LogFileManager file = new LogFileManager(path,ref vars);
           // file.ClearBuffer();
            //var temp = file.Readbuffer();
            Grapher graphform = new Grapher(file);
            graphform.Show(this);
            graphform.BringToFront();
        }

        private void opendialog_FileOk(object sender, CancelEventArgs e)
        {
            OpenLogFile(opendialog.FileName);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Start();
            if (checkBox1.Checked)
            {
                folderdialog.RootFolder = Environment.SpecialFolder.Desktop;
                folderdialog.ShowDialog();
                if (folderdialog.SelectedPath != "")
                {
                    log = new Logger(folderdialog.SelectedPath+"\\");
                    isRecording = true;
                }
            }
            else
            {
                log.CloseFiles();
                isRecording = false;
                gotdata = false;
            }
        }

        private void savedialog_FileOk(object sender, CancelEventArgs e)
        {
            log = new Logger(AppDomain.CurrentDomain.BaseDirectory + "Logs\\");
            isRecording = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isRecording && gotdata)
            {
                if (timeoutCounter++ > MaxTimeout)
                    checkBox1.Checked = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MaxTimeout = (int)numericUpDown1.Value;
        }

    }
}
