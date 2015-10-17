
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
            public double[] Altitude;
            public double[] Latitude;
            public double[] Longitude;
            public double[] X;
            public double[] Y;
            public double[] Z;
            public double[] Vx;
            public double[] Vy;
            public double[] Vz;
            public double[] V;
            public double[] Ax;
            public double[] Ay;
            public double[] Az;
            public double[] A;
            public double[] PDOP;
            public double[] Altitude_Processed;
            public double[] Latitude_Processed;
            public double[] Longitude_Processed;
            public double[] X_Processed;
            public double[] Y_Processed;
            public double[] Z_Processed;
            public double[] Vx_Processed;
            public double[] Vy_Processed;
            public double[] Vz_Processed;
            public double[] V_Processed;
            public double[] Temperature;
            public double[] NumOfUsedSats;
            public double[] NumOfVisibleSats;
            public double[] state;
            public DateTime[] datetime;
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
            public int[] Temperature;
            public double[] TOW;
            public double[][] PseudoRanges;
            public int[][] Prn;
            public double[][] dopplers;
            public double[][] reliability;
            public double[][][] pAngle;
            public double[][][] snr;
            public CartesianCoordinate[][] satPos;
            public double[][] satPos_X;
            public int counter = 0;
            public int overLoad = 0;
        }

        public class AttitudeInformation
        {
            public double[] Azimuth;
            public double[] Elevation;
            public double[] X;
            public double[] Y;
            public double[] Z;
            public double[] Distance;
            public DateTime[] datetime;
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
