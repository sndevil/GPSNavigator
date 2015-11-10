
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using GPSNavigator.Source;


namespace GPSNavigator.Classes
{
        public class DataBuffer
        {
            public List<double> Altitude = new List<double>();
            public List<double> Latitude = new List<double>();
            public List<double> Longitude = new List<double>();
            public List<double> X = new List<double>();
            public List<double> Y = new List<double>();
            public List<double> Z = new List<double>();
            public List<double> Vx = new List<double>();
            public List<double> Vy= new List<double>();
            public List<double> Vz = new List<double>();
            public List<double> V = new List<double>();
            public List<double> Ax = new List<double>();
            public List<double> Ay = new List<double>();
            public List<double> Az= new List<double>();
            public List<double> A= new List<double>();
            public List<double> PDOP = new List<double>();
            public List<double> Altitude_Processed = new List<double>();
            public List<double> Latitude_Processed = new List<double>();
            public List<double> Longitude_Processed = new List<double>();
            public List<double> X_Processed = new List<double>();
            public List<double> Y_Processed = new List<double>();
            public List<double> Z_Processed = new List<double>();
            public List<double> Vx_Processed = new List<double>();
            public List<double> Vy_Processed = new List<double>();
            public List<double> Vz_Processed = new List<double>();
            public List<double> V_Processed = new List<double>();
            public List<double> Temperature = new List<double>();
            public List<double> NumOfUsedSats = new List<double>();
            public List<double> NumOfVisibleSats = new List<double>();
            public List<double> state = new List<double>();
            public List<DateTime> datetime = new List<DateTime>();
            public List<int> statcounter = new List<int>();
            public int counter = 0;
            public int counterDraw = 0;
            public int overLoad = 0;
            //File Path
            public string Directory;
            public string Altitude_FilePath;
            public string Latitude_FilePath;
            public string Longitude_FilePath;
            public string X_FilePath;
            public string Y_FilePath;
            public string Z_FilePath;
            public string Vx_FilePath;
            public string Vy_FilePath;
            public string Vz_FilePath;
            public string V_FilePath;
            public string Ax_FilePath;
            public string Ay_FilePath;
            public string Az_FilePath;
            public string A_FilePath;
            public string Altitude_Processed_FilePath;
            public string Latitude_Processed_FilePath;
            public string Longitude_Processed_FilePath;
            public string X_Processed_FilePath;
            public string Y_Processed_FilePath;
            public string Z_Processed_FilePath;
            public string Vx_Processed_FilePath;
            public string Vy_Processed_FilePath;
            public string Vz_Processed_FilePath;
            public string V_Processed_FilePath;
            public string PDOP_FilePath;
            public string State_FilePath;
            public string Temperature_FilePath;
            public string NumOfUsedSats_FilePath;
            public string NumOfVisibleSats_FilePath;
            public string datetime_FilePath;

        }

        public class SingleDataBuffer
        {
            public double Altitude;
            public byte[] BAltitude = new byte[4], BAltitudeMax = new byte[4], BAltitudeMin = new byte[4];
            public double Latitude;
            public byte[] BLatitude = new byte[4], BLatitudeMax = new byte[4], BLatitudeMin = new byte[4];
            public double Longitude;
            public byte[] BLongitude = new byte[4], BLongitudeMax = new byte[4], BLongitudeMin = new byte[4];
            public double X;
            public byte[] BX = new byte[4], BXMax = new byte[4], BXMin = new byte[4];
            public double Y;
            public byte[] BY = new byte[4], BYMax = new byte[4], BYMin = new byte[4];
            public double Z;
            public byte[] BZ = new byte[4], BZMax = new byte[4], BZMin = new byte[4];
            public double Vx;
            public byte[] BVx = new byte[4], BVxMax = new byte[4], BVxMin = new byte[4];
            public double Vy;
            public byte[] BVy = new byte[4], BVyMax = new byte[4],BVyMin = new byte[4];
            public double Vz;
            public byte[] BVz = new byte[4], BVzMax = new byte[4], BVzMin = new byte[4];
            public double V;
            public byte[] BV = new byte[4], BVMax = new byte[4], BVMin = new byte[4];
            public double Ax;
            public byte[] BAx = new byte[4], BAxMax = new byte[4], BAxMin = new byte[4];
            public double Ay;
            public byte[] BAy = new byte[4], BAyMax = new byte[4], BAyMin = new byte[4];
            public double Az;
            public byte[] BAz = new byte[4], BAzMax = new byte[4], BAzMin = new byte[4];
            public double A;
            public byte[] BA = new byte[4], BAMax = new byte[4], BAMin = new byte[4];
            public double PDOP;
            public byte[] BPDOP = new byte[4], BPDOPMax = new byte[4], BPDOPMin = new byte[4];
            public double Altitude_Processed;
            public byte[] BAltitude_Processed = new byte[4];
            public double Latitude_Processed;
            public byte[] BLatitude_Processed = new byte[4];
            public double Longitude_Processed;
            public byte[] BLongitude_Processed = new byte[4];
            public double X_Processed;
            public byte[] BX_Processed = new byte[4];
            public double Y_Processed;
            public byte[] BY_Processed = new byte[4];
            public double Z_Processed;
            public byte[] BZ_Processed = new byte[4];
            public double Vx_Processed;
            public byte[] BVx_Processed = new byte[4];
            public double Vy_Processed;
            public byte[] BVy_Processed = new byte[4];
            public double Vz_Processed;
            public byte[] BVz_Processed = new byte[4];
            public double V_Processed;
            public byte[] BV_Processed = new byte[4];
            public double Temperature;
            public byte BTemperature;
            public double NumOfUsedSats;
            public byte[] BNumOfUsedStats = new byte[4];
            public double NumOfVisibleSats;
            public byte[] BNumOfVisibleStats = new byte[4];
            public double state;
            public byte Bstate;
            public DateTime datetime;
            public byte[] Bdatetime = new byte[6];
            public int statcounter;
            public bool WriteExtreme = false;
        }

        public class ExtremumHandler
        {
            public double AltitudeMax, AltitudeMin;
            public byte[] BAltitudeMax, BAltitudeMin;
            public double LatitudeMax, LatitudeMin;
            public byte[] BLatitudeMax, BLatitudeMin;
            public double LongitudeMax, LongitudeMin;
            public byte[] BLongitudeMax, BLongitudeMin;
            public double XMax,XMin;
            public byte[] BXMax, BXMin;
            public double YMax,YMin;
            public byte[] BYMax, BYMin;
            public double ZMax,ZMin;
            public byte[] BZMax, BZMin;
            public double VxMax,VxMin;
            public byte[] BVxMax, BVxMin;
            public double VyMax,VyMin;
            public byte[] BVyMax, BVyMin;
            public double VzMax,VzMin;
            public byte[] BVzMax, BVzMin;
            public double VMax,VMin;
            public byte[] BVMax, BVMin;
            public double AxMax,AxMin;
            public byte[] BAxMax, BAxMin;
            public double AyMax,AyMin;
            public byte[] BAyMax, BAyMin;
            public double AzMax,AzMin;
            public byte[] BAzMax, BAzMin;
            public double AMax,Amin;
            public byte[] BAMax, BAmin;
            public double PDOPMax,PDOPMin;
            public byte[] BPDOPMax, BPDOPMin;
            public bool ExtremumStarted = false;
            public int ExtremeCounter = 0;
        }

        public class BinaryRawDataBuffer
        {
            public List<int> Temperature = new List<int>();

            public List<double> TOW = new List<double>();
            public List<double[]> PseudoRanges = new List<double[]>();
            public List<int[]> Prn = new List<int[]>();
            public List<double[]> dopplers = new List<double[]>();
            public List<double[]> reliability = new List<double[]>();
            public List<double[][]> pAngle = new List<double[][]>();
            public List<double[][]> snr = new List<double[][]>();
            public List<CartesianCoordinate[]> satPos = new List<CartesianCoordinate[]>();
            public List<double[]> satPos_X = new List<double[]>();
            public int counter = 0;
            public int overLoad = 0;
        }

        public class AttitudeInformation
        {
            public List<double> Azimuth = new List<double>();
            public List<double> Elevation = new List<double>();
            public List<double> X= new List<double>();
            public List<double> Y = new List<double>();
            public List<double> Z = new List<double>();
            public List<double> Distance = new List<double>();
            public List<DateTime> datetime = new List<DateTime>();
            public int counter = 0;
        }

        public enum BinaryProtocolState
        {
            waitForPacket,
            waitForMessageType,
            readMessage
        };
        public enum graphtype { X, Y, Z, Vx, Vy, Vz, Ax, Ay, Az, Latitude, Longitude, Altitude, PDOP, State, Temperature, UsedStats, VisibleStats };

        public class GEOpoint
        {
            public double Latitude;
            public double Longitude;
            public double Altitude;
        }

        public class XYZpoint
        {
            public double x;
            public double y;
            public double z;
        }

        public class CartesianCoordinate
        {
            public double x;
            public double y;
            public double z;
        }

        public enum SatType { GPS, GLONASS };

        public class Satellite
        {
            public double Elevation; /*{ get; set; }*/
            public double Azimuth;
            public double SNR;
            public SatType satType;
            //0 = Satellite signal not available    1 = Satellite signal available, not available for use in navigation    2 = Satellite used in navigation
            public int Signal_Status;
        }

        public class GraphData
        {
            public double[] x;
            public double[] y;
            public DateTime[] date;
            public List<double> max;
            public List<double> min;
            public GraphData(int Points)
            {
                x = new double[Points];
                y = new double[Points];
                date = new DateTime[Points];
                max = new List<double>();
                min = new List<double>();
            }
        }


        public class LogFileManager
        {
            public long start = 0;
            public long end;
            public int position = 0;
            public float zoom = 1f,delta;
            public DateTime StartTime, EndTime;
            public long StartTick, EndTick, Duration;
            public string filepath;

            private FileStream stream,maxstream,minstream,timestream;
            private FileStream Vx, Vy, Vz, Ax, Ay, Az, X, Y, Z, Altitude, Latitude, Longitude, PDOP, state, Temperature, UsedStats, VisibleStats, V, A;
            private FileStream VxMax, VxMin, VyMax, VyMin, VzMax, VzMin, AxMax, AxMin, AyMax, AyMin, AzMax, AzMin, XMax, XMin, YMax, YMin, ZMax, ZMin, VMax, VMin, AMax, AMin;
            private FileStream AltitudeMax, AltitudeMin, LatitudeMax, LatitudeMin, LongitudeMax, LongitudeMin, PDOPMax, PDOPMin;

            private Globals vars;

            public LogFileManager(string path,ref Globals variables)
            {
                try
                {
                    #region initializing_streams
                    V = new FileStream(path + "\\V.glf", FileMode.Open, FileAccess.Read);
                    VMax = new FileStream(path + "\\VMax.glf", FileMode.Open, FileAccess.Read);
                    VMin = new FileStream(path + "\\VMin.glf", FileMode.Open, FileAccess.Read);
                    A = new FileStream(path + "\\A.glf", FileMode.Open, FileAccess.Read);
                    AMax = new FileStream(path + "\\AMax.glf", FileMode.Open, FileAccess.Read);
                    AMin = new FileStream(path + "\\AMin.glf", FileMode.Open, FileAccess.Read);
                    Vx = new FileStream(path + "\\Vx.glf", FileMode.Open, FileAccess.Read);
                    VxMax = new FileStream(path + "\\VxMax.glf", FileMode.Open, FileAccess.Read);
                    VxMin = new FileStream(path + "\\VxMin.glf", FileMode.Open, FileAccess.Read);
                    Vy = new FileStream(path + "\\Vy.glf", FileMode.Open, FileAccess.Read);
                    VyMax = new FileStream(path + "\\VyMax.glf", FileMode.Open, FileAccess.Read);
                    VyMin = new FileStream(path + "\\VyMin.glf", FileMode.Open, FileAccess.Read);
                    Vz = new FileStream(path + "\\Vz.glf", FileMode.Open, FileAccess.Read);
                    VzMax = new FileStream(path + "\\VzMax.glf", FileMode.Open, FileAccess.Read);
                    VzMin = new FileStream(path + "\\VzMin.glf", FileMode.Open, FileAccess.Read);
                    Ax = new FileStream(path + "\\Ax.glf", FileMode.Open, FileAccess.Read);
                    AxMax = new FileStream(path + "\\AxMax.glf", FileMode.Open, FileAccess.Read);
                    AxMin = new FileStream(path + "\\AxMin.glf", FileMode.Open, FileAccess.Read);
                    Ay = new FileStream(path + "\\Ay.glf", FileMode.Open, FileAccess.Read);
                    AyMax = new FileStream(path + "\\AyMax.glf", FileMode.Open, FileAccess.Read);
                    AyMin = new FileStream(path + "\\AyMin.glf", FileMode.Open, FileAccess.Read);
                    Az = new FileStream(path + "\\Az.glf", FileMode.Open, FileAccess.Read);
                    AzMax = new FileStream(path + "\\AzMax.glf", FileMode.Open, FileAccess.Read);
                    AzMin = new FileStream(path + "\\AzMin.glf", FileMode.Open, FileAccess.Read);
                    X = new FileStream(path + "\\X.glf", FileMode.Open, FileAccess.Read);
                    XMax = new FileStream(path + "\\XMax.glf", FileMode.Open, FileAccess.Read);
                    XMin = new FileStream(path + "\\XMin.glf", FileMode.Open, FileAccess.Read);
                    Y = new FileStream(path + "\\Y.glf", FileMode.Open, FileAccess.Read);
                    YMax = new FileStream(path + "\\YMax.glf", FileMode.Open, FileAccess.Read);
                    YMin = new FileStream(path + "\\YMin.glf", FileMode.Open, FileAccess.Read);
                    Z = new FileStream(path + "\\Z.glf", FileMode.Open, FileAccess.Read);
                    ZMax = new FileStream(path + "\\ZMax.glf", FileMode.Open, FileAccess.Read);
                    ZMin = new FileStream(path + "\\ZMin.glf", FileMode.Open, FileAccess.Read);
                    Altitude = new FileStream(path + "\\Altitude.glf", FileMode.Open, FileAccess.Read);
                    AltitudeMax = new FileStream(path + "\\AltitudeMax.glf", FileMode.Open, FileAccess.Read);
                    AltitudeMin = new FileStream(path + "\\AltitudeMin.glf", FileMode.Open, FileAccess.Read);
                    Latitude = new FileStream(path + "\\Latitude.glf", FileMode.Open, FileAccess.Read);
                    LatitudeMax = new FileStream(path + "\\LatitudeMax.glf", FileMode.Open, FileAccess.Read);
                    LatitudeMin = new FileStream(path + "\\LatitudeMin.glf", FileMode.Open, FileAccess.Read);
                    Longitude = new FileStream(path + "\\Longitude.glf", FileMode.Open, FileAccess.Read);
                    LongitudeMax = new FileStream(path + "\\LongitudeMax.glf", FileMode.Open, FileAccess.Read);
                    LongitudeMin = new FileStream(path + "\\LongitudeMin.glf", FileMode.Open, FileAccess.Read);
                    PDOP = new FileStream(path + "\\PDOP.glf", FileMode.Open, FileAccess.Read);
                    PDOPMax = new FileStream(path + "\\PDOPMax.glf", FileMode.Open, FileAccess.Read);
                    PDOPMin = new FileStream(path + "\\PDOPMin.glf", FileMode.Open, FileAccess.Read);
                    state = new FileStream(path + "\\state.glf", FileMode.Open, FileAccess.Read);
                    Temperature = new FileStream(path + "\\Temperature.glf", FileMode.Open, FileAccess.Read);
                    UsedStats = new FileStream(path + "\\UsedStats.glf", FileMode.Open, FileAccess.Read);
                    VisibleStats = new FileStream(path + "\\VisibleStats.glf", FileMode.Open, FileAccess.Read);
                    timestream = new FileStream(path + "\\Time.glf", FileMode.Open, FileAccess.Read);
                    #endregion
                }
                catch
                {
                    throw new Exception("Log Files Opening Error");
                }
            }

            //Legacy
            public DataBuffer Readbuffer()
            {
                vars.Clear_buffer();
                stream.Position = start;
                var counter = 0;
                while (true)
                {
                    if (stream.ReadByte() == '~')
                    {
                        int msgType = stream.ReadByte();

                        int msgSize = Functions.checkMsgSize(msgType);
                        if (msgSize == -1)          //packet not valid
                            continue;

                        if (stream.Position + msgSize - 2 > stream.Length || counter++ >= Globals.Databuffercount)
                            break;

                        byte[] byt = new byte[msgSize];
                        byt[0] = (byte)'~';
                        byt[1] = (byte)msgType;
                        stream.Read(byt, 2, msgSize - 2);
                        Functions.handle_packet(byt,ref vars);
                        stream.Position += (int)delta - msgSize;
                        //Process_Received_BinaryBytes(byt, radioGroupDevice.SelectedIndex);
                    }
                    if (stream.Position >= stream.Length - 10)
                        break;
                }
                return vars.buffer;
            }

            public GraphData Readbuffer(graphtype type,float fstart, float fend,int gpoints)
            {
                #region stream_switch
                switch (type)
                {
                    case graphtype.Altitude:
                        stream = Altitude;
                        maxstream = AltitudeMax;
                        minstream = AltitudeMin;
                        break;
                    case graphtype.Ax:
                        stream = Ax;
                        maxstream = AxMax;
                        minstream = AxMin;
                        break;
                    case graphtype.Ay:
                        stream = Ay;
                        maxstream = AyMax;
                        minstream = AyMin;
                        break;
                    case graphtype.Az:
                        stream = Az;
                        maxstream = AzMax;
                        minstream = AzMin;
                        break;
                    case graphtype.Latitude:
                        stream = Latitude;
                        maxstream = LatitudeMax;
                        minstream = LatitudeMin;
                        break;
                    case graphtype.Longitude:
                        stream = Longitude;
                        maxstream = LongitudeMax;
                        minstream = LongitudeMin;
                        break;
                    case graphtype.PDOP:
                        stream = PDOP;
                        maxstream = PDOPMax;
                        minstream = PDOPMin;
                        break;
                    case graphtype.State:
                        stream = state;
                        maxstream = minstream = state;
                        break;
                    case graphtype.Temperature:
                        stream = Temperature;
                        maxstream = minstream = Temperature;
                        break;
                    case graphtype.UsedStats:
                        stream = UsedStats;
                        maxstream = minstream = UsedStats;
                        break;
                    case graphtype.VisibleStats:
                        stream = VisibleStats;
                        maxstream = minstream = VisibleStats;
                        break;
                    case graphtype.Vx:
                        stream = Vx;
                        maxstream = VxMax;
                        minstream = VxMin;
                        break;
                    case graphtype.Vy:
                        stream = Vy;
                        maxstream = VyMax;
                        minstream = VyMin;
                        break;
                    case graphtype.Vz:
                        stream = Vz;
                        maxstream = VzMax;
                        minstream = VzMin;
                        break;
                    case graphtype.X:
                        stream = X;
                        maxstream = XMax;
                        minstream = XMin;
                        break;
                    case graphtype.Y:
                        stream = Y;
                        maxstream = YMax;
                        minstream = YMin;
                        break;
                    case graphtype.Z:
                        stream = Z;
                        maxstream = ZMax;
                        minstream = ZMin;
                        break;
                }
                #endregion

                GraphData tempgraphdata = new GraphData(gpoints);

                if (stream.Length > 0)
                {
                    stream.Position = Functions.QuantizePosition(fstart * stream.Length);
                    maxstream.Position = minstream.Position = Functions.QuantizePosition(fstart * minstream.Length);
                    timestream.Position = Functions.QuantizePosition6bit(fstart * timestream.Length);

                    var db = (float)((fend - fstart) * stream.Length / gpoints);
                    var tdb = (float)((fend - fstart) * timestream.Length / gpoints);
                    var mdb = (float)((fend - fstart) * maxstream.Length);
                    var counter = 0;
                    int extflag = 1;
                    float pos = stream.Position;
                    float tpos = timestream.Position;
                    byte[] byt = new byte[4];
                    byte[] tbyt = new byte[6];
                    while (true)
                    {
                        double t,dt;
                        if (type == graphtype.State || type == graphtype.Temperature)
                        {
                            t = stream.ReadByte();
                            pos += db;
                            stream.Position = (int)(pos);
                        }
                        else
                        {
                            stream.Read(byt, 0, 4);
                            if (type != graphtype.UsedStats && type != graphtype.VisibleStats)
                                t = Functions.BytetoFloat(byt);
                            else
                                t = Functions.BytetoFloatOther(byt);
                            pos += db;
                            stream.Position = Functions.QuantizePosition(pos);
                        }

                        timestream.Read(tbyt,0,6);
                        tpos += tdb;
                        timestream.Position = Functions.QuantizePosition6bit(tpos);

                        tempgraphdata.x[counter] = (double)stream.Position/stream.Length;//(double)stream.Position / stream.Length;
                        tempgraphdata.y[counter] = t;
                        tempgraphdata.date[counter] = Functions.ReadDateTime(tbyt);

                        if (counter++ >= gpoints - 1)
                            break;
                        if (stream.Position >= stream.Length - 3 && type != graphtype.State && type != graphtype.Temperature)
                        {
                            for (int i = counter; i < gpoints; i++)
                            {
                                tempgraphdata.x[i] = (double)counter / gpoints;
                                tempgraphdata.y[i] = t;
                                tempgraphdata.date[i] = Functions.ReadDateTime(tbyt);
                            }
                            break;
                        }
                        else if (stream.Position >= stream.Length - 1 && type == graphtype.State && type == graphtype.Temperature)
                        {
                            for (int i = counter; i < gpoints; i++)
                            {
                                tempgraphdata.x[i] = (double)counter / gpoints;
                                tempgraphdata.y[i] = t;
                                tempgraphdata.date[i] = Functions.ReadDateTime(tbyt);
                            }
                            break;
                        }
                    }
                    while (extflag >= 0)
                    {
                        if (maxstream.Length <= 0)
                        {
                            tempgraphdata.max.Add(0);
                            tempgraphdata.min.Add(0);
                            break;
                        }
                        maxstream.Read(byt, 0, 4);
                        var tempmax = Functions.BytetoFloat(byt);
                        tempgraphdata.max.Add(tempmax);
                        minstream.Read(byt, 0, 4);
                        var tempmin = Functions.BytetoFloat(byt);
                        tempgraphdata.min.Add(tempmin);
                        float Percent = (float)maxstream.Position / (float)maxstream.Length;
                        if (Percent >= fend || Percent >= 1f)
                            extflag = -1;
                    }
                }
                return tempgraphdata;
            }

            public void ClearBuffer()
            {
                vars.buffer = new DataBuffer();
            }

            public List<double> ReadSingleBuffer()
            {
                List<double> sbuffer = new List<double>();
                stream.Position = 0;
                byte[] tempbuffer = new byte[4];
                while (true)
                {
                    if (stream.Position + 4 > stream.Length)
                        break;

                    stream.Read(tempbuffer, 0, 4);
                    int t = tempbuffer[3];
                    for (int i = 2; i >= 0; --i)
                        t = t * 256 + tempbuffer[i];
                    var t2 = Functions.formatFloat(t);
                    sbuffer.Add(t2);
                }

                return sbuffer;
            }

        }

        public class Logger
        {
            private FileStream Vx,Vy,Vz,Ax,Ay,Az,X,Y,Z,Altitude,Latitude,Longitude,PDOP,state,Temperature,UsedStats,VisibleStats,V,A;
            private FileStream VxMax, VxMin, VyMax, VyMin, VzMax, VzMin, AxMax, AxMin, AyMax, AyMin, AzMax, AzMin, XMax, XMin, YMax, YMin, ZMax, ZMin,VMax,VMin,AMax,AMin;
            private FileStream AltitudeMax, AltitudeMin, LatitudeMax, LatitudeMin, LongitudeMax, LongitudeMin, PDOPMax, PDOPMin,Time;

            public Logger(string DirPath)
            {
                System.IO.Directory.CreateDirectory(DirPath);
                V = new FileStream(DirPath + "V.glf", FileMode.Create, FileAccess.Write);
                VMax = new FileStream(DirPath + "VMax.glf", FileMode.Create, FileAccess.Write);
                VMin = new FileStream(DirPath + "VMin.glf", FileMode.Create, FileAccess.Write);
                A = new FileStream(DirPath + "A.glf", FileMode.Create, FileAccess.Write);
                AMax = new FileStream(DirPath + "AMax.glf", FileMode.Create, FileAccess.Write);
                AMin = new FileStream(DirPath + "AMin.glf", FileMode.Create, FileAccess.Write);
                Vx = new FileStream(DirPath + "Vx.glf", FileMode.Create, FileAccess.Write);
                VxMax = new FileStream(DirPath + "VxMax.glf", FileMode.Create, FileAccess.Write);
                VxMin = new FileStream(DirPath + "VxMin.glf", FileMode.Create, FileAccess.Write);
                Vy = new FileStream(DirPath + "Vy.glf", FileMode.Create, FileAccess.Write);
                VyMax = new FileStream(DirPath + "VyMax.glf", FileMode.Create, FileAccess.Write);
                VyMin = new FileStream(DirPath + "VyMin.glf", FileMode.Create, FileAccess.Write);
                Vz = new FileStream(DirPath + "Vz.glf", FileMode.Create, FileAccess.Write);
                VzMax = new FileStream(DirPath + "VzMax.glf", FileMode.Create, FileAccess.Write);
                VzMin = new FileStream(DirPath + "VzMin.glf", FileMode.Create, FileAccess.Write);
                Ax = new FileStream(DirPath + "Ax.glf", FileMode.Create, FileAccess.Write);
                AxMax = new FileStream(DirPath + "AxMax.glf", FileMode.Create, FileAccess.Write);
                AxMin = new FileStream(DirPath + "AxMin.glf", FileMode.Create, FileAccess.Write);
                Ay = new FileStream(DirPath + "Ay.glf", FileMode.Create, FileAccess.Write);
                AyMax = new FileStream(DirPath + "AyMax.glf", FileMode.Create, FileAccess.Write);
                AyMin = new FileStream(DirPath + "AyMin.glf", FileMode.Create, FileAccess.Write);
                Az = new FileStream(DirPath + "Az.glf", FileMode.Create, FileAccess.Write);
                AzMax = new FileStream(DirPath + "AzMax.glf", FileMode.Create, FileAccess.Write);
                AzMin = new FileStream(DirPath + "AzMin.glf", FileMode.Create, FileAccess.Write);
                X = new FileStream(DirPath + "X.glf", FileMode.Create, FileAccess.Write);
                XMax = new FileStream(DirPath + "XMax.glf", FileMode.Create, FileAccess.Write);
                XMin = new FileStream(DirPath + "XMin.glf", FileMode.Create, FileAccess.Write);
                Y = new FileStream(DirPath + "Y.glf", FileMode.Create, FileAccess.Write);
                YMax = new FileStream(DirPath + "YMax.glf", FileMode.Create, FileAccess.Write);
                YMin = new FileStream(DirPath + "YMin.glf", FileMode.Create, FileAccess.Write);
                Z = new FileStream(DirPath + "Z.glf", FileMode.Create, FileAccess.Write);
                ZMax = new FileStream(DirPath + "ZMax.glf", FileMode.Create, FileAccess.Write);
                ZMin = new FileStream(DirPath + "ZMin.glf", FileMode.Create, FileAccess.Write);
                Altitude = new FileStream(DirPath + "Altitude.glf", FileMode.Create, FileAccess.Write);
                AltitudeMax = new FileStream(DirPath + "AltitudeMax.glf", FileMode.Create, FileAccess.Write);
                AltitudeMin = new FileStream(DirPath + "AltitudeMin.glf", FileMode.Create, FileAccess.Write);
                Latitude = new FileStream(DirPath + "Latitude.glf", FileMode.Create, FileAccess.Write);
                LatitudeMax = new FileStream(DirPath + "LatitudeMax.glf", FileMode.Create, FileAccess.Write);
                LatitudeMin = new FileStream(DirPath + "LatitudeMin.glf", FileMode.Create, FileAccess.Write);
                Longitude = new FileStream(DirPath + "Longitude.glf", FileMode.Create, FileAccess.Write);
                LongitudeMax = new FileStream(DirPath + "LongitudeMax.glf", FileMode.Create, FileAccess.Write);
                LongitudeMin = new FileStream(DirPath + "LongitudeMin.glf", FileMode.Create, FileAccess.Write);
                PDOP = new FileStream(DirPath + "PDOP.glf", FileMode.Create, FileAccess.Write);
                PDOPMax = new FileStream(DirPath + "PDOPMax.glf", FileMode.Create, FileAccess.Write);
                PDOPMin = new FileStream(DirPath + "PDOPMin.glf", FileMode.Create, FileAccess.Write);
                state = new FileStream(DirPath + "state.glf", FileMode.Create, FileAccess.Write);
                Temperature = new FileStream(DirPath + "Temperature.glf", FileMode.Create, FileAccess.Write);
                UsedStats = new FileStream(DirPath + "UsedStats.glf", FileMode.Create, FileAccess.Write);
                VisibleStats = new FileStream(DirPath + "VisibleStats.glf", FileMode.Create, FileAccess.Write);
                Time = new FileStream(DirPath + "Time.glf", FileMode.Create, FileAccess.Write);
            }

            public void Writebuffer(SingleDataBuffer buffer)
            {
                if (buffer.state == 1.0)
                {
                    V.Write(buffer.BV, 0, 4);
                    A.Write(buffer.BA, 0, 4);
                    Vx.Write(buffer.BVx, 0, 4);
                    Vy.Write(buffer.BVy, 0, 4);
                    Vz.Write(buffer.BVz, 0, 4);
                    Ax.Write(buffer.BAx, 0, 4);
                    Ay.Write(buffer.BAy, 0, 4);
                    Az.Write(buffer.BAz, 0, 4);
                    X.Write(buffer.BX, 0, 4);
                    Y.Write(buffer.BY, 0, 4);
                    Z.Write(buffer.BZ, 0, 4);
                    Altitude.Write(buffer.BAltitude, 0, 4);
                    Latitude.Write(buffer.BLatitude, 0, 4);
                    Longitude.Write(buffer.BLongitude, 0, 4);
                    PDOP.Write(buffer.BPDOP, 0, 4);
                    state.WriteByte(buffer.Bstate);
                    Temperature.WriteByte(buffer.BTemperature);
                    UsedStats.Write(buffer.BNumOfUsedStats, 0, 4);
                    VisibleStats.Write(buffer.BNumOfVisibleStats, 0, 4);
                    Time.Write(buffer.Bdatetime, 0, 6);
                }
                if (buffer.WriteExtreme)
                {
                    buffer.WriteExtreme = false;
                    VxMax.Write(buffer.BVxMax, 0, 4);
                    VxMin.Write(buffer.BVxMin, 0, 4);
                    VyMax.Write(buffer.BVyMax, 0, 4);
                    VyMin.Write(buffer.BVyMin, 0, 4);
                    VzMax.Write(buffer.BVzMax, 0, 4);
                    VzMin.Write(buffer.BVzMin, 0, 4);
                    AxMax.Write(buffer.BAxMax, 0, 4);
                    AxMin.Write(buffer.BAxMin, 0, 4);
                    AyMax.Write(buffer.BAyMax, 0, 4);
                    AyMin.Write(buffer.BAyMin, 0, 4);
                    AzMax.Write(buffer.BAzMax, 0, 4);
                    AzMin.Write(buffer.BAzMin, 0, 4);
                    XMax.Write(buffer.BXMax, 0, 4);
                    XMin.Write(buffer.BXMin, 0, 4);
                    YMax.Write(buffer.BYMax, 0, 4);
                    YMin.Write(buffer.BYMin, 0, 4);
                    ZMax.Write(buffer.BZMax, 0, 4);
                    ZMin.Write(buffer.BZMin, 0, 4);
                    VMax.Write(buffer.BVMax, 0, 4);
                    VMin.Write(buffer.BVMin, 0, 4);
                    AMax.Write(buffer.BAMax, 0, 4);
                    AMin.Write(buffer.BAMin, 0, 4);
                    AltitudeMax.Write(buffer.BAltitudeMax, 0, 4);
                    AltitudeMin.Write(buffer.BAltitudeMin, 0, 4);
                    LatitudeMax.Write(buffer.BLatitudeMax, 0, 4);
                    LatitudeMin.Write(buffer.BLatitudeMin, 0, 4);
                    LongitudeMax.Write(buffer.BLongitudeMax, 0, 4);
                    LongitudeMin.Write(buffer.BLongitudeMin, 0, 4);
                    PDOPMax.Write(buffer.BPDOPMax, 0, 4);
                    PDOPMin.Write(buffer.BPDOPMin, 0, 4);
                }
            }

            public void WriteTimebuffer(SingleDataBuffer buffer)
            {
                Time.Write(buffer.Bdatetime, 0, 6);
            }

            public void CloseFiles()
            {
                Time.Close();
                Vx.Close();
                Vy.Close();
                Vz.Close();
                Ax.Close();
                Ay.Close();
                Az.Close();
                X.Close();
                Y.Close();
                Z.Close();
                V.Close();
                A.Close();
                Altitude.Close();
                Latitude.Close();
                Longitude.Close();
                PDOP.Close();
                state.Close();
                Temperature.Close();
                UsedStats.Close();
                VisibleStats.Close();
                VxMax.Close();
                VxMin.Close();
                VyMax.Close();
                VyMin.Close();
                VzMax.Close();
                VzMin.Close();
                AxMax.Close();
                AxMin.Close();
                AyMax.Close();
                AyMin.Close();
                AzMax.Close();
                AzMin.Close();
                XMax.Close();
                XMin.Close();
                YMax.Close();
                YMin.Close();
                ZMax.Close();
                ZMin.Close();
                VMax.Close();
                VMin.Close();
                AMax.Close();
                AMin.Close();
                AltitudeMax.Close();
                AltitudeMin.Close();
                LongitudeMin.Close();
                LongitudeMax.Close();
                LatitudeMin.Close();
                LatitudeMax.Close();
                PDOPMax.Close();
                PDOPMin.Close();
            }
        }


}
