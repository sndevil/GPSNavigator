
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

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

        public class BinaryRawDataBuffer
        {
            public List<int> Temperature;

            public List<double> TOW;
            public List<double[]> PseudoRanges;
            public List<int[]> Prn;
            public List<double[]> dopplers;
            public List<double[]> reliability;
            public List<double[][]> pAngle;
            public List<double[][]> snr;
            public List<CartesianCoordinate[]> satPos;
            public List<double[]> satPos_X;
            public int counter = 0;
            public int overLoad = 0;
        }

        public class AttitudeInformation
        {
            public List<double> Azimuth;
            public List<double> Elevation;
            public List<double> X;
            public List<double> Y;
            public List<double> Z;
            public List<double> Distance;
            public List<DateTime> datetime;
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


}
