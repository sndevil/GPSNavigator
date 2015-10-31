
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using GPSNavigator.Source;


namespace GPSNavigator.Classes
{
     //const int Max_buf = 100000;
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
            public double Latitude;
            public double Longitude;
            public double X;
            public double Y;
            public double Z;
            public double Vx;
            public double Vy;
            public double Vz;
            public double V;
            public double Ax;
            public double Ay;
            public double Az;
            public double A;
            public double PDOP;
            public double Altitude_Processed;
            public double Latitude_Processed;
            public double Longitude_Processed;
            public double X_Processed;
            public double Y_Processed;
            public double Z_Processed;
            public double Vx_Processed;
            public double Vy_Processed;
            public double Vz_Processed;
            public double V_Processed;
            public double Temperature;
            public double NumOfUsedSats;
            public double NumOfVisibleSats;
            public double state;
            public DateTime datetime;
            public int statcounter;
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


        public class LogFileManager
        {
            private int Databuffercount = 1000; // 500 packets would be loaded to RAM
            public long start = 0;
            public long end;
            public int position = 0;
            public float zoom = 1f,delta;

            public string filepath;

            private FileStream stream;
            private Globals vars;

            public LogFileManager(string path,ref Globals variables)
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                vars = variables;
                end = stream.Length;
                delta = (float)((end - start) / Databuffercount);
            }

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

                        if (stream.Position + msgSize - 2 > stream.Length || counter++ >= Databuffercount)
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

            public void ClearBuffer()
            {
                vars.buffer = new DataBuffer();
            }



        }

        public class Logger
        {
            private FileStream Vx,Vy,Vz,Ax,Ay,Az,X,Y,Z,Altitude,Latitude,Longitude,PDOP,state,Temperature,UsedStats,VisibleStats;
            
            public Logger(string DirPath)
            {
                System.IO.Directory.CreateDirectory(DirPath);
                Vx = new FileStream(DirPath + "Vx.glf", FileMode.Create, FileAccess.Write); //DirPath + ""
                Vy = new FileStream(DirPath + "Vy.glf", FileMode.Create, FileAccess.Write);
                Vz = new FileStream(DirPath + "Vz.glf", FileMode.Create, FileAccess.Write);
                Ax = new FileStream(DirPath + "Ax.glf", FileMode.Create, FileAccess.Write);
                Ay = new FileStream(DirPath + "Ay.glf", FileMode.Create, FileAccess.Write);
                Az = new FileStream(DirPath + "Az.glf", FileMode.Create, FileAccess.Write);
                X = new FileStream(DirPath + "X.glf", FileMode.Create, FileAccess.Write);
                Y = new FileStream(DirPath + "Y.glf", FileMode.Create, FileAccess.Write);
                Z = new FileStream(DirPath + "Z.glf", FileMode.Create, FileAccess.Write);
                Altitude = new FileStream(DirPath + "Altitude.glf", FileMode.Create, FileAccess.Write);
                Latitude = new FileStream(DirPath + "Latitude.glf", FileMode.Create, FileAccess.Write);
                Longitude = new FileStream(DirPath + "Longitude.glf", FileMode.Create, FileAccess.Write);
                PDOP = new FileStream(DirPath + "PDOP.glf", FileMode.Create, FileAccess.Write);
                state = new FileStream(DirPath + "state.glf", FileMode.Create, FileAccess.Write);
                Temperature = new FileStream(DirPath + "Temperature.glf", FileMode.Create, FileAccess.Write);
                UsedStats = new FileStream(DirPath + "UsedStats.glf", FileMode.Create, FileAccess.Write);
                VisibleStats = new FileStream(DirPath + "VisibleStats.glf", FileMode.Create, FileAccess.Write);
            }

            public void Writebuffer(SingleDataBuffer buffer)
            {
                if (buffer.state == 1.0)
                {
                    Vx.Write(Encoding.UTF8.GetBytes(buffer.Vx.ToString()+"\r\n"), 0, buffer.Vx.ToString().Length+2);
                    Vy.Write(Encoding.UTF8.GetBytes(buffer.Vy.ToString()+"\r\n"), 0, buffer.Vy.ToString().Length+2);
                    Vz.Write(Encoding.UTF8.GetBytes(buffer.Vz.ToString()+"\r\n"), 0, buffer.Vz.ToString().Length+2);
                    Ax.Write(Encoding.UTF8.GetBytes(buffer.Ax.ToString()+"\r\n"), 0, buffer.Ax.ToString().Length+2);
                    Ay.Write(Encoding.UTF8.GetBytes(buffer.Ay.ToString()+"\r\n"), 0, buffer.Ay.ToString().Length+2);
                    Az.Write(Encoding.UTF8.GetBytes(buffer.Az.ToString()+"\r\n"), 0, buffer.Az.ToString().Length+2);
                    X.Write(Encoding.UTF8.GetBytes(buffer.X.ToString() + "\r\n"), 0, buffer.X.ToString().Length + 2);
                    Y.Write(Encoding.UTF8.GetBytes(buffer.Y.ToString() + "\r\n"), 0, buffer.Y.ToString().Length + 2);
                    Z.Write(Encoding.UTF8.GetBytes(buffer.Z.ToString() + "\r\n"), 0, buffer.Z.ToString().Length + 2);
                    Altitude.Write(Encoding.UTF8.GetBytes(buffer.Altitude.ToString() + "\r\n"), 0, buffer.Altitude.ToString().Length + 2);
                    Latitude.Write(Encoding.UTF8.GetBytes(buffer.Latitude.ToString() + "\r\n"), 0, buffer.Latitude.ToString().Length + 2);
                    Longitude.Write(Encoding.UTF8.GetBytes(buffer.Longitude.ToString() + "\r\n"), 0, buffer.Longitude.ToString().Length + 2);
                    PDOP.Write(Encoding.UTF8.GetBytes(buffer.PDOP.ToString() + "\r\n"), 0, buffer.PDOP.ToString().Length + 2);
                    state.Write(Encoding.UTF8.GetBytes(buffer.state.ToString() + "\r\n"), 0, buffer.state.ToString().Length + 2);
                    Temperature.Write(Encoding.UTF8.GetBytes(buffer.Temperature.ToString() + "\r\n"), 0, buffer.Temperature.ToString().Length + 2);
                    UsedStats.Write(Encoding.UTF8.GetBytes(buffer.NumOfUsedSats.ToString() + "\r\n"), 0, buffer.NumOfUsedSats.ToString().Length + 2);
                    VisibleStats.Write(Encoding.UTF8.GetBytes(buffer.NumOfVisibleSats.ToString() + "\r\n"), 0, buffer.NumOfVisibleSats.ToString().Length + 2);
                }
                else
                {
                    Vx.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Vy.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Vz.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Ax.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Ay.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Az.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    X.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Y.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Z.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Altitude.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Latitude.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Longitude.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    PDOP.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    state.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    Temperature.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    UsedStats.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                    VisibleStats.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 1);
                }
            }

            public void CloseFiles()
            {
                Vx.Close();
                Vy.Close();
                Vz.Close();
                Ax.Close();
                Ay.Close();
                Az.Close();
                X.Close();
                Y.Close();
                Z.Close();
                Altitude.Close();
                Latitude.Close();
                Longitude.Close();
                PDOP.Close();
                state.Close();
                Temperature.Close();
                UsedStats.Close();
                VisibleStats.Close();
            }



        }


}
