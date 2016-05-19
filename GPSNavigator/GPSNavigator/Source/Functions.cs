using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPSNavigator.Classes;
using GPSNavigator;

namespace GPSNavigator.Source
{

    public static class Functions
    {

        static SingleDataBuffer dbuffer = new SingleDataBuffer();

        #region Constants
        public const int nTRACKING_MODULES = 12;
        public const double C = 299792458.0;

        //Commands
        public const string MSG_Header = "$CMD";
        public const char START_CMD = (char)0;
        public const char SERIAL_SETTING_CMD = (char)1;
        public const char REFRESH_RATE_CMD = (char)2;
        public const char DEASSIGN_SATELLITES = (char)3;
        public const char SEARCH_TYPE_CMD = (char)4;
        public const char SAT_NUMBER_CMD = (char)5;
        public const char PDOP_THRESHOLD_CMD = (char)6;
        public const char SAT_DISTANCE_ERROR_THRESHOLD_CMD = (char)7;
        public const char GPS_DEASSIGN_THRESHOLD_CMD = (char)8;
        public const char GLONASS_DEASSIGN_THRESHOLD_CMD = (char)9;
        public const char RELIABILITY_DEASSIGN_THRESHOLD_CMD = (char)10;
        public const char GPS_USE_THRESHOLD_CMD = (char)11;
        public const char GLONASS_USE_THRESHOLD_CMD = (char)12;
        public const char TROPOSPHORIC_CORRECTION_CMD = (char)13;
        public const char MAX_SPEED_CMD = (char)14;
        public const char MAX_ACCELERATION_CMD = (char)15;
        public const char GREEN_SAT_TYP_CMD = (char)16;
        public const char SAVE_SETTING_CMD = (char)17;
        public const char READ_SETTING_CMD = (char)18;
        public const char MASK_ANGLE_CMD = (char)19;
        public const char IONOSPHORIC_CORRECTION_CMD = (char)20;
        public const char AUTO_MAX_ANGLE_ATTITUDE_CMD = (char)21;
        public const char POSITIONING_TYPE_CMD = (char)22;
        public const char READ_LICENCE_CMD = (char)23;
        public const char ADD_LICENCE_CMD = (char)24;
        public const char REMOVE_LICENCE_CMD = (char)25;
        public const char ATTITUDE_INFO_CMD = (char)26;
        public const char BASE_STATION_SET_TIME_CMD = (char)27;
        public const char BASE_STATION_SET_POS_CMD = (char)28;
        public const char BASE_STATION_GET_STATUS_CMD = (char)29;
        public const char BASE_STATION_RESET_LTR_CMD = (char)30;
        public const char AUTOMATIC_SEARCH_ENABLE_CMD = (char)31;
        public const char AUTOMATIC_SEARCH_DIABLE_CMD = (char)32;
        public const char BASE_STATION_CHANGE_NUMBER_CMD = (char)33;
        public const char CHANGE_BASE_STATION_POSITIONING_MODE_CMD = (char)34;
        public const char SET_RANGE_OFFSET_CMD = (char)35;

        public const char GET_STATUS_BASED_ON_MAC_CMD = (char)36;
        public const char SET_LTR_TX_POWER_LEVEL_CMD = (char)37;
        public const char TURN_ON_OFF_LTR_CMD = (char)38;

        public const int BIN_FULL_MSG_SIZE = 152;
        public const int BIN_FULL_PLUS_MSG_SIZE = 164;
        public const int BIN_COMPACT_MSG_SIZE = 80;
        public const int BIN_GPS_SUPPLEMENT_MSG_SIZE = 56;
        public const int BIN_DEBUG_MSG_SIZE = 100;
        public const int BIN_GLONASS_SUPPLEMENT_MSG_SIZE = 56;
        public const int BIN_RAW_DATA_MSG_SIZE = 504 + 192;
        public const int BIN_LICENCE_MSG_SIZE = 22;
        public const int BIN_SETTING_MSG_SIZE = 41;
        public const int BIN_ATTITUDE_INFO_MSG_SIZE = 40;
        public const int BIN_DUAL_CHANNEL_MSG_SIZE = 164;
        public const int BIN_BASESTATION_INFO_MSG_SIZE = 60; //49
        public const int BIN_KEEP_ALIVE_MSG_SIZE = 12;
        public const int BIN_ACK_SIGNAL_MSG_SIZE = 13;
        public const int BIN_COMPACT_DUAL_CHANNEL_MSG_SIZE = 88;


        public const int BIN_FULL = 0;
        public const int BIN_FULL_PLUS = 1;
        public const int BIN_COMPACT = 0x14;
        public const int BIN_GPS_SUPPLEMENT = 2;
        public const int BIN_DEBUG = 3;
        public const int BIN_GLONASS_SUPPLEMENT = 4;
        public const int BIN_RAW_DATA = 5;
        public const int BIN_LICENCE = READ_LICENCE_CMD;
        public const int BIN_SETTING = READ_SETTING_CMD;
        public const int BIN_ATTITUDE_INFO = 6;
        public const int BIN_DUAL_CHANNEL = 7;
        public const int BIN_BASESTATION_INFO = 8;
        public const int BIN_KEEP_ALIVE = 9;
        public const int BIN_ACK_SIGNAL = 10;

        public const double Elevation_OutOfRange = 91;
        public const double Azimuth_OutOfRange = 361;

        public const int TOTAL_BASE_STATIONS_COUNT = 256;
        public const int TOTAL_FLYING_OBJECTS_COUNT = 16;
        public const int MAX_CONNECTION_ACOUNT = 30;//30;


        #region Programmer_Constants
        public const int FLASH_SECTOR_SIZE = 4096;
        public const int FLASH_MEMORY_nSECTORS = 2048;
        public const int INTEL_HEX_MAX_DATA_LENGTH = 64;

        public const char INTEL_HEX_START_CODE =':';

        public const int INTEL_HEX_MIN_RECORD_LENGTH = 11;

        public const int INTEL_HEX_START_CODE_OFFSET = 0;
        public const int INTEL_HEX_START_CODE_LENGTH = 1;
        public const int INTEL_HEX_BYTE_COUNT_LENGTH = 2;
        public const int INTEL_HEX_ADDRESS_LENGTH = 4;
        public const int INTEL_HEX_RECORD_TYPE_LENGTH = 2;
        public const int INTEL_HEX_CHECKSUM_LENGTH = 2;
        public const int INTEL_HEX_BYTE_COUNT_OFFSET = (INTEL_HEX_START_CODE_OFFSET+INTEL_HEX_START_CODE_LENGTH);
        public const int INTEL_HEX_ADDRESS_OFFSET = (INTEL_HEX_BYTE_COUNT_OFFSET+INTEL_HEX_BYTE_COUNT_LENGTH);
        public const int INTEL_HEX_RECORD_TYPE_OFFSET = (INTEL_HEX_ADDRESS_OFFSET + INTEL_HEX_ADDRESS_LENGTH);
        public const int INTEL_HEX_DATA_OFFSET	= (INTEL_HEX_RECORD_TYPE_OFFSET+INTEL_HEX_RECORD_TYPE_LENGTH);

        public const char REMOTE_FLASH_PACKET_HEADER0 = (char)0xAA;
        public const char REMOTE_FLASH_PACKET_HEADER1=(char)0x55;
        public const char REMOTE_FLASH_PACKET_HEADER2 = (char)0x55;
        public const char REMOTE_FLASH_PACKET_HEADER3 = (char)0xAA;
        public const int REMOTE_FLASH_PACKET_HEADER_SIZE = 4;
        public const int REMOTE_FLASH_RETRY_COUNT = 5;
        public const int REMOTE_FLASH_FULL_ERASE_TIMOUT_RETRIES = 40;

        public const char REMOTE_FLASH_PROGRAMMING_MODE_COMMAND = (char)27;



        #endregion


        #endregion

        public static double[][] MatrixMult(double[][] matrixA, int aRows, int aCols, double[][] matrixB, int bCols)
        {
            double[][] result = MatrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k)
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            return result;
        }

        public static double[][] MatrixCreate(int rows, int cols)
        {
            // creates a matrix initialized to all 0.0s  
            // do error checking here?  
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            // auto init to 0.0  
            return result;
        }

        public static double[][] matrix_invert_4X4_FAST(double[][] mat)
        {
            double[][] result = MatrixCreate(4, 4);
            double a = mat[0][0], b = mat[0][1], c = mat[0][2], d = mat[0][3],
                   e = mat[1][0], f = mat[1][1], g = mat[1][2], h = mat[1][3],
                   j = mat[2][0], k = mat[2][1], l = mat[2][2], m = mat[2][3],
                   n = mat[3][0], o = mat[3][1], p = mat[3][2], q = mat[3][3];

            double det = (a * f * l * q + a * g * m * o + a * h * k * p + b * e * m * p + b * g * j * q + b * h * l * n + c * e * k * q + c * f * m * n + c * h * j * o + d * e * l * o + d * f * j * p + d * g * k * n) - (a * f * m * p + a * g * k * q + a * h * l * o + b * e * l * q + b * g * m * n + b * h * j * p + c * e * m * o + c * f * j * q + c * h * k * n + d * e * k * p + d * f * l * n + d * g * j * o);
            double deti = 1 / det;

            result[0][0] = f * l * q - f * m * p - g * k * q + g * m * o + h * k * p - h * l * o; result[0][1] = b * m * p - b * l * q + c * k * q - c * m * o - d * k * p + d * l * o; result[0][2] = b * g * q - b * h * p - c * f * q + c * h * o + d * f * p - d * g * o; result[0][3] = b * h * l - b * g * m + c * f * m - c * h * k - d * f * l + d * g * k;
            result[1][0] = e * m * p - e * l * q + g * j * q - g * m * n - h * j * p + h * l * n; result[1][1] = a * l * q - a * m * p - c * j * q + c * m * n + d * j * p - d * l * n; result[1][2] = a * h * p - a * g * q + c * e * q - c * h * n - d * e * p + d * g * n; result[1][3] = a * g * m - a * h * l - c * e * m + c * h * j + d * e * l - d * g * j;
            result[2][0] = e * k * q - e * m * o - f * j * q + f * m * n + h * j * o - h * k * n; result[2][1] = a * m * o - a * k * q + b * j * q - b * m * n - d * j * o + d * k * n; result[2][2] = a * f * q - a * h * o - b * e * q + b * h * n + d * e * o - d * f * n; result[2][3] = a * h * k - a * f * m + b * e * m - b * h * j - d * e * k + d * f * j;
            result[3][0] = e * l * o - e * k * p + f * j * p - f * l * n - g * j * o + g * k * n; result[3][1] = a * k * p - a * l * o - b * j * p + b * l * n + c * j * o - c * k * n; result[3][2] = a * g * o - a * f * p + b * e * p - b * g * n - c * e * o + c * f * n; result[3][3] = a * f * l - a * g * k - b * e * l + b * g * j + c * e * k - c * f * j;

            result[0][0] *= deti; result[0][1] *= deti; result[0][2] *= deti; result[0][3] *= deti;
            result[1][0] *= deti; result[1][1] *= deti; result[1][2] *= deti; result[1][3] *= deti;
            result[2][0] *= deti; result[2][1] *= deti; result[2][2] *= deti; result[2][3] *= deti;
            result[3][0] *= deti; result[3][1] *= deti; result[3][2] *= deti; result[3][3] *= deti;

            return result;
        }

        public static double distance(CartesianCoordinate p1, CartesianCoordinate p2)
        {
            return Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2) + Math.Pow(p1.z - p2.z, 2));
        }

        public static CartesianCoordinate leastSquare(CartesianCoordinate[] satpos, double[] pseudoRanges, double[] dopplerFrequency, double[] reliability, int[] commonSatsInx, int commonSatNum)
        {
            double[] res = new double[4];
            double[][] A = MatrixCreate(nTRACKING_MODULES, 4);
            double[][] TA = MatrixCreate(4, nTRACKING_MODULES);
            double[][] RA = MatrixCreate(4, 4);
            double[][] RA2 = MatrixCreate(4, nTRACKING_MODULES);
            double[] omc = new double[nTRACKING_MODULES];
            double[] obs = new double[nTRACKING_MODULES];
            int nIterations;
            double travelTime;
            double[] satelliteReliability = new double[nTRACKING_MODULES];

            CartesianCoordinate rotatedSatellitePosition = new CartesianCoordinate();
            CartesianCoordinate calculatedPosition = new CartesianCoordinate();
            CartesianCoordinate temp = new CartesianCoordinate();
            double tempLenght;
            double timeOffset;

            nIterations = 7;

            timeOffset = 0;

            for (int i = 0; i < commonSatNum; i++)
            {
                obs[i] = pseudoRanges[commonSatsInx[i]];
                satelliteReliability[i] = reliability[commonSatsInx[i]];
            }
            double reliabilityNormalizationCoefficient = 1;
            int nSatellites = 0;
            for (int i = 0; i < commonSatNum; i++)
            {
                reliabilityNormalizationCoefficient *= satelliteReliability[i];
                nSatellites++;
            }

            reliabilityNormalizationCoefficient = Math.Pow(reliabilityNormalizationCoefficient, 1.0 / nSatellites);
            for (int i = 0; i < commonSatNum; i++)
                satelliteReliability[i] /= reliabilityNormalizationCoefficient;

            for (int iter = 1; iter <= nIterations; iter++)
            {
                for (int i = 0; i < commonSatNum; i++)
                {
                    if (iter == 1)
                        rotatedSatellitePosition = satpos[commonSatsInx[i]];
                    else
                    {
                        temp.x = satpos[commonSatsInx[i]].x - calculatedPosition.x;
                        temp.y = satpos[commonSatsInx[i]].y - calculatedPosition.y;
                        temp.z = satpos[commonSatsInx[i]].z - calculatedPosition.z;
                        tempLenght = distance(satpos[commonSatsInx[i]], calculatedPosition);
                        travelTime = tempLenght / C;

                        //rotatedSatellitePosition = earthRotate(satellites[k]->getPosition(), travelTime);
                        rotatedSatellitePosition = satpos[commonSatsInx[i]];
                    }

                    temp.x = rotatedSatellitePosition.x - calculatedPosition.x;
                    temp.y = rotatedSatellitePosition.y - calculatedPosition.y;
                    temp.z = rotatedSatellitePosition.z - calculatedPosition.z;
                    tempLenght = distance(rotatedSatellitePosition, calculatedPosition);

                    omc[i] = obs[i] - tempLenght - timeOffset;

                    A[i][0] = -temp.x / obs[i];
                    A[i][1] = -temp.y / obs[i];
                    A[i][2] = -temp.z / obs[i];
                    A[i][3] = 1;
                }

                //x = A \ omc; 		==>  	 x = inv(A'*A)*A'*omc

                //A'
                for (int i = 0; i < nTRACKING_MODULES; i++)
                    for (int j = 0; j < 4; j++)
                        TA[j][i] = A[i][j] * satelliteReliability[i];

                //A'*A
                RA = MatrixMult(TA, 4, nTRACKING_MODULES, A, 4);

                //inv(A'*A)
                RA = matrix_invert_4X4_FAST(RA);

                //inv(A'*A)*A'
                RA2 = MatrixMult(RA, 4, 4, TA, nTRACKING_MODULES);

                //res = A \ omc; 		==>  	 x = inv(A'*A)*A'*omc
                for (int i = 0; i < 4; i++)
                {
                    res[i] = 0;
                    for (int k = 0; k < commonSatNum; k++)
                        res[i] = res[i] + RA2[i][k] * omc[k];
                }

                calculatedPosition.x += res[0];
                calculatedPosition.y += res[1];
                calculatedPosition.z += res[2];
                timeOffset += res[3];
            }

            return calculatedPosition;
        }

        public static GEOpoint Calculate_LatLongAlt_From_XYZ(double x, double y, double z)
        {
            GEOpoint result = new GEOpoint();

            result.Longitude = Math.Atan2(y, x);
            double ex2 = (2 - (1 / 298.257223563)) * (1 / 298.257223563) / ((1 - (1 / 298.257223563)) * (1 - (1 / 298.257223563)));
            double c = 6378137 * Math.Sqrt(1 + ex2);
            result.Latitude = Math.Atan(z / ((Math.Sqrt(x * x + y * y) * (1 - (2 - (1 / 298.257223563))) * (1 / 298.257223563))));

            result.Altitude = 0.1;
            double oldh = 0, N;
            int iterations = 0;
            while (Math.Abs(result.Altitude - oldh) > 1e-12)
            {
                oldh = result.Altitude;
                N = c / Math.Sqrt(1 + ex2 * Math.Pow(Math.Cos(result.Latitude), 2));
                result.Latitude = Math.Atan(z / ((Math.Sqrt(x * x + y * y) * (1 - (2 - (1 / 298.257223563)) * (1 / 298.257223563) * N / (N + result.Altitude)))));
                result.Altitude = Math.Sqrt(x * x + y * y) / Math.Cos(result.Latitude) - N;

                iterations = iterations + 1;
                if (iterations > 100)
                    break;
            }

            result.Latitude = result.Latitude * (180 / Math.PI);
            result.Longitude = result.Longitude * (180 / Math.PI);

            return result;
        }

        public static XYZpoint Calculate_XYZ_From_LatLongAlt(double latitude, double longitude, double altitude)
        {
            XYZpoint result = new XYZpoint();

            double b = latitude;
            b = b * Math.PI / 180;
            double l = longitude;
            l = l * Math.PI / 180;

            double ex2 = ((2 - 1 / 298.257223563) / 298.257223563) / Math.Pow((1 - 1 / 298.257223563), 2);
            double c = 6378137 * Math.Sqrt(1 + ex2);
            double N = c / Math.Sqrt(1 + ex2 * Math.Pow(Math.Cos(b), 2));

            result.x = (N + altitude) * Math.Cos(b) * Math.Cos(l);
            result.y = (N + altitude) * Math.Cos(b) * Math.Sin(l);
            result.z = (Math.Pow((1 - 1 / 298.257223563), 2) * N + altitude) * Math.Sin(b);

            return result;
        }

        private static int calcrc(byte[] data, int count)
        {
            int crc = 0, i;

            for (int j = 1; j <= count; j++)
            {
                crc = crc ^ (int)data[j] << 8;

                i = 8;
                do
                {
                    if ((crc & 0x8000) != 0)
                        crc = crc << 1 ^ 0x1021;
                    else
                        crc = crc << 1;
                } while (--i != 0);
            }
            return (crc);
        }

        public static double formatFloat(Int64 a)
        {
            double m, e, s;

            m = a & 0x7FFFFF;
            m = 1 + m * Math.Pow(2, -23.0);
            a >>= 23;
            e = a & 0xFF;
            a >>= 8;
            s = (a == 0 ? 1 : -1);

            return s * m * Math.Pow(2, e - 127);
        }

        public static Int64 unformatFloat(double a)
        {
            int sign = (a>=0) ? 0 : 1;
            int p = (int)Math.Floor(Math.Log(a,2));
            Int64 result = sign * 256 + p + 127;
            double m = (sign == 0) ? (a / Math.Pow(2, p)) - 1 : (-a / Math.Pow(2, p)) -1;
            m *= Math.Pow(2, 23);
            result <<= 23;
            result += (int)m;
            return result;
        }

        public static double BytetoFloat(byte[] array)
        {
            int t = array[3];
            for (int i = 2; i >= 0; --i)
                t = t * 256 + array[i];
            var t2 = (t == -524288) ? double.NaN : Functions.formatFloat(t);
            return t2;
        }

        public static double BytetoFloatOther(byte[] array)
        {
            double t = (double)array[3];
            for (int i = 2; i >= 0; --i)
                t = t * 256 + array[i];
            return t;
        }

        public static double formatDouble(Int64 a)
        {
            double m, e, s;

            m = a & 0xFFFFFFFFFFFFF;
            m = 1 + m * Math.Pow(2, -52.0);
            a >>= 1;
            a &= 0x7FFFFFFFFFFFFFFF;
            a >>= 51;
            e = a & 0x07FF;
            a >>= 11;
            s = (a == 0 ? 1 : -1);

            return s * m * Math.Pow(2, e - 1023);
        }

        private static double Decompress_DOP(double DOP)
        {
            if (DOP < 100)
                return DOP / 20;
            if (DOP < 150)
                return (DOP - 80) / 4;
            if (DOP < 200)
                return DOP - 132.5;
            return (DOP - 197.625) * 20;
        }

        private static int Calculate_Checksum(string Data)
        {
            int Checksum = 0;

            for (int i = 1; i < Data.IndexOf('*'); i++)
                Checksum ^= (int)Data[i];

            return Checksum;
        }

        public static char Calculate_Checksum_Char(char[] Data, int num)
        {
            char Checksum = (char)0;

            for (int i = 1; i < num; i++)
                Checksum ^= Data[i];

            return Checksum;
        }

        private static bool Checksum_validity(string Data)
        {
            string Received_Checksum = Data.Substring(Data.IndexOf('*') + 1, 2);

            try
            {
                if (Calculate_Checksum(Data) != Convert.ToInt32(Received_Checksum, 16))      //hex to int
                    return false;
            }
            catch
            {
                return false;
            }

            if (Data.IndexOf('*') < Data.LastIndexOf(','))      //'*' must be after the last ','
                return false;

            return true;
        }

        public static List<string> Calculate_NMEA_fields(string Data)
        {
            List<string> field = new List<string>();
            int field_Num = Data.Count(f => f == ',') + 2;       //Number of fields from begining to checksum

            int pos = 0;
            for (int i = 0; i < field_Num - 2; i++)
            {
                field.Add(Data.Substring(pos, Data.IndexOf(',', pos) - pos));
                pos = Data.IndexOf(',', pos) + 1;
            }
            field.Add(Data.Substring(Data.LastIndexOf(',') + 1, Data.IndexOf('*') - Data.LastIndexOf(',') - 1));
            field.Add(Data.Substring(Data.Length - 3, 2));              //Checksum
            return field;
        }

        public static int checkMsgSize(int msgType)
        {
            int msgSize;

            if (msgType == BIN_FULL)
                msgSize = BIN_FULL_MSG_SIZE;
            else if (msgType == BIN_FULL_PLUS)
                msgSize = BIN_FULL_PLUS_MSG_SIZE;
            else if (msgType == BIN_COMPACT)
                msgSize = BIN_COMPACT_MSG_SIZE;
            else if (msgType == BIN_GPS_SUPPLEMENT)
                msgSize = BIN_GPS_SUPPLEMENT_MSG_SIZE;
            else if (msgType == BIN_DEBUG)
                msgSize = BIN_DEBUG_MSG_SIZE;
            else if (msgType == BIN_GLONASS_SUPPLEMENT)
                msgSize = BIN_GLONASS_SUPPLEMENT_MSG_SIZE;
            else if (msgType == BIN_RAW_DATA)
                msgSize = BIN_RAW_DATA_MSG_SIZE;
            else if (msgType == BIN_LICENCE)
                msgSize = BIN_LICENCE_MSG_SIZE;
            else if (msgType == BIN_SETTING)
                msgSize = BIN_SETTING_MSG_SIZE;
            else if (msgType == BIN_ATTITUDE_INFO)
                msgSize = BIN_ATTITUDE_INFO_MSG_SIZE;
            else if (msgType == BIN_DUAL_CHANNEL)
                msgSize = BIN_DUAL_CHANNEL_MSG_SIZE;
            else if (msgType == BIN_BASESTATION_INFO)
                msgSize = BIN_BASESTATION_INFO_MSG_SIZE;
            else if (msgType == BIN_KEEP_ALIVE)
                msgSize = BIN_KEEP_ALIVE_MSG_SIZE;
            else if (msgType == BIN_ACK_SIGNAL)
                msgSize = BIN_ACK_SIGNAL_MSG_SIZE;
            else
                msgSize = -1;           //packet not valid

            return msgSize;
        }

        public static SingleDataBuffer Process_Binary_Message_Full(byte[] data, int SerialNum,ref List<Satellite[]> GPS,ref List<Satellite[]> GLONASS, ref DateTime PTime)
        {
            SingleDataBuffer buffer = new SingleDataBuffer();
            byte[] NaNBytes = {0,0,248,255};
            buffer.BGPSstat.Add(new byte[12]);
            buffer.BGLONASSstat.Add(new byte[12]);
            buffer.ChannelCounter = 1;
            buffer.BSatStats[16] = (byte)1;
            var tempsat = new List<Satellite[]>();
            if (GPS.Count < 1 || GLONASS.Count < 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    tempsat.Add(new Satellite[32]);
                    for (int j = 0; j < 32; j++)
                    {
                        tempsat[i][j] = new Satellite();
                    }
                    if (i % 2 == 0 && GPS.Count < 1)
                        GPS.Add(tempsat[i]);
                    else if (GLONASS.Count < 1)
                        GLONASS.Add(tempsat[i]);
                }
            }

            int msgSize = 0;

            if (data[1] == BIN_FULL)
                msgSize = BIN_FULL_MSG_SIZE;
            else if (data[1] == BIN_FULL_PLUS)
                msgSize = BIN_FULL_PLUS_MSG_SIZE;

            int checksum = calcrc(data, msgSize - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[msgSize - 3] || checksum1 != data[msgSize - 2])
            {
            //Show_Error("CheckSum Error", "Recieved checksum is incorrect");
               throw new Exception("Checksum Error");
               //return new SingleDataBuffer();
            }

            int index = 1;      //header
            index++;    //messageType

            // State
            int state = data[index];
            buffer.state = state;
            buffer.Bstate = data[index];
            index++;

            // Temperature
            buffer.Temperature = data[index];
            buffer.BTemperature = data[index];
            index++;

            //Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            buffer.Bdatetime[5] = data[index + 3];
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.Bdatetime[2 + i] = data[index + i]; }
            double TOW = a;     //millisecond
            index += 4;

            buffer.NumOfVisibleSats = 0;
            buffer.BSatStats[3] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[i] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[0][i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GPS[0][i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            
            index += 4;

            buffer.NumOfUsedSats = 0;
            buffer.BSatStats[7] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[i + 4] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[0][i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;

            buffer.BSatStats[11] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[8 + i] = data[index + i]; }
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[0][i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GLONASS[0][i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            buffer.BSatStats[15] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[12 + i] = data[index + i]; }
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[0][i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;

            var temp = buffer.NumOfVisibleSats;
            for (int i = 0; i <= 3; i++)
            {
                buffer.BNumOfVisibleStats[i] = (byte)(temp % 256);
                temp /= 256;
            }
            temp = buffer.NumOfUsedSats;
            for (int i = 0; i <= 3; i++)
            {
                buffer.BNumOfUsedStats[i] = (byte)(temp % 256);
                temp /= 256;
            }

            // X
            if (state == 1)
            {
                buffer.BX[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BX[i] = data[index + i]; };
                buffer.X = formatFloat(a);
            }
            else
            {
                buffer.X = double.NaN;
                buffer.BX = NaNBytes;
            }
            //dbuf.X[dbuf.counter] = formatFloat(a);
            index += 4;

            // X Processed
            if (state == 1)
            {
                buffer.BX_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BX_Processed[i] = data[index + i]; };
                buffer.X_Processed = formatFloat(a);
            }
            else
            {
                buffer.X_Processed = double.NaN;
                buffer.BX_Processed = NaNBytes;
            }

            index += 4;

            // Y
            if (state == 1)
            {
                buffer.BY[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BY[i] = data[index + i]; };
                buffer.Y = formatFloat(a);
            }
            else
            {
                buffer.Y = double.NaN;
                buffer.BY = NaNBytes;
            }

            index += 4;

            // Y Processed
            if (state == 1)
            {
                buffer.BY_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BY_Processed[i] = data[index + i]; };
                buffer.Y_Processed = formatFloat(a);
            }
            else
            {
                buffer.Y_Processed = double.NaN;
                buffer.BY_Processed = NaNBytes; 
            }
            index += 4;

            // Z
            if (state == 1)
            {
                buffer.BZ[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BZ[i] = data[index + i]; };
                buffer.Z = formatFloat(a);
            }
            else
            {
                buffer.Z = double.NaN;
                buffer.BZ = NaNBytes;
            }
            index += 4;

            // Z Processed
            if (state == 1)
            {
                buffer.BZ_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BZ_Processed[i] = data[index + i]; };
                buffer.Z_Processed = formatFloat(a);
            }
            else
            {
                buffer.Z_Processed = double.NaN;
                buffer.BZ_Processed = NaNBytes;
            }
            index += 4;

            //Latitude
            if (state == 1)
            {
                buffer.BLatitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLatitude[i] = data[index + i]; };
                    buffer.Latitude = formatFloat(a);
            }
            else
            {
                buffer.Latitude = double.NaN;
                buffer.BLatitude = NaNBytes;
            }
            index += 4;

            //Latitude Processed
            if (state == 1)
            {
                buffer.BLatitude_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLatitude_Processed[i] = data[index + i]; };
                var Latitude = formatFloat(a);
                buffer.Latitude_Processed = Latitude;
            }
            else
            {
                buffer.Latitude_Processed = double.NaN;
                buffer.BLatitude_Processed = NaNBytes;
            }
            index += 4;

            //Longitude
            if (state == 1)
            {
                buffer.BLongitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLongitude[i] = data[index + i]; };
                    buffer.Longitude = formatFloat(a);
            }
            else
            {
                buffer.Longitude = double.NaN;
                buffer.BLongitude = NaNBytes;
            }
            index += 4;

            //Longitude Processed
            if (state == 1)
            {
                buffer.BLongitude_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLongitude_Processed[i] = data[index + i]; };
                var Longitude = formatFloat(a);
                buffer.Longitude_Processed = Longitude;
            }
            else
            {
                buffer.Longitude_Processed = double.NaN;
                buffer.BLongitude_Processed = NaNBytes;
            }

            index += 4;

            //Altitude
            if (state == 1)
            {
                buffer.BAltitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAltitude[i] = data[index + i]; };
                buffer.Altitude = formatFloat(a);
            }
            else
            {
                buffer.Altitude = double.NaN;
                buffer.BAltitude = NaNBytes;
            }
            index += 4;

            //Altitude Processed
            if (state == 1)
            {
                buffer.BAltitude_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAltitude_Processed[i] = data[index + i]; };
                var Altitude = formatFloat(a);
                buffer.Altitude_Processed = Altitude;
            }
            else
            {
                buffer.Altitude_Processed = double.NaN;
                buffer.BAltitude_Processed = NaNBytes;
            }

            index += 4;

            // Vx
            if (state == 1)
            {
                buffer.BVx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVx[i] = data[index + i]; };
                buffer.Vx = formatFloat(a);
            }
            else
            {
                buffer.Vx = double.NaN;
                buffer.BVx = NaNBytes;
            }
            index += 4;

            // Vx Processed
            if (state == 1)
            {
                buffer.BVx_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVx_Processed[i] = data[index + i]; };
                buffer.Vx_Processed = formatFloat(a);
            }
            else
            {
                buffer.Vx_Processed = double.NaN;
                buffer.BVx_Processed = NaNBytes;
            }
            index += 4;

            // Vy
            if (state == 1)
            {
                buffer.BVy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVy[i] = data[index + i]; };
                buffer.Vy = formatFloat(a);
            }
            else
            {
                buffer.Vy = double.NaN;
                buffer.BVy = NaNBytes;
            }

            index += 4;

            // Vy Processed
            if (state == 1)
            {
                buffer.BVy_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVy_Processed[i] = data[index + i]; };
                buffer.Vy_Processed = formatFloat(a);
            }
            else
            {
                buffer.Vy_Processed = double.NaN;
                buffer.BVy_Processed = NaNBytes;
            }
            index += 4;

            // Vz
            if (state == 1)
            {
                buffer.BVz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVz[i] = data[index + i]; };
                buffer.Vz = formatFloat(a);
            }
            else
            {
                buffer.Vz = double.NaN;
                buffer.BVz = NaNBytes;
            }
            index += 4;

            // Vz Processed
            if (state == 1)
            {
                buffer.BVz_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVz_Processed[i] = data[index + i]; };
                buffer.Vz_Processed = formatFloat(a);
            }
            else
            {
                buffer.Vz_Processed = double.NaN;
                buffer.BVz_Processed = NaNBytes;
            }
            index += 4;

            //Ax acceleration
            if (state == 1)
            {
                buffer.BAx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAx[i] = data[index + i]; };
                buffer.Ax = formatFloat(a);
            }
            else
            {
                buffer.Ax = double.NaN;
                buffer.BAx = NaNBytes;
            }
            index += 4;

            //Ay acceleration
            if (state == 1)
            {
                buffer.BAy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAy[i] = data[index + i]; };
                buffer.Ay = formatFloat(a);
            }
            else
            {
                buffer.Ay = double.NaN;
                buffer.BAy = NaNBytes;
            }
            index += 4;

            //Az acceleration
            if (state == 1)
            {
                buffer.BAz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAz[i] = data[index + i]; };
                buffer.Az = formatFloat(a);
            }
            else
            {
                buffer.Az = double.NaN;
                buffer.BAz = NaNBytes;
            }
            index += 4;

            //V, V_Processed, A
            if (state == 1)
            {
                buffer.V = (Math.Sqrt(Math.Pow(buffer.Vx, 2) + Math.Pow(buffer.Vy, 2) + Math.Pow(buffer.Vz, 2)));
                var unformatted = unformatFloat(buffer.V);
                for (int i = 0; i < 4; i++)
                { 
                    buffer.BV[i] = (byte)((long)(unformatted % 256)); 
                    unformatted /= 256; 
                }
                //buffer.BV = Functions.CopyByteWithOffset(BitConverter.GetBytes(buffer.V),4);
                buffer.V_Processed = (Math.Sqrt(Math.Pow(buffer.Vx_Processed, 2) + Math.Pow(buffer.Vy_Processed, 2) + Math.Pow(buffer.Vz_Processed, 2)));
                buffer.BV_Processed = CopyByteWithOffset(BitConverter.GetBytes(buffer.V_Processed), 4);
                buffer.A = (Math.Sqrt(Math.Pow(buffer.Ax, 2) + Math.Pow(buffer.Ay, 2) + Math.Pow(buffer.Az, 2)));
                unformatted = unformatFloat(buffer.A);
                for (int i = 0; i < 4; i++) { buffer.BA[i] = (byte)((int)unformatted % 256); unformatted /= 256; } 
            }
            else
            {
                buffer.V = double.NaN;
                buffer.V_Processed = double.NaN;
                buffer.A = double.NaN;
                buffer.BV = NaNBytes;
                buffer.BV_Processed = NaNBytes;
                buffer.BA = NaNBytes;
            }


            //SNR GPS
            int readSNR = 0;
            for (int i = 0; i < 12; i++)
                buffer.BGPSstat[0][i] = data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                GPS[0][i].SNR = 0;

                if (GPS[0][i].Signal_Status != 0)      //visible
                {
                    GPS[0][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //GLONASS SNR
            readSNR = 0;
            for (int i = 0; i < 12; i++)
                buffer.BGLONASSstat[0][i] = data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                GLONASS[0][i].SNR = 0;

                if (GLONASS[0][i].Signal_Status != 0)      //visible
                {
                    GLONASS[0][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            a = data[index] + data[index + 1] * 256;
            buffer.Bdatetime[0] = data[index];
            buffer.Bdatetime[1] = data[index + 1];
            var weekNumber = (int)a;
            index += 2;

            //UTC Offset
            index += 2;

            
            //int localTOW = (int)a;
            index += 4;

            var datetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            datetimeUTC = datetimeUTC.AddDays(weekNumber * 7);
            datetimeUTC = datetimeUTC.AddMilliseconds(TOW);
            if (datetimeUTC.Year > 2020 || datetimeUTC.Year < 2000)
                buffer.error = true;

            buffer.datetime = datetimeUTC;
            PTime = datetimeUTC;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int packetDelay = (int)a;
            index += 4;

            // DOP
            double GDOP = 0;
            buffer.PDOP = 0;

            if (state == 1)
            {
                GDOP = Decompress_DOP(data[index]);
                index++;
                buffer.PDOP = Decompress_DOP(data[index]);
                //buffer.BPDOP = CopyByteWithOffset(BitConverter.GetBytes(buffer.PDOP), 4);
                var unformatted = unformatFloat(buffer.PDOP);
                for (int i = 0; i < 4; i++) { buffer.BPDOP[i] = (byte)((int)unformatted % 256); unformatted /= 256; } 
                index++;
                buffer.HDOP = Decompress_DOP(data[index]);
                index++;
                buffer.VDOP = Decompress_DOP(data[index]);
                index++;
                buffer.TDOP = Decompress_DOP(data[index]);
                index++;
            }
            else
            {
                buffer.PDOP = double.NaN;
                buffer.BPDOP = NaNBytes;
                buffer.VDOP = double.NaN;
                buffer.HDOP = double.NaN;
                buffer.TDOP = double.NaN;
            }

            // Checksum
            index += 2;
            return buffer;
            // Trailer

            /*
            if (SNRControlUpdate && radioGroupDevice.SelectedIndex == SerialNum)
            {
                Update_SNR_Chart(chartCtrlSNRs_Ant1, GPS_satellite);
                Update_SNR_Chart(chartCtrlSNRs_Ant1, GLONASS_satellite);

                SNRControlUpdate = false;
            }

             */
        }

        public static SingleDataBuffer Process_Binary_Message_Compact(byte[] data, int SerialNum, ref List<Satellite[]> GPS,ref List<Satellite[]> GLONASS, ref DateTime PTime)
        {
            SingleDataBuffer buffer = new SingleDataBuffer();
            byte[] NaNBytes = { 0, 0, 248, 255 };
            buffer.BGPSstat.Add(new byte[12]);
            buffer.BGLONASSstat.Add(new byte[12]);
            buffer.ChannelCounter = 1;
            buffer.BSatStats[16] = (byte)1;
            var tempsat = new List<Satellite[]>();
            if (GPS.Count < 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    tempsat.Add(new Satellite[32]);
                    for (int j = 0; j < 32; j++)
                    {
                        tempsat[i][j] = new Satellite();
                    }
                    if (i % 2 == 0)
                        GPS.Add(tempsat[i]);
                    else
                        GLONASS.Add(tempsat[i]);
                }
            }
            int msgSize = 0;

            if (data[1] == BIN_COMPACT)
                msgSize = BIN_COMPACT_MSG_SIZE;

            int checksum = calcrc(data, msgSize - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[msgSize - 3] || checksum1 != data[msgSize - 2])
            {
                //Show_Error("CheckSum Error", "Recieved checksum is incorrect");
                throw new Exception("Checksum Error");
                //return new SingleDataBuffer();
            }

            int index = 1;      //header
            index++;        //messageType

            // State
            int state = data[index];
            buffer.state = state;
            buffer.Bstate = data[index];
            index++;

            // Temperature
            buffer.Temperature = data[index];
            buffer.BTemperature = data[index];
            index++;

            buffer.Bdatetime[5] = data[index + 3];
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.Bdatetime[2 + i] = data[index + i]; }
            double TOW = a;      //millisecond
            index += 4;

            buffer.NumOfVisibleSats = 0;
            buffer.BSatStats[3] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[i] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                GLONASS[0][i].Signal_Status = 0;
                if (a % 2 == 1)
                {
                    GPS[0][i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GPS[0][i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            var temp = buffer.NumOfVisibleSats;
            for (int i = 0; i <= 3; i++)
            {
                buffer.BNumOfVisibleStats[i] = (byte)(temp % 256);
                temp /= 256;
            }
            index += 4;

            buffer.NumOfUsedSats = 0;
            buffer.BSatStats[7] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BSatStats[i + 4] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[0][i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            var temp2 = buffer.NumOfUsedSats;
            for (int i = 0; i <= 3; i++)
            {
                buffer.BNumOfUsedStats[i] = (byte)(temp2 % 256);
                temp2 /= 256;
            }
            index += 4;

            // X
            if (state == 1)
            {
                buffer.BX[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BX[i] = data[index + i]; };
                buffer.X = formatFloat(a);
                buffer.X_Processed = buffer.X;
                buffer.BX_Processed = buffer.BX;
            }
            else
            {
                buffer.X = double.NaN;
                buffer.X_Processed = double.NaN;
                buffer.BX = NaNBytes;
                buffer.BX_Processed = NaNBytes;
            }
            index += 4;

            // Y
            if (state == 1)
            {
                buffer.BY[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BY[i] = data[index + i]; };
                buffer.Y = formatFloat(a);
                buffer.Y_Processed = buffer.Y;
                buffer.BY_Processed = buffer.BY;
            }
            else
            {
                buffer.Y = double.NaN;
                buffer.Y_Processed = double.NaN;
                buffer.BY = NaNBytes;
                buffer.BY_Processed = NaNBytes;
            }
            index += 4;

            // Z
            if (state == 1)
            {
                buffer.BZ[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BZ[i] = data[index + i]; };

                buffer.Z = formatFloat(a);
                buffer.Z_Processed = buffer.Z;
                buffer.BZ_Processed = buffer.BZ;
            }
            else
            {
                buffer.Z = double.NaN;
                buffer.Z_Processed = double.NaN;
                buffer.BZ = NaNBytes;
                buffer.BZ_Processed = NaNBytes;
            }
            index += 4;

            //Latitude
            if (state == 1)
            {
                buffer.BLatitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLatitude[i] = data[index + i]; };
                buffer.Latitude = formatFloat(a);
                buffer.Latitude_Processed = buffer.Latitude;
                buffer.BLatitude_Processed = buffer.BLatitude;
            }
            else
            {
                buffer.Latitude = double.NaN;
                buffer.Latitude_Processed = double.NaN;
                buffer.BLatitude = NaNBytes;
                buffer.BLatitude_Processed = NaNBytes;
            }
            index += 4;

            //Longitude
            if (state == 1)
            {
                buffer.BLongitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BLongitude[i] = data[index + i]; };
                buffer.Longitude = formatFloat(a);
                buffer.Longitude_Processed = buffer.Longitude;
                buffer.BLongitude_Processed = buffer.BLongitude;
            }
            else
            {
                buffer.Longitude = double.NaN;
                buffer.Longitude_Processed = double.NaN;
                buffer.BLongitude = NaNBytes;
                buffer.BLongitude_Processed = NaNBytes;
            }
            index += 4;

            //Altitude
            if (state == 1)
            {
                buffer.BAltitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BAltitude[i] = data[index + i]; };
                buffer.Altitude = formatFloat(a);
                buffer.Altitude_Processed = buffer.Altitude;
                buffer.BAltitude_Processed = buffer.BAltitude;
            }
            else
            {
                buffer.Altitude = double.NaN;
                buffer.Altitude_Processed = double.NaN;
                buffer.BAltitude = NaNBytes;
                buffer.BAltitude_Processed = NaNBytes;
            }
            index += 4;

            // Vx
            if (state == 1)
            {
                buffer.BVx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVx[i] = data[index + i]; };
                buffer.Vx = formatFloat(a);
                buffer.Vx_Processed = buffer.Vx;
                buffer.BVx_Processed = buffer.BVx;
            }
            else
            {
                buffer.Vx = double.NaN;
                buffer.Vx_Processed = double.NaN;
                buffer.BVx = NaNBytes;
                buffer.BVx_Processed = NaNBytes;
            }
            index += 4;

            // Vy
            if (state == 1)
            {
                buffer.BVy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVy[i] = data[index + i]; };
                buffer.Vy = formatFloat(a);
                buffer.Vy_Processed = buffer.Vy;
                buffer.BVy_Processed = buffer.BVy;
            }
            else
            {
                buffer.Vy = double.NaN;
                buffer.Vy_Processed = double.NaN;
                buffer.BVy = NaNBytes;
                buffer.BVy_Processed = NaNBytes;
            }
            index += 4;

            // Vz
            if (state == 1)
            {
                buffer.BVz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BVz[i] = data[index + i]; };
                buffer.Vz = formatFloat(a);
                buffer.Vz_Processed = buffer.Vz;
                buffer.BVz_Processed = buffer.BVz;
            }
            else
            {
                buffer.Vz = double.NaN;
                buffer.Vz_Processed = double.NaN;
                buffer.BVz = NaNBytes;
                buffer.BVz_Processed = NaNBytes;
            }
            index += 4;

            //V
            if (state == 1)
            {
                var temp3 = Math.Sqrt(Math.Pow(buffer.Vx, 2) + Math.Pow(buffer.Vy, 2) + Math.Pow(buffer.Vz, 2));
                buffer.V = (temp3);
                buffer.V_Processed = (temp3);
                var unformatted = unformatFloat(temp3);
                for (int i = 0; i < 4; i++) { buffer.BV[i] = (byte)((int)unformatted % 256); unformatted /= 256; }
                buffer.BV_Processed = buffer.BV;// = CopyByteWithOffset(BitConverter.GetBytes(temp3), 4);
            }
            else
            {
                buffer.V = double.NaN;
                buffer.V_Processed = double.NaN;
                buffer.BV = NaNBytes;
                buffer.BV_Processed = NaNBytes;
            }
            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;
            if (state == 1)
            {
                buffer.BPDOP[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BPDOP[i] = data[index + i]; }
                buffer.PDOP = (formatFloat(a));
            }
            else
            {
                buffer.PDOP = double.NaN;
                buffer.BPDOP = NaNBytes;
            }

            index += 4;

            int readSNR = 0;
            bool highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS[0][i].SNR = 0;

                if (GPS[0][i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        GPS[0][i].SNR = (data[index] & 0xF) + 40;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        GPS[0][i].SNR = ((data[index] & 0xF0) >> 4) + 40;
                    }
                }
            }
            index += 8 - readSNR;

            buffer.Bdatetime[0] = data[index];
            buffer.Bdatetime[1] = data[index + 1];

            a = data[index] + data[index + 1] * 256;
            var weekNumber = (int)a + 1024;
            index += 2;

            
            //int localTOW = (int)a;
            index += 4;

            var datetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            datetimeUTC = datetimeUTC.AddDays(weekNumber * 7);
            datetimeUTC = datetimeUTC.AddMilliseconds(TOW);

            dbuffer.datetime = datetimeUTC;
            PTime = datetimeUTC;

            // Reserved
            GDOP = data[index];
            HDOP = data[index + 1];
            VDOP = data[index + 2];
            TDOP = data[index + 3];
            if (GDOP > 100)
            {
                GDOP -= 90;
                HDOP -= 90;
                VDOP -= 90;
                TDOP -= 90;
            }
            else
            {
                GDOP /= 10;
                HDOP /= 10;
                VDOP /= 10;
                TDOP /= 10;
            }
            if (state == 1)
            {
                buffer.HDOP = HDOP;
                buffer.VDOP = VDOP;
                buffer.TDOP = TDOP;
            }
            else
            {
                buffer.TDOP = double.NaN;
                buffer.HDOP = double.NaN;
                buffer.VDOP = double.NaN;
            }
            index += 7;

            // Checksum
            index += 2;
            return buffer;
            // Trailer

        }

        public static void Process_Binary_Message_SupplementGPS(byte[] data, int SerialNum,ref List<Satellite[]> GPS)
        {

            var tempsat = new List<Satellite[]>();
            if (GPS.Count < 1)
            {
                    tempsat.Add(new Satellite[32]);
                    for (int j = 0; j < 32; j++)
                    {
                        tempsat[0][j] = new Satellite();
                    }
                    GPS.Add(tempsat[0]);
            }

            int id = (SerialNum == 0) ? 0 : 1;
            int checksum = calcrc(data, BIN_GPS_SUPPLEMENT_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[BIN_GPS_SUPPLEMENT_MSG_SIZE - 3] || checksum1 != data[BIN_GPS_SUPPLEMENT_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");
            }

           /* if (SerialNum == 0)
                DataReceiveTimeOut = 0;
            else
                DataReceiveTimeOutS2 = 0;
            */

            int index = 1;  //header
            index++;        //messageType
            index++;        //state
            index++;        // Reserved

            // PRNs
            byte[] PRNs = new byte[12];
            for (int i = 0; i < 12; i++)
                PRNs[i] = data[i + index];
            index += 12;

            for (int i = 0; i < 32; i++)
            {
                GPS[id][i].Azimuth = Azimuth_OutOfRange;
                GPS[id][i].Elevation = Elevation_OutOfRange;
            }

            for (int i = 0; i < 12; i++)
                if (PRNs[i] != 255)
                {
                    GPS[id][PRNs[i]].Elevation = data[i + index];
                    if (GPS[id][PRNs[i]].Elevation > 127)
                        GPS[id][PRNs[i]].Elevation -= 256;
                    GPS[id][PRNs[i]].Azimuth = data[i * 2 + (index + 12)] + data[i * 2 + 1 + (index + 12)] * 256;
                    if (GPS[id][PRNs[i]].Azimuth > 32767)
                        GPS[id][PRNs[i]].Azimuth -= 32768;
                }

            index += 24;
        }

        public static void Process_Binary_Message_SupplementGLONASS(byte[] data, int SerialNum,ref List<Satellite[]> GLONASS)
        {

            var tempsat = new List<Satellite[]>();
            if (GLONASS.Count < 1)
            {
                tempsat.Add(new Satellite[32]);
                for (int j = 0; j < 32; j++)
                {
                    tempsat[0][j] = new Satellite();
                }
                GLONASS.Add(tempsat[0]);
            }
            int id = (SerialNum == 0) ? 0 : 1;

            int checksum = calcrc(data, BIN_GLONASS_SUPPLEMENT_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[BIN_GLONASS_SUPPLEMENT_MSG_SIZE - 3] || checksum1 != data[BIN_GLONASS_SUPPLEMENT_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");
            }

            int index = 1;  //header

            int messageType = data[index];
            index++;

            // State
            int state = data[index];
            index++;

            // Reserved
            index++;

            // PRNs
            byte[] PRNs = new byte[12];
            for (int i = 0; i < 12; i++)
                PRNs[i] = data[i + index];
            index += 12;

            for (int i = 0; i < 28; i++)
            {
                GLONASS[id][i].Azimuth = Azimuth_OutOfRange;
                GLONASS[id][i].Elevation = Elevation_OutOfRange;
            }

            for (int i = 0; i < 12; i++)
                if (PRNs[i] != 255)
                {
                    GLONASS[id][PRNs[i]].Elevation = data[i + index];
                    if (GLONASS[id][PRNs[i]].Elevation > 127)
                        GLONASS[id][PRNs[i]].Elevation -= 256;
                    GLONASS[id][PRNs[i]].Azimuth = data[i * 2 + (index + 12)] + data[i * 2 + 1 + (index + 12)] * 256;
                    if (GLONASS[id][PRNs[i]].Azimuth > 32767)
                        GLONASS[id][PRNs[i]].Azimuth -= 32768;
                }
            index += 24;
        }

        public static string Process_Binary_Message_Debug(byte[] data)
        {
            string Debug_text = null;

            for (int i = 2; i <= BIN_DEBUG_MSG_SIZE; i++)
            {
                if (data[i] == 0)
                    break;

                Debug_text += (char)data[i];
            }

            return Debug_text;
        }

        public static SingleDataBuffer Process_Binary_Message_RawData(byte[] data, int SerialNum)
        {          
            int index = 0, buffercounter = 0;
            Int64 a;
            byte checksum0 = 0, checksum1 = 0;
            SingleDataBuffer dbuf = new SingleDataBuffer();

            int checksum = calcrc(data, BIN_RAW_DATA_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);

            //if (checksum0 != data[BIN_RAW_DATA_MSG_SIZE - 3] || checksum1 != data[BIN_RAW_DATA_MSG_SIZE - 2])
            //{
            //   throw new Exception("Checksum Error");
            //}

            dbuf.rawbuffer.RawReceived = true;
            index++;    //header
            index++;    //messageType
            index++;    // State

            // Temperature
            //rdBuf.Temperature[rdBuf.counter] = data[index];
            //index++;

            int nReceiver = data[index];
            index++;

            
            //TOW
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            dbuf.rawbuffer.TOW.Add(a);
            buffercounter = dbuf.rawbuffer.TOW.Count - 1;
            index += 4;

            //satellite Positions
            dbuf.rawbuffer.satPos.Add(new CartesianCoordinate[12]);
            for (int j = 0; j < 12; j++)
            {
                dbuf.rawbuffer.satPos[buffercounter][j] = new CartesianCoordinate();

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.satPos[buffercounter][j].x = formatDouble(a);
                index += 8;

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.satPos[buffercounter][j].y = formatDouble(a);
                index += 8;

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.satPos[buffercounter][j].z = formatDouble(a);
                index += 8;
            }

            //PseudoRanges
            dbuf.rawbuffer.PseudoRanges.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.PseudoRanges[buffercounter][j] = formatDouble(a);
                index += 8;
            }

            //Prn
            dbuf.rawbuffer.Prn.Add(new int[12]);
            for (int i = 0; i < 12; i++)
            {
                dbuf.rawbuffer.Prn[buffercounter][i] = data[index];
                index++;
            }

            //dopplers
            dbuf.rawbuffer.dopplers.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.dopplers[buffercounter][j] = formatFloat(a);
                index += 4;
            }

            //reliability
            dbuf.rawbuffer.reliability.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                dbuf.rawbuffer.reliability[buffercounter][j] = formatFloat(a);
                index += 4;
            }

            //pAngle
            dbuf.rawbuffer.pAngle.Add(new double[12][]);
            for (int j = 0; j < 12; j++)
            {
                dbuf.rawbuffer.pAngle[buffercounter][j] = new double[2];
                for (int k = 0; k < 2; k++)
                {
                    a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                    dbuf.rawbuffer.pAngle[buffercounter][j][k] = formatFloat(a);
                    index += 4;
                }
            }

            //snr
            dbuf.rawbuffer.snr.Add(new double[12][]);
            for (int j = 0; j < 12; j++)
            {
                dbuf.rawbuffer.snr[buffercounter][j] = new double[2];
                for (int k = 0; k < 2; k++)
                {
                    a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                    dbuf.rawbuffer.snr[buffercounter][j][k] = formatFloat(a);
                    index += 4;
                }
            }

            return dbuf;
            /*rdBuf.counter++;
            if (rdBuf.counter >= Max_buf)
            {
                rdBuf.overLoad++;
                rdBuf.counter = 0;
            }*/
        }

        public static SingleDataBuffer Process_Binary_Message_Dual_Channel(byte[] data, int serialNum, ref List<Satellite[]> GPS, ref List<Satellite[]> GLONASS, ref DateTime PTime)
        {
            SingleDataBuffer dbuf = new SingleDataBuffer();
            dbuf.BGPSstat.Add(new byte[12]);
            dbuf.BGPSstat.Add(new byte[12]);
            dbuf.BGLONASSstat.Add(new byte[12]);
            dbuf.BGLONASSstat.Add(new byte[12]);
            var temp = new List<Satellite[]>();
            var max = 2*(2-GPS.Count);
            if (GPS.Count < 2)
            {   
                for (int i = 0; i < max; i++)
                {
                    temp.Add(new Satellite[32]);
                    for (int j = 0; j < 32; j++)
                    {
                        temp[i][j] = new Satellite();
                    }
                    if (i%2 == 0)
                    GPS.Add(temp[i]);
                    else
                    GLONASS.Add(temp[i]);
                }
            }

            byte[] NaNBytes = { 0, 0, 248, 255 };
            dbuf.ChannelCounter = 2;
            dbuf.BSatStats[16] = (byte)2;
            int SatIndex = serialNum - 1;

            int checksum = calcrc(data, BIN_DUAL_CHANNEL_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[BIN_DUAL_CHANNEL_MSG_SIZE - 3] || checksum1 != data[BIN_DUAL_CHANNEL_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");
                //Error
            }
            //if (serialNum == 0)
             //   DataReceiveTimeOut = 0;
            //else
             //   DataReceiveTimeOutS2 = 0;

            int index = 1;      //header
            index++;    //messageType

            // State
            int state = data[index];
            dbuf.state = state;
            dbuf.Bstate = data[index];
            index++;

            // Temperature
            dbuf.Temperature = data[index];
            dbuf.BTemperature = data[index];
            index++;

            dbuf.Bdatetime[5] = data[index + 3];
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.Bdatetime[2 + i] = data[index + i]; }
            double TOW = a;     //millisecond
            index += 4;

            dbuf.NumOfVisibleSats = 0;

            dbuf.BSatStats[3] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[i] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GPS.Count;j++)
                        GPS[j][i].Signal_Status = 1;       //visible
                    dbuf.NumOfVisibleSats++;
                }
                else
                    for (int j = 0; j < GPS.Count; j++)
                        GPS[j][i].Signal_Status = 0;       //not visible

                a >>= 1;
            }
            var temp2 = dbuf.NumOfVisibleSats;
            for (int i = 0; i <= 3; i++)
            {
                dbuf.BNumOfVisibleStats[i] = (byte)(temp2 % 256);
                temp2 /= 256;
            }
            index += 4;

            dbuf.NumOfUsedSats = 0;
            dbuf.BSatStats[7] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[4+i] = data[index + i]; }
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GPS.Count; j++)
                        GPS[j][i].Signal_Status = 2;       //Used
                    dbuf.NumOfUsedSats++;
                }
                a >>= 1;
                //GPSsatAnt2[i].Signal_Status = GPS[SatIndex][i].Signal_Status;
            }
            temp2 = dbuf.NumOfUsedSats;
            for (int i = 0; i <= 3; i++)
            {
                dbuf.BNumOfUsedStats[i] = (byte)(temp2 % 256);
                temp2 /= 256;
            }
            index += 4;
            dbuf.BSatStats[11] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[8 + i] = data[index + i]; }
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GLONASS.Count; j++)
                        GLONASS[j][i].Signal_Status = 1;       //visible
                    dbuf.NumOfVisibleSats++;
                }
                else
                    for (int j = 0; j < GLONASS.Count; j++)
                        GLONASS[j][i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            dbuf.BSatStats[15] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[12 + i] = data[index + i]; }
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GLONASS.Count;j++)
                        GLONASS[j][i].Signal_Status = 2;       //Used
                    dbuf.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;
            // X
            if (state == 1)
            {
                dbuf.BX_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BX_Processed[i] = data[index + i]; };
                dbuf.X = dbuf.X_Processed = formatFloat(a);
                dbuf.BX = dbuf.BX_Processed;

            }
            else
            {
                dbuf.X = dbuf.X_Processed = double.NaN;
                dbuf.BX = dbuf.BX_Processed = NaNBytes;
            }
            index += 4;

            // Y
            if (state == 1)
            {
                dbuf.BY_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BY_Processed[i] = data[index + i]; };
                dbuf.Y = dbuf.Y_Processed = formatFloat(a);
                dbuf.BY = dbuf.BY_Processed;
            }
            else
            {
                dbuf.Y = dbuf.Y_Processed = double.NaN;
                dbuf.BY = dbuf.BY_Processed = NaNBytes;
            }
            index += 4;

            // Z
            if (state == 1)
            {
                dbuf.BZ_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BZ_Processed[i] = data[index + i]; };
                dbuf.Z = dbuf.Z_Processed = formatFloat(a);
                dbuf.BZ = dbuf.BZ_Processed;
            }
            else
            {
                dbuf.Z = dbuf.Z_Processed = double.NaN;
                dbuf.BZ = dbuf.BZ_Processed = NaNBytes;
            }
            index += 4;

            //Latitude
            if (state == 1)
            {
                dbuf.BLatitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BLatitude[i] = data[index + i]; };
                dbuf.Latitude_Processed = dbuf.Latitude = formatFloat(a);
                dbuf.BLatitude_Processed = dbuf.BLatitude;
            }
            else
            {
                dbuf.Latitude_Processed = dbuf.Latitude = double.NaN;
                dbuf.BLatitude_Processed = dbuf.BLatitude = NaNBytes;
            }
            index += 4;

            //Longitude
            if (state == 1)
            {
                dbuf.BLongitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BLongitude[i] = data[index + i]; };
                dbuf.Longitude_Processed = dbuf.Longitude = formatFloat(a);
                dbuf.BLongitude_Processed = dbuf.BLongitude;
            }
            else
            {
                dbuf.Longitude_Processed = dbuf.Longitude = double.NaN;
                dbuf.BLongitude_Processed = dbuf.BLongitude = NaNBytes;
            }
            index += 4;

            //Altitude
            if (state == 1)
            {
                dbuf.BAltitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BAltitude[i] = data[index + i]; };
                dbuf.Altitude_Processed = dbuf.Altitude = formatFloat(a);
                dbuf.BAltitude_Processed = dbuf.BAltitude;
            }
            else
            {
                dbuf.Altitude_Processed = dbuf.Altitude = double.NaN;
                dbuf.BAltitude_Processed = dbuf.BAltitude = NaNBytes;
            }
            index += 4;

            // Vx
            if (state == 1)
            {
                dbuf.BVx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVx[i] = data[index + i]; };
                dbuf.Vx = formatFloat(a);
            }
            else
            {
                dbuf.Vx = double.NaN;
                dbuf.BVx = NaNBytes;
            }
            index += 4;

            // Vx Processed
            if (state == 1)
            {
                dbuf.BVx_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVx_Processed[i] = data[index + i]; };
                dbuf.Vx_Processed = formatFloat(a);
            }
            else
            {
                dbuf.Vx_Processed = double.NaN;
                dbuf.BVx_Processed = NaNBytes;
            }
            index += 4;

            // Vy
            if (state == 1)
            {
                dbuf.BVy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVy[i] = data[index + i]; };
                dbuf.Vy = formatFloat(a);
            }
            else
            {
                dbuf.Vy = double.NaN;
                dbuf.BVy = NaNBytes;
            }
            index += 4;

            // Vy Processed
            if (state == 1)
            {
                dbuf.BVy_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVy_Processed[i] = data[index + i]; };
                dbuf.Vy_Processed = formatFloat(a);
            }
            else
            {
                dbuf.Vy_Processed = double.NaN;
                dbuf.BVy_Processed = NaNBytes;
            }
            index += 4;

            // Vz
            if (state == 1)
            {
                dbuf.BVz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVz[i] = data[index + i]; };
                dbuf.Vz = formatFloat(a);
            }
            else
            {
                dbuf.Vz = double.NaN;
                dbuf.BVz= NaNBytes;
            }
            index += 4;

            // Vz Processed
            if (state == 1)
            {
                dbuf.BVz_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVz_Processed[i] = data[index + i]; };
                dbuf.Vz_Processed = formatFloat(a);
            }
            else
            {
                dbuf.Vz_Processed = double.NaN;
                dbuf.BVz_Processed = NaNBytes;
            }
            index += 4;

            //Ax acceleration
            if (state == 1)
            {
                dbuf.BAx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BAx[i] = data[index + i]; };
                dbuf.Ax = formatFloat(a);
            }
            else
            {
                dbuf.Ax = double.NaN;
                dbuf.BAx = NaNBytes;
            }
            index += 4;

            //Ay acceleration
            if (state == 1)
            {
                dbuf.BAy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BAy[i] = data[index + i]; };
                dbuf.Ay = formatFloat(a);
            }
            else
            {
                dbuf.Ay = double.NaN;
                dbuf.BAy = NaNBytes;
            }
            index += 4;

            //Az acceleration
            if (state == 1)
            {
                dbuf.BAz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BAz[i] = data[index + i]; };
                dbuf.Az = formatFloat(a);
            }
            else
            {
                dbuf.Az = double.NaN;
                dbuf.BAz = NaNBytes;
            }
            index += 4;

            //V, V_Processed, A
            if (state == 1)
            {
                dbuf.V = (Math.Sqrt(Math.Pow(dbuf.Vx, 2) + Math.Pow(dbuf.Vy, 2) + Math.Pow(dbuf.Vz, 2)));
                var Unformatted = unformatFloat(dbuf.V);
                for (int i = 0; i < 4; i++) { dbuf.BV[i] = (byte)((int)Unformatted % 256); Unformatted /= 256; }
                //dbuf.BV = Functions.CopyByteWithOffset(BitConverter.GetBytes(dbuf.V), 4);
                dbuf.V_Processed = (Math.Sqrt(Math.Pow(dbuf.Vx_Processed, 2) + Math.Pow(dbuf.Vy_Processed, 2) + Math.Pow(dbuf.Vz_Processed, 2)));
                Unformatted = unformatFloat(dbuf.V_Processed);
                for (int i = 0; i < 4; i++) { dbuf.BV_Processed[i] = (byte)((int)Unformatted % 256); Unformatted /= 256; }
                //dbuf.BV_Processed = CopyByteWithOffset(BitConverter.GetBytes(dbuf.V_Processed), 4);
                dbuf.A = (Math.Sqrt(Math.Pow(dbuf.Ax, 2) + Math.Pow(dbuf.Ay, 2) + Math.Pow(dbuf.Az, 2)));
                Unformatted = unformatFloat(dbuf.A);
                for (int i = 0; i < 4; i++) { dbuf.BA[i] = (byte)((int)Unformatted % 256); Unformatted /= 256; }
                //dbuf.BA = Functions.CopyByteWithOffset(BitConverter.GetBytes(dbuf.A), 4);
            }
            else
            {
                dbuf.V = double.NaN;
                dbuf.V_Processed = double.NaN;
                dbuf.A = double.NaN;
                dbuf.BV = NaNBytes;
                dbuf.BV_Processed = NaNBytes;
                dbuf.BA = NaNBytes;
            }

            //SNR GPS
            int readSNR = 0;
            for (int i = 0; i < 12; i++) dbuf.BGPSstat[0][i] = data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                GPS[0][i].SNR = 0;

                if (GPS[0][i].Signal_Status != 0)      //visible
                {
                    GPS[0][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //GLONASS SNR
            readSNR = 0;
            for (int i = 0; i < 12; i++) dbuf.BGLONASSstat[0][i] = data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                GLONASS[0][i].SNR = 0;

                if (GLONASS[0][i].Signal_Status != 0)      //visible
                {
                    GLONASS[0][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //SNR GPS
            readSNR = 0;
            for (int i = 0; i < 12; i++) dbuf.BGPSstat[1][i] = data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                GPS[1][i].SNR = 0;

                if (GPS[1][i].Signal_Status != 0)      //visible
                {
                    GPS[1][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //GLONASS SNR
            readSNR = 0;
            for (int i = 0; i < 12; i++) dbuf.BGLONASSstat[1][i] = data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                GLONASS[1][i].SNR = 0;

                if (GLONASS[1][i].Signal_Status != 0)      //visible
                {
                    GLONASS[1][i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            a = data[index] + data[index + 1] * 256;
            dbuf.Bdatetime[0] = data[index];
            dbuf.Bdatetime[1] = data[index + 1];
            var weekNumber = (int)a;
            index += 2;

            //UTC Offset
            index += 2;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int localTOW = (int)a;
            index += 4;

            var datetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            datetimeUTC = datetimeUTC.AddDays(weekNumber * 7);
            datetimeUTC = datetimeUTC.AddMilliseconds(TOW);

            dbuf.datetime = datetimeUTC;
            PTime = datetimeUTC;

////
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int packetDelay = (int)a;
            index += 4;

            // DOP
            dbuf.PDOP = 0;

            if (state == 1)
            {
                dbuf.PDOP = Decompress_DOP(data[index]);
                var unformatted = unformatFloat(dbuf.PDOP);
                for (int i = 0; i < 4; i++) { dbuf.BPDOP[i] = (byte)((int)unformatted % 256); unformatted /= 256; } 
                //dbuf.BPDOP = CopyByteWithOffset(BitConverter.GetBytes(dbuf.PDOP), 4);
                index++;
                dbuf.HDOP = Decompress_DOP(data[index]);
                index++;
                dbuf.VDOP = Decompress_DOP(data[index]);
                index++;
                dbuf.TDOP = Decompress_DOP(data[index]);
                index++;
            }
            else
            {
                dbuf.PDOP = double.NaN;
                dbuf.BPDOP = NaNBytes;
                dbuf.TDOP = double.NaN;
                dbuf.HDOP = double.NaN;
                dbuf.VDOP = double.NaN;
            }

            // Checksum
            index += 2;

            // Trailer
            return dbuf;
           // Update_Controls(dbuf, state, TOW, serialNum, HDOP, VDOP);
        }

        public static SingleDataBuffer Process_Binary_Message_Compact_Dual_Channel(byte[] data, int serialNum, ref List<Satellite[]> GPS, ref List<Satellite[]> GLONASS, ref DateTime PTime)
        {
            SingleDataBuffer dbuf = new SingleDataBuffer();
            dbuf.BGPSstat.Add(new byte[12]);
            dbuf.BGPSstat.Add(new byte[12]);
            dbuf.BGLONASSstat.Add(new byte[12]);
            dbuf.BGLONASSstat.Add(new byte[12]);
            var temp = new List<Satellite[]>();
            var max = 2 * (2 - GPS.Count);
            if (GPS.Count < 2)
            {
                for (int i = 0; i < max; i++)
                {
                    temp.Add(new Satellite[32]);
                    for (int j = 0; j < 32; j++)
                    {
                        temp[i][j] = new Satellite();
                    }
                    if (i % 2 == 0)
                        GPS.Add(temp[i]);
                    else
                        GLONASS.Add(temp[i]);
                }
            }

            byte[] NaNBytes = { 0, 0, 248, 255 };
            dbuf.ChannelCounter = 2;
            dbuf.BSatStats[16] = (byte)2;
            int SatIndex = serialNum - 1;

            int checksum = calcrc(data, BIN_COMPACT_DUAL_CHANNEL_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[BIN_COMPACT_DUAL_CHANNEL_MSG_SIZE - 3] || checksum1 != data[BIN_COMPACT_DUAL_CHANNEL_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");
                //Error
            }
            //if (serialNum == 0)
            //   DataReceiveTimeOut = 0;
            //else
            //   DataReceiveTimeOutS2 = 0;

            int index = 1;      //header
            index++;    //messageType

            // State
            int state = data[index];
            dbuf.state = state;
            dbuf.Bstate = data[index];
            index++;

            // Temperature
            dbuf.Temperature = data[index];
            dbuf.BTemperature = data[index];
            index++;

            dbuf.Bdatetime[5] = data[index + 3];
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.Bdatetime[2 + i] = data[index + i]; }
            double TOW = a;     //millisecond
            index += 4;

            dbuf.NumOfVisibleSats = 0;

            dbuf.BSatStats[3] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[i] = data[index + i]; };
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GPS.Count; j++)
                    {
                        GPS[j][i].Signal_Status = 1;       //visible
                        GLONASS[j][i].Signal_Status = 0;
                    }
                    dbuf.NumOfVisibleSats++;
                }
                else
                    for (int j = 0; j < GPS.Count; j++)
                    {
                        GPS[j][i].Signal_Status = 0;       //not visible
                        GLONASS[j][i].Signal_Status = 0;
                    }

                a >>= 1;
            }
            var temp2 = dbuf.NumOfVisibleSats;
            for (int i = 0; i <= 3; i++)
            {
                dbuf.BNumOfVisibleStats[i] = (byte)(temp2 % 256);
                temp2 /= 256;
            }
            index += 4;

            dbuf.NumOfUsedSats = 0;
            dbuf.BSatStats[7] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BSatStats[4 + i] = data[index + i]; }
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    for (int j = 0; j < GPS.Count; j++)
                    {
                        GLONASS[j][i].Signal_Status = 0;
                        GPS[j][i].Signal_Status = 2;       //Used
                    }
                    dbuf.NumOfUsedSats++;
                }
                a >>= 1;
                //GPSsatAnt2[i].Signal_Status = GPS[SatIndex][i].Signal_Status;
            }
            temp2 = dbuf.NumOfUsedSats;
            for (int i = 0; i <= 3; i++)
            {
                dbuf.BNumOfUsedStats[i] = (byte)(temp2 % 256);
                temp2 /= 256;
            }
            index += 4;
            
            // X
            if (state == 1)
            {
                dbuf.BX_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BX_Processed[i] = data[index + i]; };
                dbuf.X = dbuf.X_Processed = formatFloat(a);
                dbuf.BX = dbuf.BX_Processed;

            }
            else
            {
                dbuf.X = dbuf.X_Processed = double.NaN;
                dbuf.BX = dbuf.BX_Processed = NaNBytes;
            }
            index += 4;

            // Y
            if (state == 1)
            {
                dbuf.BY_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BY_Processed[i] = data[index + i]; };
                dbuf.Y = dbuf.Y_Processed = formatFloat(a);
                dbuf.BY = dbuf.BY_Processed;
            }
            else
            {
                dbuf.Y = dbuf.Y_Processed = double.NaN;
                dbuf.BY = dbuf.BY_Processed = NaNBytes;
            }
            index += 4;

            // Z
            if (state == 1)
            {
                dbuf.BZ_Processed[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BZ_Processed[i] = data[index + i]; };
                dbuf.Z = dbuf.Z_Processed = formatFloat(a);
                dbuf.BZ = dbuf.BZ_Processed;
            }
            else
            {
                dbuf.Z = dbuf.Z_Processed = double.NaN;
                dbuf.BZ = dbuf.BZ_Processed = NaNBytes;
            }
            index += 4;

            //Latitude
            if (state == 1)
            {
                dbuf.BLatitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BLatitude[i] = data[index + i]; };
                dbuf.Latitude_Processed = dbuf.Latitude = formatFloat(a);
                dbuf.BLatitude_Processed = dbuf.BLatitude;
            }
            else
            {
                dbuf.Latitude_Processed = dbuf.Latitude = double.NaN;
                dbuf.BLatitude_Processed = dbuf.BLatitude = NaNBytes;
            }
            index += 4;

            //Longitude
            if (state == 1)
            {
                dbuf.BLongitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BLongitude[i] = data[index + i]; };
                dbuf.Longitude_Processed = dbuf.Longitude = formatFloat(a);
                dbuf.BLongitude_Processed = dbuf.BLongitude;
            }
            else
            {
                dbuf.Longitude_Processed = dbuf.Longitude = double.NaN;
                dbuf.BLongitude_Processed = dbuf.BLongitude = NaNBytes;
            }
            index += 4;

            //Altitude
            if (state == 1)
            {
                dbuf.BAltitude[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BAltitude[i] = data[index + i]; };
                dbuf.Altitude_Processed = dbuf.Altitude = formatFloat(a);
                dbuf.BAltitude_Processed = dbuf.BAltitude;
            }
            else
            {
                dbuf.Altitude_Processed = dbuf.Altitude = double.NaN;
                dbuf.BAltitude_Processed = dbuf.BAltitude = NaNBytes;
            }
            index += 4;

            // Vx
            if (state == 1)
            {
                dbuf.BVx[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVx[i] = data[index + i]; };
                dbuf.Vx_Processed = dbuf.Vx = formatFloat(a);
                dbuf.BVx_Processed = dbuf.BVx;
            }
            else
            {
                dbuf.Vx_Processed = dbuf.Vx = double.NaN;
                dbuf.BVx_Processed = dbuf.BVx = NaNBytes;
            }
            index += 4;

            // Vy
            if (state == 1)
            {
                dbuf.BVy[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVy[i] = data[index + i]; };
                dbuf.Vy_Processed = dbuf.Vy = formatFloat(a);
                dbuf.BVy_Processed = dbuf.BVy;
            }
            else
            {
                dbuf.Vy_Processed = dbuf.Vy = double.NaN;
                dbuf.BVy_Processed = dbuf.BVy = NaNBytes;
            }
            index += 4;

            // Vz
            if (state == 1)
            {
                dbuf.BVz[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BVz[i] = data[index + i]; };
                dbuf.Vz_Processed = dbuf.Vz = formatFloat(a);
                dbuf.BVz_Processed = dbuf.BVz;
            }
            else
            {
                dbuf.Vz_Processed = dbuf.Vz = double.NaN;
                dbuf.BVz_Processed = dbuf.BVz = NaNBytes;
            }
            index += 4;

            //Ax acceleration

                dbuf.Ax = double.NaN;
                dbuf.BAx = NaNBytes;

            //Ay acceleration

                dbuf.Ay = double.NaN;
                dbuf.BAy = NaNBytes;

            //Az acceleration

                dbuf.Az = double.NaN;
                dbuf.BAz = NaNBytes;

            //V, V_Processed, A
            if (state == 1)
            {
                dbuf.V_Processed = dbuf.V = (Math.Sqrt(Math.Pow(dbuf.Vx, 2) + Math.Pow(dbuf.Vy, 2) + Math.Pow(dbuf.Vz, 2)));
                var Unformatted = unformatFloat(dbuf.V_Processed);
                for (int i = 0; i < 4; i++) { dbuf.BV_Processed[i] = (byte)((int) Unformatted % 256); Unformatted /= 256; }
                //dbuf.BV_Processed = dbuf.BV = CopyByteWithOffset(BitConverter.GetBytes(dbuf.V), 4);
                dbuf.A = double.NaN;
                dbuf.BA = NaNBytes;
            }
            else
            {
                dbuf.V = double.NaN;
                dbuf.V_Processed = double.NaN;
                dbuf.A = double.NaN;
                dbuf.BV = NaNBytes;
                dbuf.BV_Processed = NaNBytes;
                dbuf.BA = NaNBytes;
            }

            // DOP
            if (state == 1)
            {
                dbuf.BPDOP[3] = data[index + 3];
                a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.BPDOP[i] = data[index + i]; }
                dbuf.PDOP = (formatFloat(a));
            }
            else
            {
                dbuf.PDOP = double.NaN;
                dbuf.BPDOP = NaNBytes;
            }

            index += 4;

            int readSNR = 0,GPSCount = 0,snr = -1;
            bool highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS[0][i].SNR = 0;

                if (GPS[0][i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        snr = (data[index] & 0xF) + 40;
                        GPS[0][i].SNR = (double)snr;
                        dbuf.BGPSstat[0][GPSCount++] = (byte)snr;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        snr = ((data[index] & 0xF0) >> 4) + 40;
                        GPS[0][i].SNR = (double)snr;
                        dbuf.BGPSstat[0][GPSCount++] = (byte)snr;
                    }
                }
            }
            index += 8 - readSNR;

            readSNR = 0;
            GPSCount = 0;
            snr = -1;
            highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS[1][i].SNR = 0;

                if (GPS[1][i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        snr = (data[index] & 0xF) + 40;
                        GPS[1][i].SNR = (double)snr;
                        dbuf.BGPSstat[1][GPSCount++] = (byte)snr;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        snr = ((data[index] & 0xF0) >> 4) + 40;
                        GPS[1][i].SNR = (double)snr;
                        dbuf.BGPSstat[1][GPSCount++] = (byte)snr;
                    }
                }
            }
            index += 8 - readSNR;




            a = data[index] + data[index + 1] * 256;
            dbuf.Bdatetime[0] = data[index];
            dbuf.Bdatetime[1] = data[index + 1];
            var weekNumber = (int)a;
            index += 2;

            //UTC Offset
            index += 2;

            var datetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            datetimeUTC = datetimeUTC.AddDays(weekNumber * 7);
            datetimeUTC = datetimeUTC.AddMilliseconds(TOW);

            dbuf.datetime = datetimeUTC;
            PTime = datetimeUTC;

            ////
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int packetDelay = (int)a;
            index += 4;

            // Checksum
            index += 2;

            // Trailer
            return dbuf;
            // Update_Controls(dbuf, state, TOW, serialNum, HDOP, VDOP);
        }

        public static void Process_Binary_Message_Licence(byte[] data, int serialNum,List<byte[]> licenses)
        {
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_LICENCE_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);


            int index = 1;      //header
            index++;        //messageType

            int licenseNum = data[index];
            index++;

            for (int i = 0; i < 16; i++)
            {
                licenses[licenseNum][i] = data[index];
                index++;
            }

            checksum = 0;
            for (int i = 0; i < 14; i++)
                checksum += licenses[licenseNum][i];

            checksum0 = (byte)((checksum >> 8) & 0xFF);
            checksum1 = (byte)(checksum & 0xFF);

            int licenseType = (licenses[licenseNum][0] << 8) | licenses[licenseNum][1];
            int licenseValue = (licenses[licenseNum][2] << 32) | (licenses[licenseNum][3] << 16)
                                | (licenses[licenseNum][4] << 8) | licenses[licenseNum][5];


           /* if (licenseType == 1)
            {
                listBoxLicence.Items.Add("License " + licenseNum.ToString() + " - Speed");
                listBoxLicenceProperties.Items[0] = "Max Speed: " + licenseValue.ToString() + " m/s";
            }
            else if (licenseType == 2)
            {
                listBoxLicence.Items.Add("License " + licenseNum.ToString() + " - Acceleration");
                listBoxLicenceProperties.Items[1] = "Max Acceleration: " + licenseValue.ToString() + " m/s2";
            }
            else if (licenseType == 3)
            {
                listBoxLicence.Items.Add("License " + licenseNum.ToString() + " - Height");
                listBoxLicenceProperties.Items[2] = "Max Height: " + licenseValue.ToString() + " km";
            }
            else if (licenseType == 4)
            {
                listBoxLicence.Items.Add("License " + licenseNum.ToString() + " - Refresh Rate");
                listBoxLicenceProperties.Items[3] = "Max Refresh Rate: " + licenseValue.ToString();
            }*/
        }

        public static SingleDataBuffer Process_Binary_Message_Setting(byte[] data, int serialNum)
        {
            SingleDataBuffer dbuf = new SingleDataBuffer();
            dbuf.settingbuffer.SettingReceived = true;
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_SETTING_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);

            if (checksum0 != data[BIN_SETTING_MSG_SIZE - 3] || checksum1 != data[BIN_SETTING_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");
            }

            int index = 1;      //header
            index++;        //messageType

            //Sat Type
            int SatType = data[index];
            //dbuf.settingbuffer.
            index++;

            //Number of satellites
            dbuf.settingbuffer.SatNum = data[index];
            index++;
            dbuf.settingbuffer.GPSNum = data[index];
            index++;
            dbuf.settingbuffer.GLONASSNum = data[index];
            index++;
            dbuf.settingbuffer.GalileoNum = data[index];
            index++;
            dbuf.settingbuffer.CompassNum = data[index];
            index++;

            //Pos Type
            dbuf.settingbuffer.PosType = data[index] - 1;
            //radDDLPosTyp.SelectedIndex = data[index] - 1;
            index++;

            // if (radDDLSelectSerial.FindStringExact(radDDLSelectSerial.Text) == 1)
            //    index += 5;

            int baudRate = data[index];
            index++;
            baudRate = (baudRate << 8) + data[index];
            index++;
            baudRate = (baudRate << 8) + data[index];
            index++;
            dbuf.settingbuffer.BaudRate = baudRate;

            int PacketType = data[index];
            index++;
            PacketType = (PacketType << 8) + data[index];
            index++;
            dbuf.settingbuffer.PacketType = PacketType;


            //if (radDDLSelectSerial.FindStringExact(radDDLSelectSerial.Text) == 0)
            //    index += 5;

            //radDDLRefreshRate.Text = data[index].ToString();
            dbuf.settingbuffer.RefreshRate = data[index];
            index++;

            int PDOP_Threshold = data[index];
            index++;
            PDOP_Threshold = (PDOP_Threshold << 8) + data[index];
            index++;
            dbuf.settingbuffer.PDOPTh = PDOP_Threshold;
            //textEditPdopTHD.Text = PDOP_Threshold.ToString();

            //GPS Use Threshold
            //textEditGpsUseTHD.Text = (data[index] / 4).ToString();
            dbuf.settingbuffer.GPSUseTh = (data[index] / 4);
            index++;

            //GLONASS Use Threshold
            //textEditGlonassTHD.Text = (data[index] / 4).ToString();
            dbuf.settingbuffer.GLONASSUseTh = (data[index] / 4);
            index++;

            //GPS Deassign Threshold
            //textEditGpsDeassTHD.Text = (data[index] / 4).ToString();
            dbuf.settingbuffer.GPSDisTh = data[index] / 4;
            index++;

            //GLONASS Deassign Threshold
            //textEditGlonassDeassTHD.Text = (data[index] / 4).ToString();
            dbuf.settingbuffer.GLONASSDisTh = data[index] / 4;
            index++;

            //Reliability Deassign Threshold
            //textEditReliabilityTHD.Text = (data[index] / 100).ToString();
            dbuf.settingbuffer.RelyDisTh = data[index] / 100;
            index++;

            //Satellite Distance Error Threshold
            int SatDistErrTHD = data[index];
            index++;
            SatDistErrTHD = ((SatDistErrTHD << 8) + data[index]) / 10;
            index++;
            dbuf.settingbuffer.SatDisErrTh = SatDistErrTHD;
            //textEditSatDistErrTHD.Text = SatDistErrTHD.ToString();

            int MAX_Speed = data[index];
            index++;
            MAX_Speed = (MAX_Speed << 8) + data[index];
            index++;
            dbuf.settingbuffer.MaxSpeed = MAX_Speed;
            //textEditMaxSpeed.Text = MAX_Speed.ToString();

            int MAX_Acceleration = data[index];
            index++;
            MAX_Acceleration = ((MAX_Acceleration << 8) + data[index]) / 10;
            index++;
            dbuf.settingbuffer.MaxAcc = MAX_Acceleration;
            //textEditMaxAcc.Text = MAX_Acceleration.ToString();

            //Mask Angle
            //textEditMaskAngle.Text = data[index].ToString();
            dbuf.settingbuffer.MaskAngle = data[index];
            index++;

            //Green Satellite Type
            // radDDLGreenType.SelectedIndex = data[index];
            dbuf.settingbuffer.GreenSatType = data[index];
            index++;

            //Tropospheric Correction
            //radDDLTropo.SelectedIndex = data[index];
            dbuf.settingbuffer.TropoCor = data[index];
            index++;

            //Automatic Max Angle Attitude
            //radDDLAutoMaxAngle.SelectedIndex = data[index];
            dbuf.settingbuffer.AutoMaxAngle = data[index];
            index++;

            //Ionospheric Correction
            //radDDLIonospheric.SelectedIndex = data[index];
            dbuf.settingbuffer.IonoCor = data[index];
            index++;
            return dbuf;
        }

        public static SingleDataBuffer Process_Binary_Message_Attitude_Info(byte[] data, int serialNum)
        {
            SingleDataBuffer dbuf = new SingleDataBuffer();
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_ATTITUDE_INFO_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);
            if (checksum0 != data[BIN_ATTITUDE_INFO_MSG_SIZE - 3] || checksum1 != data[BIN_ATTITUDE_INFO_MSG_SIZE - 2])
            {
                throw new Exception("Checksum Error");                
            }

            dbuf.AttitudeBuffer.isNorthfinder = true;

            int index = 1;      //header
            index++;        //messageType

            int state = data[index];
            
            if (state == 1)
                dbuf.statcounter = 1;
            else
                dbuf.statcounter = -1;
            index++;

            var attitudeState = data[index];
            index++;        //Attitude Determination State;
            dbuf.AttitudeBuffer.AttitudeState = attitudeState;

            // Azimuth (yaw)
            dbuf.AttitudeBuffer.BAzimuth[3] = data[index + 3];
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.AttitudeBuffer.BAzimuth[i] = data[index + i]; }
            dbuf.AttitudeBuffer.Azimuth = formatFloat(a);
            //var buffercounter = attitudeInfoDataBuf.Azimuth.Count - 1;
            index += 4;

            index += 4;     //roll

            // Elevation (pitch)
            dbuf.AttitudeBuffer.BElevation[3] = data[index + 3];
            a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; dbuf.AttitudeBuffer.BElevation[i] = data[index + i]; }
            dbuf.AttitudeBuffer.Elevation = formatFloat(a);
            //attitudeInfoDataBuf.Elevation.Add(formatFloat(a));
            index += 4;

            double x = 0, y = 0, z = 0;
            //GEOpoint point;

            //x
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                x = formatFloat(a);
                dbuf.AttitudeBuffer.X = x;
                //attitudeInfoDataBuf.X.Add(x);
            }
            index += 4;

            //y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                y = formatFloat(a);
                dbuf.AttitudeBuffer.Y = y;
                //attitudeInfoDataBuf.Y.Add(y);
            }
            index += 4;

            //z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                z = formatFloat(a);
                dbuf.AttitudeBuffer.Z = z;
                //attitudeInfoDataBuf.Z.Add(z);
            }
            index += 4;

            //TOW
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            a = data[index] + data[index + 1] * 256;
            int WeekNumber = (int)a;
            index += 2;

            a = data[index] + data[index + 1] * 256;
            int ambiguity = (int)a;
            dbuf.AttitudeBuffer.Ambiguity = ambiguity;
            index += 2;

            DateTime DatetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            DatetimeUTC = DatetimeUTC.AddDays(WeekNumber * 7);
            DatetimeUTC = DatetimeUTC.AddMilliseconds(TOW);

            dbuf.datetime = DatetimeUTC;

            //attitudeInfoDataBuf.datetime.Add(DatetimeUTC);

            if (state == 1)
            {

                //Distance
                double distance = Math.Sqrt(Math.Pow(dbuf.AttitudeBuffer.X, 2) + Math.Pow(dbuf.AttitudeBuffer.Y, 2) + Math.Pow(dbuf.AttitudeBuffer.Z, 2));
                dbuf.AttitudeBuffer.Distance = distance;
                var unformatted = unformatFloat(distance);
                for (int i = 0; i < 4; i++) { dbuf.AttitudeBuffer.BDistance[i] = (byte)((int)unformatted % 256); unformatted /= 256; }

                //attitudeInfoDataBuf.Distance[statcounter] = distance;
            }
            else
            {
                /*if (AmbiguityControlUpdate)
                {
                    radLabelAmbiguity.Text = "Remaining Ambiguity: ?";
                    AmbiguityControlUpdate = false;
                }*/

                //if (attitudeInfoDataBuf.counter == 0)
                //    return;

            }

            dbuf.AttitudeBuffer.counter++;
            //attitudeInfoDataBuf.counter++;

            return dbuf;
        }

        public static SingleDataBuffer Process_Binary_Message_BaseStation_Info(byte[] data, int serialNum)
        {
            SingleDataBuffer dbuf = new SingleDataBuffer();
            BaseStationInfo baseStationInfo = new BaseStationInfo();

            int checksum = calcrc(data, BIN_BASESTATION_INFO_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            Int64 a, b;

            int index = 1;      //header
            index++;    //messageType

            baseStationInfo.stationNumber = data[index];
            index++;

            // State
            baseStationInfo.voltage = data[index] / 10.0;
            index++;

            baseStationInfo.current = data[index] / 20.0;
            index++;

            baseStationInfo.stability = Math.Pow(10, -data[index] / 10.0);
            index++;

            baseStationInfo.temperature = data[index];
            index++;

            baseStationInfo.humidity = data[index] / 2.0;
            index++;

            baseStationInfo.batteryCharge = data[index] / 2.0;
            index++;

            baseStationInfo.lastLocktoGPS = data[index];
            index++;

            baseStationInfo.statusFlag = data[index];
            index++;

            //Mac Address
            baseStationInfo.stationMACNumber = data[index];
            index++;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.x = a / 100.0;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.y = a / 100.0;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.z = a / 100.0;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.weekNumber = (int)Math.Round(a / 7.0 / 24.0 / 3600.0 / 10.0);
            baseStationInfo.timeofweek = a / 10.0 - baseStationInfo.weekNumber * 7.0 * 24.0 * 3600.0;
            index += 4;



            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.assignedFlyingObject = (int)a;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            baseStationInfo.readyFlyingObject = (int)a;
            index += 4;

            a = (data[index] & 0x7F);
            b = (data[index + 1] & 0x07) * 2 + (data[index] >> 7);
            switch (b)
            {
                case 0x7:
                    b = 0;
                    break;
                case 0xA:
                    b = 24;
                    break;
                case 0x9:
                    b = 45;
                    break;
                case 0x8:
                    b = 63;
                    break;
                case 0x4:
                    b = 90;
                    break;
                case 0:
                    b = 105;
                    break;
                default:
                    b = 0;
                    break;
            }

            baseStationInfo.rssiBaseStation = (int)(-120 + ((a + b) * 0.5));
            index += 2;

            a = (data[index] & 0x7F);
            b = (data[index + 1] & 0x07) * 2 + (data[index] >> 7);
            switch (b)
            {
                case 0x7:
                    b = 0;
                    break;
                case 0xA:
                    b = 24;
                    break;
                case 0x9:
                    b = 45;
                    break;
                case 0x8:
                    b = 63;
                    break;
                case 0x4:
                    b = 90;
                    break;
                case 0:
                    b = 105;
                    break;
                default:
                    b = 0;
                    break;
            }

            baseStationInfo.rssiCenterStation = (int)(-120.0 + (a + b) * 0.5);
            index += 2;

            for (int i = 0; i < TOTAL_FLYING_OBJECTS_COUNT; ++i)
            {
                baseStationInfo.SNRs[i] = data[index];
                index++;
            }

            baseStationInfo.ltrHealth = data[index];
            index++;

            int crc = 0;
            ushort tmp = 0;
            a = data[index+1]; for (int i = 0; i >= 0; --i) a = a * 256 + data[index + i];
            crc = (int)a;
            index += 2;

            for (int ii = 0; ii < index; ii++)
                tmp += data[ii];

           // if (tmp != crc)
           //     throw new Exception("CRC error");
            dbuf.hasBaseStationInfo = true;
            dbuf.BaseStationBuffer = baseStationInfo;

            return dbuf;
        }

        public static SingleDataBuffer handle_packet(byte[] packet,ref Globals vars, int number)
        {

            var key = packet[1];
            if (key == Functions.BIN_FULL)
                dbuffer = Process_Binary_Message_Full(packet, number, ref vars.GPSlist, ref vars.GLONASSlist, ref vars.PacketTime);
            else if (key == Functions.BIN_FULL_PLUS)
                dbuffer = Process_Binary_Message_Full(packet, number, ref vars.GPSlist, ref vars.GLONASSlist, ref vars.PacketTime);
            else if (key == Functions.BIN_COMPACT)
                dbuffer = Process_Binary_Message_Compact(packet, number, ref vars.GPSlist, ref vars.GLONASSlist, ref vars.PacketTime);
            else if (key == Functions.BIN_SETTING)
                dbuffer = Process_Binary_Message_Setting(packet, number);
            else if (key == Functions.BIN_DUAL_CHANNEL)
                dbuffer = Process_Binary_Message_Dual_Channel(packet, number, ref vars.GPSlist, ref vars.GLONASSlist, ref vars.PacketTime);
            else if (key == Functions.BIN_RAW_DATA)
                dbuffer = Process_Binary_Message_RawData(packet, number);
            else if (key == Functions.BIN_GPS_SUPPLEMENT)
                Process_Binary_Message_SupplementGPS(packet, number, ref vars.GPSlist);
            else if (key == Functions.BIN_GLONASS_SUPPLEMENT)
                Process_Binary_Message_SupplementGLONASS(packet, number, ref vars.GLONASSlist);
            else if (key == Functions.BIN_ATTITUDE_INFO)
                dbuffer = Process_Binary_Message_Attitude_Info(packet, number);
            else if (key == BIN_BASESTATION_INFO)
                dbuffer =  Process_Binary_Message_BaseStation_Info(packet, number);
            else if (key == BIN_KEEP_ALIVE)
            {
                dbuffer = new SingleDataBuffer();
                dbuffer.isAlive = true;
                //if (serialNum == 0)
                //    DataReceiveTimeOut = 0;
                //else
                //    DataReceiveTimeOutS2 = 0;
            }
            else if (key == BIN_ACK_SIGNAL)
            {
                dbuffer = new SingleDataBuffer();
                dbuffer.AckSignalReceived = true;
            }
            else
            {
                dbuffer.ToString();
            }

            return dbuffer;
        }

        public static void handle_NMEA(List<string> fields, ref Globals vars, ref SingleDataBuffer Data)
        {
            TimeSpan dt;
            int weeknum;
            double TOW;
            switch (fields[0])
            {
                case "$GPGGA":  //positioning system fix data
                    Data.Latitude = StringToFloatNMEA(fields[2]);
                    Data.BLatitude = doubletoByte(Data.Latitude, 4);
                    Data.Longitude = StringToFloatNMEA(fields[4]);
                    Data.BLongitude = doubletoByte(Data.Longitude, 4);
                    Data.NumOfUsedSats = StringToFloatNMEA(fields[7]);
                    Data.BNumOfUsedStats = doubletoByte(Data.NumOfUsedSats, 4, false);
                    Data.Altitude = StringToFloatNMEA(fields[9]);
                    Data.BAltitude = doubletoByte(Data.Altitude, 4);
                    break;

                case "$GPVTG":  //SPEED
                    Data.V = StringToFloatNMEA(fields[7]);// * 1000;
                    Data.BV = doubletoByte(Data.V, 4);
                    break;
                case "$GPGSA":  //PDOP
                    #region GSA
                    /*if (vars.GPSlist.Count < 1)
                    {
                        List<Satellite[]> tempsat = new List<Satellite[]>();
                        for (int i = 0; i < 2; i++)
                        {
                            tempsat.Add(new Satellite[32]);
                            for (int j = 0; j < 32; j++)
                            {
                                tempsat[i][j] = new Satellite();
                            }
                            if (i % 2 == 0)
                                vars.GPSlist.Add(tempsat[i]);
                            else
                                vars.GLONASSlist.Add(tempsat[i]);
                        }                        
                    }*/
                    Data.NumOfUsedSats = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        int index = (int)StringToFloatNMEA(fields[3 + i]);
                        if (fields[3+i].Length > 0)
                        {
                            Data.NumOfUsedSats++;
                        }
                    }
                    Data.BNumOfUsedStats = doubletoByte(Data.NumOfUsedSats, 4,false);
                    Data.PDOP = StringToFloatNMEA(fields[15]);
                    Data.BPDOP = doubletoByte(Data.PDOP, 4);
                    Data.HDOP = StringToFloatNMEA(fields[16]);                    
                    Data.VDOP = StringToFloatNMEA(fields[17]);
#endregion
                    break;
                case "$GPZDA":  //Time
                    DateTime t = TimeFromStrings(fields[1],fields[2],fields[3],fields[4]);
                    Data.datetime = t;
                    dt = Data.datetime - new DateTime(1980, 1, 6, 0, 0, 0);
                    weeknum = (int)Math.Floor((double)dt.Days / 7);
                    TOW = dt.TotalMilliseconds - (double)weeknum * 604800000;
                    Data.Bdatetime = new byte[6];
                    Data.Bdatetime[0] = (byte)(weeknum % 256);
                    Data.Bdatetime[1] = (byte)(weeknum / 256);
                    for (int i = 0; i < 4; i++)
                    {
                        Data.Bdatetime[2 + i] = (byte)(TOW % 256);
                        TOW /= 256;
                    }
                    vars.PacketTime = t;
                    break;
                case "$GPGLL":  //Latitude and longitude, with time of position fix and status
                    Data.Latitude = StringToFloatNMEA(fields[1]);
                    Data.BLatitude = doubletoByte(Data.Latitude,4);
                    Data.Longitude = StringToFloatNMEA(fields[3]);
                    Data.BLongitude = doubletoByte(Data.Longitude,4);
                    Data.datetime = TimeFromStrings(fields[5]);
                    dt = Data.datetime - new DateTime(1980, 1, 6, 0, 0, 0);
                    weeknum = (int)Math.Floor((double)dt.Days / 7);
                    TOW = dt.TotalMilliseconds - (double)weeknum * 604800000;
                    Data.Bdatetime = new byte[6];
                    Data.Bdatetime[0] = (byte)(weeknum % 256);
                    Data.Bdatetime[1] = (byte)(weeknum / 256);
                    for (int i = 0; i < 4; i++)
                    {
                        Data.Bdatetime[2 + i] = (byte)(TOW % 256);
                        TOW /= 256;
                    }
                    vars.PacketTime = Data.datetime;
                    break;

            }
            
        }

        public static byte[] CopyByteWithOffset(byte[] input, int offset)
        {
            byte[] output = new byte[input.Length - offset];
            for (int i = offset; i < input.Length; i++)
                output[i - offset] = input[i];
            return output;
        }

        public static double[] FindMaxes(List<double> input)
        {
            double[] output = new double[Globals.Databuffercount];
            float dt = (float)input.Count / 100;
            float dto = (float)output.Length / 100;
            for (int i = 0; i < 100; i++)
            {
                var firstdata = (int)(i * dt);
                var rmax = input[firstdata];
                for (int j = 1; j < (int)dt; j++)
                    if (input[(int)(i * dt) + j] != double.NaN)
                        if (rmax == double.NaN)
                            rmax = input[(int)(i * dt) + j];
                        else if (input[(int)(i * dt) + j] > rmax)
                            rmax = input[(int)(i * dt) + j];
                for (int j = 0; j < (int)dto; j++)
                    output[(int)(i * dto) + j] = rmax;
            }

            return output;
        }

        public static double[] FindMins(List<double> input)
        {
            double[] output = new double[Globals.Databuffercount];
            float dt = (float)input.Count / 100;
            float dto = (float)output.Length / 100;
            for (int i = 0; i < 100; i++)
            {
                var firstdata = (int)(i * dt);
                var rmin = input[firstdata];
                for (int j = 1; j < (int)dt; j++)
                    if (input[(int)(i*dt) + j] != double.NaN)
                        if (rmin == double.NaN)
                            rmin = input[(int)(i * dt) + j];
                        else if (input[(int)(i * dt) + j] < rmin)
                            rmin = input[(int)(i * dt) + j];
                for (int j = 0; j < (int)(dto); j++)
                    output[(int)(i * dto) + j] = rmin;
            }
            return output;
        }

        public static long QuantizePosition(float Adress)
        {
            var mod = (int)(Adress % 4);
            mod = (mod < 3) ? -mod : 4 - mod;
            return (long)(Adress + mod);
        }
        public static long QuantizePosition6bit(float Adress)
        {
            var mod = (int)(Adress % 6);
            mod = (mod < 4) ? -mod : 6 - mod;
            return (long)(Adress + mod);
        }
        public static long QuantizePosition8bit(float Adress)
        {
            var mod = (int)(Adress % 8);
            mod = (mod < 5) ? -mod : 8 - mod;
            return (long)(Adress + mod);
        }
        public static long QuantizePosition12bit(float Adress)
        {
            var mod = (int)(Adress % 12);
            mod = (mod < 7) ? -mod : 12 - mod;
            return (long)(Adress + mod);
        }
        public static long QuantizePosition17bit(float Adress)
        {
            var mod = (int)(Adress % 17);
            mod = (mod < 8) ? -mod : 17 - mod;
            return (long)(Adress + mod);
        }

        public static DateTime ReadDateTime(byte[] input)
        {
            var a = input[0] + input[1] * 256;
            var weekNumber = (int)a;

            a = input[5]; for (int i = 2; i >= 0; --i) { a = a * 256 + input[2 + i]; }
            var localTOW = (int)a;
            if (weekNumber > 65530)
                return new DateTime();
            var datetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            datetimeUTC = datetimeUTC.AddDays(weekNumber * 7);
            datetimeUTC = datetimeUTC.AddMilliseconds(localTOW);

            return datetimeUTC;

        }

        public static char MakeTimeIntervalByte(int interval,DelayRange range)
        {
            char output = (char)0;
            switch (range)
            {
                case DelayRange.ms:
                    if (interval >= 5000)
                    {
                        output = MakeTimeIntervalByte(interval / 1000, DelayRange.s);
                        break;
                    }
                    else if (interval < 250)
                    {
                        output = (char)0;
                        break;
                    }
                    output = (char)((interval / 250) - 1);
                    //    0 <= output < 19
                    break;

                case DelayRange.s:
                    if (interval >= 120)
                    {
                        output = MakeTimeIntervalByte(interval / 60, DelayRange.m);
                        break;
                    }
                    else if (interval < 1)
                    {
                        output = (char)3;
                        break;
                    }
                    output = (char)(interval + 18);
                    //    19 <= output < 138
                    break;

                case DelayRange.m:
                    if (interval >= 90)
                    {
                        output = MakeTimeIntervalByte(interval / 60, DelayRange.h);
                        break;
                    }
                    else if (interval < 1)
                    {
                        output = (char)78;
                        break;
                    }
                    output = (char)(interval + 137);
                    //    138 <= output < 227
                    break;

                case DelayRange.h:
                    if (interval > 29)
                        interval = 29;
                    else if (interval < 1)
                        interval = 1;

                    output = (char)(interval + 226);
                    //    227 <= output < 255
                    break;

            }
            /*if (interval < 250)
                output = (char)0;
            else if (interval < 10200)
            {
                //error = 100ms
                int f = (int)Math.Floor((double)(interval - 200) / 100);
                output = (char)f;
                //output < 100
            }
            else
            {
                //error = 1s;
                int sec = (int)Math.Floor((double)(interval - 10000) / 1000);
                if (sec > 155)
                    sec = 155;
                output = (char)(sec + 100);

            }*/
            return output;
        }


        public static int crc32b(char[] message, int length, int seed)
        {
            int i, j;
            uint Byte, crc, mask;
            i = 0;
            crc = ~(uint)seed;
            for (i = 0; i < length; i++)
            {
                Byte = message[i];            // Get next byte.
                crc = crc ^ Byte;
                for (j = 7; j >= 0; j--)
                {    // Do eight times.
                    mask = (uint)(-(crc & 1));
                    crc = (uint)((crc >> 1) ^ (0xEDB88320 & mask));
                }
            }
            return (int)(~crc);
        }

        public static uint crc32b(char[] message, int length, uint seed)
        {
            int i, j;
            uint Byte, crc, mask;
            i = 0;
            crc = ~seed;
            for (i = 0; i < length; i++)
            {
                Byte = message[i];            // Get next byte.
                crc = crc ^ Byte;
                for (j = 7; j >= 0; j--)
                {    // Do eight times.
                    mask = (uint)(-(crc & 1));
                    crc = (uint)((crc >> 1) ^ (0xEDB88320 & mask));
                }
            }
            return ~crc;
        }


        public static int crc32b(byte[] message, int length, int seed)
        {
            int i, j;
            uint Byte, crc, mask;
            i = 0;
            crc = ~(uint)seed;
            for (i = 0; i < length; i++)
            {
                Byte = message[i];            // Get next byte.
                crc = crc ^ Byte;
                for (j = 7; j >= 0; j--)
                {    // Do eight times.
                    mask = (uint)(-(crc & 1));
                    crc = (uint)((crc >> 1) ^ (0xEDB88320 & mask));
                }
            }
            return (int)(~crc);
        }

        public static int hexToDecimal(char[] hex,int offset, int length)
        {
	        int integer = 0;
            int pow = 1;
	        for (int i=length-1; i>=0; i--)
	        {
		        char hexDigit = hexDigitToInt(hex[offset+i]);
                if (hexDigit != '#')
                {
                    integer += hexDigit * pow;
                    pow *= 16;
                    //integer <<= 4;
                    //integer |= hexDigit;
                }
                else
                    continue;
	        }
	        return integer;
        }

        public static char hexDigitToInt(char digit)
        {
            char result = '#';
            switch (digit)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    result = (char)(digit - 48);
                    break;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                    result = (char)(digit - 87);
                    break;
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                    result = (char)(digit - 55);
                    break;
                default:
                    break;
            }
            return result;
        }

        public static byte[] ChartoByte(char[] input)
        {
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
                output[i] = (byte)input[i];
            return output;
        }

        public static int CharToInt(char[] input)
        {
            int temp = 0;
            for (int i = input.Length -1; i >= 0; i--)
            {
                temp *= 256;
                temp += (int)input[i];
            }
            return temp;
        }

        public static int ByteToInt(byte[] input)
        {
            int temp = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                temp *= 256;
                temp += (int)input[i];
            }
            return temp;
        }

        public static float StringToFloatNMEA(string input)
        {
            char[] inp = input.ToCharArray();
            bool negative = false;
            if (inp.Length == 0)
                return 0;
            if (inp[0] == '-')
            {
                negative = true;
                inp = input.ToCharArray(1, input.Length - 1);
            }
            float temp = 0;
            int flag = inp.Length;
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i] == '.')
                {
                    flag = i;
                    continue;
                }
                temp += (int)inp[i] - (int)'0';
                if (i != inp.Length-1)
                    temp *= 10;
            }
            for (int i = 0; i < inp.Length - flag - 1; i++)
                temp /= 10;
            if (negative)
                temp *= -1;
            return temp;
        }

        public static DateTime TimeFromStrings(string Time, string Day, string Month, string Year)
        {
            int timeint = (int)StringToFloatNMEA(Time);
            int sec,minute,hour;
            sec = timeint %100;
            timeint/=100;
            minute = timeint %100;
            //timeint/=100;
            hour =timeint/100;
            int dayint = (int)StringToFloatNMEA(Day);
            int monthint = (int)StringToFloatNMEA(Month);
            int yearint = (int)StringToFloatNMEA(Year);
            DateTime t = new DateTime(yearint, monthint, dayint, hour, minute, sec);
            return t;
        }
        public static DateTime TimeFromStrings(string Time)
        {
            int timeint = (int)StringToFloatNMEA(Time);
            int sec, minute, hour;
            sec = timeint % 100;
            timeint /= 100;
            minute = timeint % 100;
            //timeint/=100;
            hour = timeint / 100;
            DateTime t = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, sec);
            return t;
        }

        public static byte[] doubletoByte(double input, int bytes, bool Unformat = true)
        {
            byte[] output = new byte[bytes];
            Int64 temp = (Int64)input;
            if (Unformat)
                temp = unformatFloat(input);

            for (int i = 0; i < bytes; i++)
            {
                output[i] = (byte)(temp % 256);
                temp /= 256;
            }

            //buffer.BPDOP[3] = data[index + 3];
            //a = data[index + 3]; for (int i = 2; i >= 0; --i) { a = a * 256 + data[index + i]; buffer.BPDOP[i] = data[index + i]; }
            return output;
        }
    }
}
