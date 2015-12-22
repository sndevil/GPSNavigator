using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Resources;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GPSNavigator.Source;
using GPSNavigator.Classes;
using Ionic.Zip;
using Telerik.WinControls.UI.Docking;

namespace GPSNavigator
{
    public partial class Form1 : Form
    {

        #region definitions
        bool isRecording = false;
        bool isPlaying = true;
        bool gotdata = false;
        bool showdetail = false , showsky = false;
        bool saved = false;
        bool appclosing = false;
        public bool extractdone = false,justclosed = false;
        Globals vars = new Globals();
        public List<Grapher> grapherlist = new List<Grapher>();
        public List<MomentDetail> detaillist = new List<MomentDetail>();
        Logger log;
        DateTime RecordStarttime;
        int serialcounter = 0, packetcounter = 0, timeoutCounter = 0, MaxTimeout = 5, DetailRefreshCounter = 0, GraphRefreshCounter = 0,serial1_MsgSize=-1,RefreshRate = 50;
        MomentDetail DetailForm;
        Skyview SkyView;
        ExtremumHandler exthandler = new ExtremumHandler();
        byte[] byt = new byte[150];
        BinaryProtocolState Serial1State = BinaryProtocolState.waitForPacket;
        ZipFile z;
        ControlPanel controlPanel;
        enum Form1Status { Disconnected, Connected, Recording, Saving }
        //Form1Status status = Form1Status.Disconnected;
        public FolderManager folderManager;
        string logdir;
        #endregion

        public Form1()
        {
            InitializeComponent();
            folderManager = new FolderManager(AppDomain.CurrentDomain.BaseDirectory+"Logs\\");
            DetailForm = new MomentDetail(this);
            controlPanel = new ControlPanel(this);
            SkyView = new Skyview(vars);
            ToggleDetailForm();
            RefreshSerial();
            serialPorts.SelectedIndex = 0;
            if (OpenPort())
            {
                //status = Form1Status.Connected;
                StatusLabel.Text = "Connected";
                openPort.Text = "Close Port";
            }
        }

        void RefreshSerial()
        {
            serialPorts.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                serialPorts.Items.Add(s);
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            timeoutCounter = 0;
            gotdata = true;
            SingleDataBuffer dbuf;
            while (isPlaying && serialPort1.BytesToRead > ((Serial1State == BinaryProtocolState.readMessage) ? serial1_MsgSize - 3 : 0) && !appclosing)
            {

                switch (Serial1State)
                {
                    case BinaryProtocolState.waitForPacket:
                        serialPort1.Read(byt, 0, 1);
                        if (byt[0] == '~')
                            Serial1State = BinaryProtocolState.waitForMessageType;
                        break;

                    case BinaryProtocolState.waitForMessageType:
                        serialPort1.Read(byt, 1, 1);
                        Serial1State = BinaryProtocolState.readMessage;

                        int msgType = byt[1];

                        serial1_MsgSize = Functions.checkMsgSize(msgType);
                        if (serial1_MsgSize == -1)          //packet not valid
                            Serial1State = BinaryProtocolState.waitForPacket;
                        else
                        {
                            byt = new byte[serial1_MsgSize];
                            byt[0] = (byte)'~';
                            byt[1] = (byte)msgType;
                        }
                        break;

                    case BinaryProtocolState.readMessage:
                        if (serialPort1.BytesToRead < serial1_MsgSize - 2)
                            break;
                        Serial1State = BinaryProtocolState.waitForPacket;
                        serialPort1.Read(byt, 2, serial1_MsgSize - 2);
                        try
                        {
                            dbuf = Functions.handle_packet(byt, ref vars, 0);
                            if (DetailRefreshCounter++ > RefreshRate - 1)
                            {
                                if (showdetail)
                                {
                                    UpdateRealtimeData(dbuf);
                                    DetailRefreshCounter = 0;
                                }
                                if (showsky)
                                    SkyUpdater(); // SkyView.UpdateView(vars);
                            }
                            if (GraphRefreshCounter++ > GraphRefreshrate.Value - 1)
                            {
                                if (showdetail)
                                {
                                    UpdateRealtimeGraph(dbuf);
                                    GraphRefreshCounter = 0;
                                }
                            }

                            if (dbuf.settingbuffer.SettingReceived)
                                DetailForm.ChangeSettings(dbuf.settingbuffer);

                            if (isRecording)
                            {
                                //DetailForm.UpdateData(vars);
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
                                    if (dbuf.A > exthandler.AMax) { exthandler.AMax = dbuf.A; exthandler.BAMax = dbuf.BA; }
                                    if (dbuf.A < exthandler.Amin) { exthandler.Amin = dbuf.A; exthandler.BAmin = dbuf.BA; }
                                    if (dbuf.V > exthandler.VMax) { exthandler.VMax = dbuf.V; exthandler.BVMax = dbuf.BV; }
                                    if (dbuf.V < exthandler.VMin) { exthandler.VMin = dbuf.V; exthandler.BVMin = dbuf.BV; }
                                    if (dbuf.Altitude > exthandler.AltitudeMax) { exthandler.AltitudeMax = dbuf.Altitude; exthandler.BAltitudeMax = dbuf.BAltitude; }
                                    if (dbuf.Altitude < exthandler.AltitudeMin) { exthandler.AltitudeMin = dbuf.Altitude; exthandler.BAltitudeMin = dbuf.BAltitude; }
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
                            }
                        }
                        catch
                        {
                            ErrorCount.Text = (int.Parse(ErrorCount.Text) + 1).ToString();
                        }

                        if (DateTime.Now.Second != serialcounter)
                        {
                            serialcounter = DateTime.Now.Second;
                            if (ascii.Checked)
                                WriteText(packetcounter.ToString() + " Packet Per Second\r\n" + Encoding.UTF8.GetString(byt));
                            else if (hex.Checked)
                                WriteText(packetcounter.ToString() + " Packet Per Second\r\n0x" + ByteArrayToString(byt));
                            packetcounter = 0;
                        }
                        else
                            packetcounter++;
                        break;

                    default:
                        Serial1State = BinaryProtocolState.waitForPacket;
                        break;
                }
            }
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public void Serial1_Write(byte[] data,int offset, int count)
        {
            serialPort1.Write(data, offset, count);
        }

        public void ToggleDetailForm()
        {
            if (checkBox2.Checked)
                DetailForm.Show();
            else
                DetailForm.Hide();

        }

        private void serialPort2_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            timeoutCounter = 0;
            gotdata = true;
            SingleDataBuffer dbuf;
            byte[] byt = new byte[10];
            if (isPlaying)
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

                        dbuf = Functions.handle_packet(byt, ref vars, 2);
                        if (showdetail && DetailRefreshCounter++ > 50)
                        {
                            UpdateRealtimeData(dbuf);
                            DetailRefreshCounter = 0;
                        }
                        if (isRecording)
                        {
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
                                if (dbuf.A > exthandler.AMax) { exthandler.AMax = dbuf.A; exthandler.BAMax = dbuf.BA; }
                                if (dbuf.A < exthandler.Amin) { exthandler.Amin = dbuf.A; exthandler.BAmin = dbuf.BA; }
                                if (dbuf.V > exthandler.VMax) { exthandler.VMax = dbuf.V; exthandler.BVMax = dbuf.BV; }
                                if (dbuf.V < exthandler.VMin) { exthandler.VMin = dbuf.V; exthandler.BVMin = dbuf.BV; }
                                if (dbuf.Altitude > exthandler.AltitudeMax) { exthandler.AltitudeMax = dbuf.Altitude; exthandler.BAltitudeMax = dbuf.BAltitude; }
                                if (dbuf.Altitude < exthandler.AltitudeMin) { exthandler.AltitudeMin = dbuf.Altitude; exthandler.BAltitudeMin = dbuf.BAltitude; }
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
                        }

                    }
                    if (DateTime.Now.Second != serialcounter)
                    {
                        serialcounter = DateTime.Now.Second;
                        if (ascii.Checked)
                            WriteText(packetcounter.ToString() + " Packet Per Second\r\n" + Encoding.UTF8.GetString(byt));
                        else if (hex.Checked)
                            WriteText(packetcounter.ToString() + " Packet Per Second\r\n0x" + ByteArrayToString(byt));
                        packetcounter = 0;
                    }
                    else
                        packetcounter++;

                }
                else
                    WriteText("Receiving Data");
               
            }
        }

        delegate void SetTextCallback(string text);

        private void WriteText(string text)
        {
            try
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
            catch { }
        }

        delegate void SkyViewUpdater();

        private void SkyUpdater()
        {
            if (this.SkyView.InvokeRequired)
            {
                SkyViewUpdater d = new SkyViewUpdater(SkyUpdater);
                this.Invoke(d);
            }
            else
            {
                SkyView.UpdateView(vars);
            }
        }


        delegate void ShowRealtime(SingleDataBuffer databuffer);

        private void UpdateRealtimeData(SingleDataBuffer databuffer)
        {
            if (DetailForm.InvokeRequired)
            {
                ShowRealtime d = new ShowRealtime(UpdateRealtimeData);
                this.Invoke(d, new object[] { databuffer });
            }
            else
            {
                DetailForm.UpdateData(vars,databuffer,0);
            }
        }

        private void UpdateRealtimeGraph(SingleDataBuffer databuffer)
        {
            if (DetailForm.InvokeRequired)
            {
                ShowRealtime d = new ShowRealtime(UpdateRealtimeGraph);
                this.Invoke(d, new object[] { databuffer });
            }
            else
                DetailForm.UpdateRealtimeGraph(vars, databuffer, 0);

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //serialPort1.Close();
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
                button2.BackgroundImage = GPSNavigator.Properties.Resources.play;
                //button2.Text = "Play";
                DetailForm.paused = true;
                this.Text = "GPS Navigator";
            }
            else
            {
                isPlaying = true;
                button2.BackgroundImage = GPSNavigator.Properties.Resources.pause;
                DetailForm.paused = false;
                if (isRecording)
                    this.Text = "GPS Navigator (Recording)";
                else
                    this.Text = "GPS Navigator";
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
            this.Text = "GPS Navigator";
            //serialPort1.Close();
            this.Close();
            Application.Exit();
        }

        private void openLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opendialog.FileName = "";
            opendialog.Filter = "LogPackage File (*.GLP)|*.GLP|All Files|*.*";
            opendialog.ShowDialog();
        }

        public void OpenLogFile(string path)
        {
            LogFileManager file = new LogFileManager(path,ref vars);
            Grapher graphform;
            if (folderManager.readytouse.Count > 0)
            {
                folderManager.readytouse.Sort();
                graphform = new Grapher(file, this, folderManager.readytouse[0],opendialog.FileName);
            }
            else
                graphform = new Grapher(file, this, grapherlist.Count,opendialog.FileName);     
            //graphform.Dock = DockStyle.Fill;
            graphform.Dock = DockStyle.None;
            graphform.TopLevel = false;
            graphform.Show();
            //graphform.index = grapherlist.Count;
            if (folderManager.readytouse.Count > 0)
            {
                grapherlist[folderManager.readytouse[0]] = graphform;
                folderManager.readytouse.RemoveAt(0);
            }
            else
                grapherlist.Add(graphform);
            DocumentWindow NewDockWindow = new DocumentWindow("Grapher " + graphform.index.ToString());
            NewDockWindow.AutoScroll = true;
            NewDockWindow.Controls.Add(graphform);
            AddDocumentControl(NewDockWindow);
        }

        public void AddDocumentControl(DocumentWindow toadd)
        {
            radDock1.AddDocument(toadd);
        }

        private void opendialog_FileOk(object sender, CancelEventArgs e)
        {
                string path = folderManager.addfolder(); //  AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
                try
                {
                    Directory.Delete(path, true);
                }
                catch { }
                Directory.CreateDirectory(path);
                extractdone = false;
                StatusLabel.Text = "Loading Log";
                try
                {
                    logdir = path;
                    Task<bool> extractor = ExtractAsync(opendialog.FileName);
                }
                catch
                {
                    MessageBox.Show("There was an error opening the file");
                }
        }

        public Task<bool> ExtractAsync(string path)
        {

            return Task.Factory.StartNew(() =>
            {
                using (ZipFile z = ZipFile.Read(path))
                {
                    z.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(z_ExtractProgress);                   
                    z.ExtractAll(logdir);
                }
                extractdone = true;
                return true;
            });
        }

        void z_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EntriesTotal != 0)
            {
                float progress = (float)e.EntriesExtracted / e.EntriesTotal;
                ProgressbarChangeValue((int)(progress * 100));
            }

        }

        private void savedialog_FileOk(object sender, CancelEventArgs e)
        {
            if (showdetail)
                DetailForm.Show();
            var path = folderManager.findrecordfolder();
            log = new Logger(path);
            isRecording = true;
            //status = Form1Status.Recording;
            RecordStarttime = DateTime.Now;
            StatusLabel.Text = "Recording";
            this.Text = "GPS Navigator (Recording)";
            button1.BackgroundImage = GPSNavigator.Properties.Resources.stop;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isRecording && gotdata)
            {
                if (AutoCancel.Checked)
                    if (++timeoutCounter > MaxTimeout)
                        EndRecording();

                var dt = DateTime.Now - RecordStarttime;
                timeLabel.Text = dt.Hours.ToString("00") + " : " + dt.Minutes.ToString("00") + " : " + dt.Seconds.ToString("00");
            }
            if (saved)
            {
                ProgressbarChangeValue(0);
                //status = Form1Status.Connected;
                StatusLabel.Text = "Connected";
                this.Text = "GPS Navigator";
                saved = false;
            }
            if (extractdone)
            {
                OpenLogFile(logdir);
                ProgressbarChangeValue(0);
                StatusLabel.Text = "Connected";
                extractdone = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MaxTimeout = (int)numericUpDown1.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                showdetail = true;
                DetailForm = new MomentDetail(this);
                DetailForm.Dock = DockStyle.Left;
                DetailForm.TopLevel = false;
                DetailForm.Show();
                Telerik.WinControls.UI.Docking.DocumentWindow NewDockWindow = new Telerik.WinControls.UI.Docking.DocumentWindow("Moment Detail (RealTime)");
                NewDockWindow.AutoScroll = true;
                NewDockWindow.Controls.Add(DetailForm);
                radDock1.AddDocument(NewDockWindow);
            }
            else
            {
                showdetail = false;
                try
                {
                    //radDock1.
                    DetailForm.Hide();
                    foreach (DockWindow dw in radDock1.DocumentManager.DocumentArray)
                    {
                        if (dw.Text == "Moment Detail (RealTime)")
                            dw.Close();
                    }
                }
                catch
                {
                    DetailForm = new MomentDetail(this);
                }
            }
        }

        private void serialPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
                ClosePort();
            serialPort1.PortName = (string)serialPorts.SelectedItem;
        }

        private void openPort_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (OpenPort())
                {
                    //status = Form1Status.Connected;
                    StatusLabel.Text = "Connected";
                    openPort.Text = "Close Port";
                }
            }
            else
            {
                ClosePort();
            }
        }

        private bool OpenPort()
        {
            try
            {
                serialPort1.Open();
                return true;
            }
            catch
            {
                MessageBox.Show("Couldnt Open");
                return false;
            }
        }
        private void ClosePort()
        {
            serialPort1.Close();
            openPort.Text = "Open Port";
            //status = Form1Status.Disconnected;
            StatusLabel.Text = "Disconnected";
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            RefreshRate = (int)numericUpDown2.Value;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
            if (!isRecording)
            {
                savedialog.FileName = "";
                savedialog.Filter = "LogPackage File (*.GLP)|*.GLP";
                savedialog.ShowDialog();
            }
            else
            {
                if (savedialog.FileName != "")
                {
                    EndRecording();
                }
            }
        }

        public Task<bool> SaveAsync(string path)
        {
            return Task.Factory.StartNew(() =>
            {
                using (z = new ZipFile(path))
                {
                    z.SaveProgress += new EventHandler<SaveProgressEventArgs>(z_SaveProgress);
                    z.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                    z.CompressionMethod = CompressionMethod.None;
                    z.AddDirectory(log.Dirpath);
                    z.Save();
                }
                Directory.Delete(log.Dirpath,true);
                saved = true;
                return true;
            });
        }

        void z_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            if (e.EntriesTotal != 0)
            {
                float progress = (float)e.EntriesSaved / e.EntriesTotal;
                ProgressbarChangeValue((int)(progress * 100));
            }
        }

        private void EndRecording()
        {
            //status = Form1Status.Saving;
            StatusLabel.Text = "Saving";
            log.CloseFiles();
            try { File.Delete(savedialog.FileName); }
            catch { }
            timeLabel.Text = "                          ";
            button1.BackgroundImage = GPSNavigator.Properties.Resources.record;
            isRecording = false;
            gotdata = false;
            this.Text = "GPS Navigator (Packaging Log Files, Dont Close)";

            saved = false;
            Task<bool> asyncsaver = SaveAsync(savedialog.FileName);
            StatusLabel.Text = "Saving";
        }
        private delegate void Progressbar(int percent);

        private void ProgressbarChangeValue(int percent)
        {
            if (this.InvokeRequired)
            {
                Progressbar d = new Progressbar(ProgressbarChangeValue);
                this.Invoke(d, new object[] { percent });
            }
            else
            {
                toolStripProgressBar1.Value = percent;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            serialPort1.BaudRate = int.Parse((string)comboBox1.SelectedItem);
            /*if ((string)comboBox1.SelectedItem == "4800")
                serialPort1.BaudRate = 4800;
            else if ((string)comboBox1.SelectedItem == "115200")
                serialPort1.BaudRate = 115200;*/
        }

        private void controlPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlPanel = new ControlPanel(this);
            controlPanel.Dock = DockStyle.None;
            controlPanel.TopLevel = false;
            controlPanel.Show();
            Telerik.WinControls.UI.Docking.DocumentWindow NewDockWindow = new Telerik.WinControls.UI.Docking.DocumentWindow("Control Panel");
            NewDockWindow.AutoScroll = true;
            NewDockWindow.Controls.Add(controlPanel);
            radDock1.AddDocument(NewDockWindow);
        }

        private void serialControllerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialController.Show();
        }

        private void ErrorCount_Click(object sender, EventArgs e)
        {          
            ErrorCount.Text = "0";
        }

        private void radDock1_DockWindowClosed(object sender, DockWindowEventArgs e)
        {
            //var t = radDock1.DocumentManager.ActiveDocument.AccessibleName;
        }

        private void radDock1_DockWindowClosing(object sender, DockWindowCancelEventArgs e)
        {
            //try
            //{
            if (!justclosed)
            {
                var text = radDock1.DocumentManager.ActiveDocument.Text;
                char[] chararray = text.ToCharArray();
                
                if (text == "Moment Detail (RealTime)")
                {
                    checkBox2.Checked = false;
                }
                else if (text == "SkyView")
                {
                    SkyView.Hide();
                    showsky = false;
                }
                else if (text == "Control Panel")
                {

                }
                else
                {
                    var s = new string(chararray, 8, chararray.Length - 8);
                    int k = -1;
                    for (int j = 0; j < radDock1.DocumentManager.DocumentArray.Length; j++)
                    {
                        if (radDock1.DocumentManager.DocumentArray[j].Text.Contains("Moment Details (Log) " + s))
                        {
                            k = j;
                            break;
                        }
                    }
                    if (k > -1)
                    {
                        int i = int.Parse(s);
                        grapherlist[i].filemanager.Close();
                        radDock1.DocumentManager.DocumentArray[k].Close();
                        folderManager.removefolder(grapherlist[i].filemanager.filepath);
                        if (grapherlist.Count == i + 1)
                        {
                            Directory.Delete(grapherlist[i].filemanager.filepath, true);
                            grapherlist.RemoveAt(i);
                            detaillist.RemoveAt(i);
                        }
                        else
                            folderManager.readytouse.Add(i);
                        justclosed = true;
                    }
                }
            }
            else
                justclosed = false;

           // }
           // catch
           // {

           // }
            // filemanager.Close();
           // parentForm.folderManager.removefolder(filemanager.filepath);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            appclosing = true;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            RefreshSerial();
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutform = new About();
            aboutform.ShowDialog();
        }

        private void skyViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SkyView = new Skyview(vars);
            SkyView.Dock = DockStyle.None;
            SkyView.TopLevel = false;
            SkyView.Show();
            showsky = true;
            Telerik.WinControls.UI.Docking.DocumentWindow NewDockWindow = new Telerik.WinControls.UI.Docking.DocumentWindow("SkyView");
            NewDockWindow.AutoScroll = false;
            NewDockWindow.Controls.Add(SkyView);
            radDock1.AddDocument(NewDockWindow);
        }

    }
}
