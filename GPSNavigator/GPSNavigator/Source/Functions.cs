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

        private const double Elevation_OutOfRange = 91;
        private const double Azimuth_OutOfRange = 361;


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

        private static double formatFloat(Int64 a)
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

        private static double formatDouble(Int64 a)
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

        public static void Process_Packet(string Data,List<string> field)             //Process Received Data
        {
            //Calculate Checksum
            if (Checksum_validity(Data) != true)
            {
                //("CheckSum Error", "Recieved checksum is incorrect");
                return;
            }

            //DataReceiveTimeOut = 0;

            Calculate_NMEA_fields(Data,field);

            string Sentence = field[0].Substring(3, 3);


           
            //Process Sentences
            /*switch (Sentence)
            {
                case "GGA":                 //Global positioning system fix data
                    Process_GGA_Message();
                    break;
                case "GSA":                 //GPS DOP and Active Satellites
                    Process_GSA_Message();
                    break;
                case "GSV":                 //GPS Satellites in View
                    Process_GSV_Message();
                    break;
                case "SV1":                 //GPS Antenna 1 (only for dual GPS)
                    Process_SV1_Message();
                    break;
                case "SV2":                 //GPS Antenna 2 (only for dual GPS)
                    Process_SV2_Message();
                    break;
                case "RMC":                 //Recommended Minimum data
                    Process_RMC_Message();
                    break;
            }*/
        }

        private static int Calculate_Checksum(string Data)
        {
            int Checksum = 0;

            for (int i = 1; i < Data.IndexOf('*'); i++)
                Checksum ^= (int)Data[i];

            return Checksum;
        }

        private static char Calculate_Checksum_Char(char[] Data, int num)
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

        private static void Calculate_NMEA_fields(string Data,List<string> field)
        {
            int field_Num = Data.Count(f => f == ',') + 2;       //Number of fields from begining to checksum

            int pos = 0;
            for (int i = 0; i < field_Num - 2; i++)
            {
                field.Add(Data.Substring(pos, Data.IndexOf(',', pos) - pos));
                pos = Data.IndexOf(',', pos) + 1;
            }
            field.Add(Data.Substring(Data.LastIndexOf(',') + 1, Data.IndexOf('*') - Data.LastIndexOf(',') - 1));
            field.Add(Data.Substring(Data.Length - 3, 2));              //Checksum
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
            else
                msgSize = -1;           //packet not valid

            return msgSize;
        }

        public static void Process_Binary_Message_Full(byte[] data, int SerialNum,ref DataBuffer buffer,List<Satellite> GPS,List<Satellite> GLONASS)
        {
           // DataBuffer dbuf;
           // List<Satellite> GPS_satellite;
            //List<Satellite> GLONASS_satellite;
            int buffercounter = -1, statcounter = -1;
           // if (SerialNum == 0)
            //{
               // dbuf = buffer;
               // GPS_satellite = GPS;
               // GLONASS_satellite = GLONASS;
            //}
          /*  else
            {
                dbuf = dataBuf2;
                GPS_satellite = GPSsatS2;
                GLONASS_satellite = GLONASSsatS2;
            }*/

            int msgSize = 0;

            if (data[1] == BIN_FULL)
                msgSize = BIN_FULL_MSG_SIZE;
            else if (data[1] == BIN_FULL_PLUS)
                msgSize = BIN_FULL_PLUS_MSG_SIZE;

            int checksum = calcrc(data, msgSize - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

           // if (checksum0 != data[msgSize - 3] || checksum1 != data[msgSize - 2])
            //{
                //Show_Error("CheckSum Error", "Recieved checksum is incorrect");
             //   return;
           // }

            int index = 1;      //header
            index++;    //messageType

            // State
            int state = data[index];
            buffer.state.Add(state);
            if (state == 1)
            {
                buffer.statcounter.Add(buffer.X.Count);
                statcounter = buffer.X.Count;
            }
            else
                buffer.statcounter.Add(-1);
            index++;

            // Temperature
            buffer.Temperature.Add(data[index]);
            buffercounter = buffer.Temperature.Count - 1;
            index++;

            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            buffer.NumOfVisibleSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GPS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;
            buffer.NumOfUsedSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GLONASS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.X.Add(formatFloat(a));
                //dbuf.X[dbuf.counter] = formatFloat(a);
            index += 4;

            // X Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.X_Processed.Add(formatFloat(a));
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Y.Add(formatFloat(a));
            index += 4;

            // Y Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Y_Processed.Add(formatFloat(a));
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Z.Add(formatFloat(a));
            index += 4;

            // Z Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Z_Processed.Add(formatFloat(a));
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Latitude.Add(formatFloat(a));
            index += 4;

            //Latitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Latitude = formatFloat(a);
                buffer.Latitude_Processed.Add(Latitude);
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Longitude.Add(formatFloat(a));
            index += 4;

            //Longitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Longitude = formatFloat(a);
                buffer.Longitude_Processed.Add(Longitude);
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Altitude.Add(formatFloat(a));
            index += 4;

            //Altitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Altitude = formatFloat(a);
                buffer.Altitude_Processed.Add(Altitude);
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vx.Add(formatFloat(a));
            index += 4;

            // Vx Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vx_Processed.Add(formatFloat(a));
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vy.Add(formatFloat(a));
            index += 4;

            // Vy Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vy_Processed.Add(formatFloat(a));
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vz.Add(formatFloat(a));
            index += 4;

            // Vz Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vz_Processed.Add(formatFloat(a));
            index += 4;

            //Ax acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Ax.Add(formatFloat(a));
            index += 4;

            //Ay acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Ay.Add(formatFloat(a));
            index += 4;

            //Az acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Az.Add(formatFloat(a));
            index += 4;

            //V, V_Processed, A
            if (state == 1)
            {
                buffer.V.Add(Math.Sqrt(Math.Pow(buffer.Vx[statcounter], 2) + Math.Pow(buffer.Vy[statcounter], 2) + Math.Pow(buffer.Vz[statcounter], 2)));
                buffer.V_Processed.Add(Math.Sqrt(Math.Pow(buffer.Vx_Processed[statcounter], 2) + Math.Pow(buffer.Vy_Processed[statcounter], 2) + Math.Pow(buffer.Vz_Processed[statcounter], 2)));
                buffer.A.Add(Math.Sqrt(Math.Pow(buffer.Ax[statcounter], 2) + Math.Pow(buffer.Ay[statcounter], 2) + Math.Pow(buffer.Az[statcounter], 2)));
            }

            //SNR GPS
            int readSNR = 0;
            for (int i = 0; i < 32; ++i)
            {
                GPS[i].SNR = 0;

                if (GPS[i].Signal_Status != 0)      //visible
                {
                    GPS[i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //GLONASS SNR
            readSNR = 0;
            for (int i = 0; i < 28; ++i)
            {
                GLONASS[i].SNR = 0;

                if (GLONASS[i].Signal_Status != 0)      //visible
                {
                    GLONASS[i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            a = data[index] + data[index + 1] * 256;
            //weekNumber = (int)a;
            index += 2;

            //UTC Offset
            index += 2;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int localTOW = (int)a;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int packetDelay = (int)a;
            index += 4;

            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;
            buffer.PDOP.Add(0);

            if (state == 1)
            {
                GDOP = Decompress_DOP(data[index]);
                index++;
                buffer.PDOP[buffer.PDOP.Count-1] = Decompress_DOP(data[index]);
                index++;
                HDOP = Decompress_DOP(data[index]);
                index++;
                VDOP = Decompress_DOP(data[index]);
                index++;
                TDOP = Decompress_DOP(data[index]);
                index++;
            }

            // Checksum
            index += 2;

            // Trailer

            /*
            if (SNRControlUpdate && radioGroupDevice.SelectedIndex == SerialNum)
            {
                Update_SNR_Chart(chartCtrlSNRs_Ant1, GPS_satellite);
                Update_SNR_Chart(chartCtrlSNRs_Ant1, GLONASS_satellite);

                SNRControlUpdate = false;
            }

            Update_Controls(dbuf, state, TOW, SerialNum, HDOP, VDOP);
             * */
        }

        public static SingleDataBuffer Process_Binary_Message_Full(byte[] data, int SerialNum, List<Satellite> GPS, List<Satellite> GLONASS)
        {
            SingleDataBuffer buffer = new SingleDataBuffer();
            // List<Satellite> GPS_satellite;
            //List<Satellite> GLONASS_satellite;
            // if (SerialNum == 0)
            //{
            // dbuf = buffer;
            // GPS_satellite = GPS;
            // GLONASS_satellite = GLONASS;
            //}
            /*  else
              {
                  dbuf = dataBuf2;
                  GPS_satellite = GPSsatS2;
                  GLONASS_satellite = GLONASSsatS2;
              }*/

            int msgSize = 0;

            if (data[1] == BIN_FULL)
                msgSize = BIN_FULL_MSG_SIZE;
            else if (data[1] == BIN_FULL_PLUS)
                msgSize = BIN_FULL_PLUS_MSG_SIZE;

            int checksum = calcrc(data, msgSize - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

            // if (checksum0 != data[msgSize - 3] || checksum1 != data[msgSize - 2])
            //{
            //Show_Error("CheckSum Error", "Recieved checksum is incorrect");
            //   return;
            // }

            int index = 1;      //header
            index++;    //messageType

            // State
            int state = data[index];
            buffer.state = state;
            index++;

            // Temperature
            buffer.Temperature = data[index];
            index++;

            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            buffer.NumOfVisibleSats = 0;
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GPS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;
            buffer.NumOfUsedSats = 0;
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GLONASS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.X = formatFloat(a);
            
            //dbuf.X[dbuf.counter] = formatFloat(a);
            index += 4;

            // X Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.X_Processed = formatFloat(a);
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Y = formatFloat(a);
            index += 4;

            // Y Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Y_Processed = formatFloat(a);
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Z = formatFloat(a);
            index += 4;

            // Z Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Z_Processed = formatFloat(a);
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Latitude = formatFloat(a);
            index += 4;

            //Latitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Latitude = formatFloat(a);
                buffer.Latitude_Processed = Latitude;
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Longitude = formatFloat(a);
            index += 4;

            //Longitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Longitude = formatFloat(a);
                buffer.Longitude_Processed = Longitude;
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Altitude = formatFloat(a);
            index += 4;

            //Altitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Altitude = formatFloat(a);
                buffer.Altitude_Processed = Altitude;
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vx = formatFloat(a);
            index += 4;

            // Vx Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vx_Processed = formatFloat(a);
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vy = formatFloat(a);
            index += 4;

            // Vy Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vy_Processed = formatFloat(a);
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vz = formatFloat(a);
            index += 4;

            // Vz Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Vz_Processed = formatFloat(a);
            index += 4;

            //Ax acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Ax = formatFloat(a);
            index += 4;

            //Ay acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Ay = formatFloat(a);
            index += 4;

            //Az acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.Az = formatFloat(a);
            index += 4;

            //V, V_Processed, A
            if (state == 1)
            {
                buffer.V = (Math.Sqrt(Math.Pow(buffer.Vx, 2) + Math.Pow(buffer.Vy, 2) + Math.Pow(buffer.Vz, 2)));
                buffer.V_Processed=(Math.Sqrt(Math.Pow(buffer.Vx_Processed, 2) + Math.Pow(buffer.Vy_Processed, 2) + Math.Pow(buffer.Vz_Processed, 2)));
                buffer.A=(Math.Sqrt(Math.Pow(buffer.Ax, 2) + Math.Pow(buffer.Ay, 2) + Math.Pow(buffer.Az, 2)));
            }

            //SNR GPS
            int readSNR = 0;
            for (int i = 0; i < 32; ++i)
            {
                GPS[i].SNR = 0;

                if (GPS[i].Signal_Status != 0)      //visible
                {
                    GPS[i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            //GLONASS SNR
            readSNR = 0;
            for (int i = 0; i < 28; ++i)
            {
                GLONASS[i].SNR = 0;

                if (GLONASS[i].Signal_Status != 0)      //visible
                {
                    GLONASS[i].SNR = data[index];
                    index++;
                    readSNR++;
                    if (readSNR > 12)
                        break;
                }
            }
            index += 12 - readSNR;

            a = data[index] + data[index + 1] * 256;
            //weekNumber = (int)a;
            index += 2;

            //UTC Offset
            index += 2;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int localTOW = (int)a;
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int packetDelay = (int)a;
            index += 4;

            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;
            buffer.PDOP = 0;

            if (state == 1)
            {
                GDOP = Decompress_DOP(data[index]);
                index++;
                buffer.PDOP = Decompress_DOP(data[index]);
                index++;
                HDOP = Decompress_DOP(data[index]);
                index++;
                VDOP = Decompress_DOP(data[index]);
                index++;
                TDOP = Decompress_DOP(data[index]);
                index++;
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

            Update_Controls(dbuf, state, TOW, SerialNum, HDOP, VDOP);
             * */
        }

        public static void Process_Binary_Message_Compact(byte[] data, int SerialNum,ref DataBuffer buffer,List<Satellite> GPS)
        {
           // DataBuffer dbuf;
           // List<Satellite> GPS_satellite;
            int buffercounter = 0, statcounter = -1;
           //     dbuf = buffer;
           //     GPS_satellite = GPS;

            Int64 a;
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_COMPACT_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);


            int index = 1;      //header
            index++;        //messageType

            // State
            int state = data[index];
            if (state == 1)
            {
                buffer.statcounter.Add(buffer.X.Count);
                statcounter = buffer.X.Count;
            }
            else
                buffer.statcounter.Add(-1);
            index++;

            // Temperature
            buffer.Temperature.Add(data[index]);
            buffercounter = buffer.Temperature.Count - 1;
            index++;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            buffer.NumOfVisibleSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GPS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            buffer.NumOfUsedSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.X.Add(formatFloat(a));
                buffer.X_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Y.Add(formatFloat(a));
                buffer.Y_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Z.Add(formatFloat(a));
                buffer.Z_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Latitude.Add(formatFloat(a));
                buffer.Latitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Longitude.Add(formatFloat(a));
                buffer.Longitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Altitude.Add(formatFloat(a));
                buffer.Altitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {              
                buffer.Vx.Add(formatFloat(a));
                buffer.Vx_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Vy.Add(formatFloat(a));
                buffer.Vy_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Vz.Add(formatFloat(a));
                buffer.Vz_Processed.Add(formatFloat(a));
            }
            index += 4;

            //V
            if (state == 1)
            {
                var temp = Math.Sqrt(Math.Pow(buffer.Vx[statcounter], 2) + Math.Pow(buffer.Vy[statcounter], 2) + Math.Pow(buffer.Vz[statcounter], 2));
                buffer.V.Add(temp);
                buffer.V_Processed.Add(temp);
            }
            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.PDOP.Add(formatFloat(a));
            index += 4;

            int readSNR = 0;
            bool highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS[i].SNR = 0;

                if (GPS[i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        GPS[i].SNR = (data[index] & 0xF) + 40;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        GPS[i].SNR = ((data[index] & 0xF0) >> 4) + 40;
                    }
                }
            }
            index += 8 - readSNR;

            a = data[index] + data[index + 1] * 256;
            var weekNumber = (int)a + 1024;
            index += 2;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int localTOW = (int)a;
            index += 4;

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
            index += 7;

            // Checksum
            index += 2;

            // Trailer

        }

        public static SingleDataBuffer Process_Binary_Message_Compact(byte[] data, int SerialNum, List<Satellite> GPS)
        {
            SingleDataBuffer buffer = new SingleDataBuffer();
            // List<Satellite> GPS_satellite;
            //     dbuf = buffer;
            //     GPS_satellite = GPS;

            Int64 a;
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_COMPACT_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);


            int index = 1;      //header
            index++;        //messageType

            // State
            int state = data[index];
            buffer.state = state;
            index++;

            // Temperature
            buffer.Temperature = data[index];
            index++;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            buffer.NumOfVisibleSats=0;
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 1;       //visible
                    buffer.NumOfVisibleSats++;
                }
                else
                    GPS[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            buffer.NumOfUsedSats=0;
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS[i].Signal_Status = 2;       //Used
                    buffer.NumOfUsedSats++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.X = formatFloat(a);
                buffer.X_Processed = formatFloat(a);
            }
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Y = formatFloat(a);
                buffer.Y_Processed = formatFloat(a);
            }
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Z = formatFloat(a);
                buffer.Z_Processed = formatFloat(a);
            }
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Latitude = formatFloat(a);
                buffer.Latitude_Processed = formatFloat(a);
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Longitude = formatFloat(a);
                buffer.Longitude_Processed = formatFloat(a);
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Altitude = formatFloat(a);
                buffer.Altitude_Processed = formatFloat(a);
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Vx = formatFloat(a);
                buffer.Vx_Processed= formatFloat(a);
            }
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Vy = formatFloat(a);
                buffer.Vy_Processed = formatFloat(a);
            }
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                buffer.Vz = formatFloat(a);
                buffer.Vz_Processed = formatFloat(a);
            }
            index += 4;

            //V
            if (state == 1)
            {
                var temp = Math.Sqrt(Math.Pow(buffer.Vx, 2) + Math.Pow(buffer.Vy, 2) + Math.Pow(buffer.Vz, 2));
                buffer.V = (temp);
                buffer.V_Processed = (temp);
            }
            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                buffer.PDOP = (formatFloat(a));
            index += 4;

            int readSNR = 0;
            bool highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS[i].SNR = 0;

                if (GPS[i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        GPS[i].SNR = (data[index] & 0xF) + 40;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        GPS[i].SNR = ((data[index] & 0xF0) >> 4) + 40;
                    }
                }
            }
            index += 8 - readSNR;

            a = data[index] + data[index + 1] * 256;
            var weekNumber = (int)a + 1024;
            index += 2;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            int localTOW = (int)a;
            index += 4;

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
            index += 7;

            // Checksum
            index += 2;
            return buffer;
            // Trailer

        }

        public static void Process_Binary_Message_SupplementGPS(byte[] data, int SerialNum,List<Satellite> GPS)
        {

            int checksum = calcrc(data, BIN_GPS_SUPPLEMENT_MSG_SIZE - 4);
            byte checksum0 = (byte)(checksum & 0xFF);
            byte checksum1 = (byte)((checksum >> 8) & 0xFF);

          /*  if (checksum0 != data[BIN_GPS_SUPPLEMENT_MSG_SIZE - 3] || checksum1 != data[BIN_GPS_SUPPLEMENT_MSG_SIZE - 2])
            {
                Show_Error("CheckSum Error", "Recieved checksum is incorrect");
                return;
            }*/

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
                GPS[i].Azimuth = Azimuth_OutOfRange;
                GPS[i].Elevation = Elevation_OutOfRange;
            }

            for (int i = 0; i < 12; i++)
                if (PRNs[i] != 255)
                {
                    GPS[PRNs[i]].Elevation = data[i + index];
                    if (GPS[PRNs[i]].Elevation > 127)
                        GPS[PRNs[i]].Elevation -= 256;
                    GPS[PRNs[i]].Azimuth = data[i * 2 + (index + 12)] + data[i * 2 + 1 + (index + 12)] * 256;
                    if (GPS[PRNs[i]].Azimuth > 32767)
                        GPS[PRNs[i]].Azimuth -= 32768;
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

        public static void Process_Binary_Message_RawData(byte[] data, int SerialNum,ref BinaryRawDataBuffer rbuffer)
        {          
            int index = 0, buffercounter = 0;
            Int64 a;
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_RAW_DATA_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);

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
            rbuffer.TOW.Add(a);
            buffercounter = rbuffer.TOW.Count - 1;
            index += 4;

            //satellite Positions
            rbuffer.satPos.Add(new CartesianCoordinate[12]);
            for (int j = 0; j < 12; j++)
            {
                rbuffer.satPos[buffercounter][j] = new CartesianCoordinate();

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.satPos[buffercounter][j].x = formatDouble(a);
                index += 8;

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.satPos[buffercounter][j].y = formatDouble(a);
                index += 8;

                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.satPos[buffercounter][j].z = formatDouble(a);
                index += 8;
            }

            //PseudoRanges
            rbuffer.PseudoRanges.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 7]; for (int i = 6; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.PseudoRanges[buffercounter][j] = formatDouble(a);
                index += 8;
            }

            //Prn
            rbuffer.Prn.Add(new int[12]);
            for (int i = 0; i < 12; i++)
            {
                rbuffer.Prn[buffercounter][i] = data[index];
                index++;
            }

            //dopplers
            rbuffer.dopplers.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.dopplers[buffercounter][j] = formatFloat(a);
                index += 4;
            }

            //reliability
            rbuffer.reliability.Add(new double[12]);
            for (int j = 0; j < 12; j++)
            {
                a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                rbuffer.reliability[buffercounter][j] = formatFloat(a);
                index += 4;
            }

            //pAngle
            rbuffer.pAngle.Add(new double[12][]);
            for (int j = 0; j < 12; j++)
            {
                rbuffer.pAngle[buffercounter][j] = new double[2];
                for (int k = 0; k < 2; k++)
                {
                    a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                    rbuffer.pAngle[buffercounter][j][k] = formatFloat(a);
                    index += 4;
                }
            }

            //snr
            rbuffer.snr.Add(new double[12][]);
            for (int j = 0; j < 12; j++)
            {
                rbuffer.snr[buffercounter][j] = new double[2];
                for (int k = 0; k < 2; k++)
                {
                    a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
                    rbuffer.snr[buffercounter][j][k] = formatFloat(a);
                    index += 4;
                }
            }

            /*rdBuf.counter++;
            if (rdBuf.counter >= Max_buf)
            {
                rdBuf.overLoad++;
                rdBuf.counter = 0;
            }*/
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

        public static void Process_Binary_Message_Setting(byte[] data, int serialNum)
        {
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_SETTING_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);

            int index = 1;      //header
            index++;        //messageType

            //Sat Type
            int SatType = data[index];
            index++;

            //Number of satellites
            //radDDLTypAll.Text = data[index].ToString();
            index++;
            //radDDLTypGPS.Text = data[index].ToString();
            index++;
            //radDDLTypGLONASS.Text = data[index].ToString();
            index++;
            //radDDLTypGalileo.Text = data[index].ToString();
            index++;
            //radDDLTypCompass.Text = data[index].ToString();
            index++;

            //Pos Type
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
            //radDDLSetBaudRate.Text = baudRate.ToString();

            int PacketType = data[index];
            index++;
            PacketType = (PacketType << 8) + data[index];
            index++;


            //if (radDDLSelectSerial.FindStringExact(radDDLSelectSerial.Text) == 0)
            //    index += 5;

            //radDDLRefreshRate.Text = data[index].ToString();
            index++;

            int PDOP_Threshold = data[index];
            index++;
            PDOP_Threshold = (PDOP_Threshold << 8) + data[index];
            index++;
            //textEditPdopTHD.Text = PDOP_Threshold.ToString();

            //GPS Use Threshold
            //textEditGpsUseTHD.Text = (data[index] / 4).ToString();
            index++;

            //GLONASS Use Threshold
            //textEditGlonassTHD.Text = (data[index] / 4).ToString();
            index++;

            //GPS Deassign Threshold
            //textEditGpsDeassTHD.Text = (data[index] / 4).ToString();
            index++;

            //GLONASS Deassign Threshold
            //textEditGlonassDeassTHD.Text = (data[index] / 4).ToString();
            index++;

            //Reliability Deassign Threshold
            //textEditReliabilityTHD.Text = (data[index] / 100).ToString();
            index++;

            //Satellite Distance Error Threshold
            int SatDistErrTHD = data[index];
            index++;
            SatDistErrTHD = ((SatDistErrTHD << 8) + data[index]) / 10;
            index++;
            //textEditSatDistErrTHD.Text = SatDistErrTHD.ToString();

            int MAX_Speed = data[index];
            index++;
            MAX_Speed = (MAX_Speed << 8) + data[index];
            index++;
            //textEditMaxSpeed.Text = MAX_Speed.ToString();

            int MAX_Acceleration = data[index];
            index++;
            MAX_Acceleration = ((MAX_Acceleration << 8) + data[index]) / 10;
            index++;
            //textEditMaxAcc.Text = MAX_Acceleration.ToString();

            //Mask Angle
            //textEditMaskAngle.Text = data[index].ToString();
            index++;

            //Green Satellite Type
           // radDDLGreenType.SelectedIndex = data[index];
            index++;

            //Tropospheric Correction
            //radDDLTropo.SelectedIndex = data[index];
            index++;

            //Automatic Max Angle Attitude
            //radDDLAutoMaxAngle.SelectedIndex = data[index];
            index++;

            //Ionospheric Correction
            //radDDLIonospheric.SelectedIndex = data[index];
            index++;
        }

        public static void Process_Binary_Message_Attitude_Info(byte[] data, int serialNum,ref DataBuffer databuffer,ref AttitudeInformation attitudeInfoDataBuf)
        {
            byte checksum0 = 0, checksum1 = 0;

            int statcounter = -1;
            int checksum = calcrc(data, BIN_ATTITUDE_INFO_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);


            int index = 1;      //header
            index++;        //messageType

            int state = data[index];
            if (state == 1)
            {
                databuffer.statcounter.Add(databuffer.X.Count);
                statcounter = databuffer.X.Count;
            }
            else
                databuffer.statcounter.Add(-1);
            index++;

            var attitudeState = data[index];
            index++;        //Attitude Determination State;
            /*if (attitudeStateControlUpdate)
            {
                switch (attitudeState)
                {
                    case 0:
                        radLabelState.Text = "Wait for Minimum Required Satellites";
                        radLabelState.ForeColor = Color.Yellow;
                        break;
                    case 1:
                        radLabelState.Text = "Wait for More Satellites";
                        radLabelState.ForeColor = Color.Yellow;
                        break;
                    case 2:
                        radLabelState.Text = "Ambiguity Resolution";
                        radLabelState.ForeColor = Color.Yellow;
                        break;
                    case 3:
                        radLabelState.Text = "Real Time Process";
                        radLabelState.ForeColor = Color.GreenYellow;
                        break;
                }

                attitudeStateControlUpdate = false;
            }
            */
            // Azimuth (yaw)
            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            attitudeInfoDataBuf.Azimuth.Add(formatFloat(a));
            var buffercounter = attitudeInfoDataBuf.Azimuth.Count - 1;
            index += 4;

            index += 4;     //roll

            // Elevation (pitch)
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            attitudeInfoDataBuf.Elevation.Add(formatFloat(a));
            index += 4;

            double x = 0, y = 0, z = 0;
            GEOpoint point;

            //x
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                x = formatFloat(a);
                attitudeInfoDataBuf.X.Add(x);
            }
            index += 4;

            //y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                y = formatFloat(a);
                attitudeInfoDataBuf.Y.Add(y);
            }
            index += 4;

            //z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                z = formatFloat(a);
                attitudeInfoDataBuf.Z.Add(z);
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
            index += 2;

            DateTime DatetimeUTC = new DateTime(1980, 1, 6, 0, 0, 0);
            DatetimeUTC = DatetimeUTC.AddDays(WeekNumber * 7);
            DatetimeUTC = DatetimeUTC.AddMilliseconds(TOW);

            attitudeInfoDataBuf.datetime.Add(DatetimeUTC);

            if (state == 1)
            {

                //Distance
                double distance = Math.Sqrt(Math.Pow(attitudeInfoDataBuf.X[statcounter], 2) + Math.Pow(attitudeInfoDataBuf.Y[statcounter], 2) + Math.Pow(attitudeInfoDataBuf.Z[statcounter], 2));
                attitudeInfoDataBuf.Distance[statcounter] = distance;
            }
            else
            {
                /*if (AmbiguityControlUpdate)
                {
                    radLabelAmbiguity.Text = "Remaining Ambiguity: ?";
                    AmbiguityControlUpdate = false;
                }*/

                if (attitudeInfoDataBuf.counter == 0)
                    return;

                //////    No need for this codes as List is replaced with arrays
                /*
                attitudeInfoDataBuf.datetime[buffercounter] = attitudeInfoDataBuf.datetime[buffercounter - 1].AddMilliseconds(1);

                attitudeInfoDataBuf.Azimuth[buffercounter] = double.NaN;
                attitudeInfoDataBuf.Distance[buffercounter] = double.NaN;
                attitudeInfoDataBuf.Elevation[buffercounter] = double.NaN;
                attitudeInfoDataBuf.X[buffercounter] = double.NaN;
                attitudeInfoDataBuf.Y[buffercounter] = double.NaN;
                attitudeInfoDataBuf.Z[buffercounter] = double.NaN;*/
            }

            attitudeInfoDataBuf.counter++;
        }

        public static void handle_packet(byte[] packet, ref Globals vars)
        {
            //vars.buffer = new DataBuffer();
            var key = packet[1];
            try
            {
                if (key == Functions.BIN_FULL)
                    Functions.Process_Binary_Message_Full(packet, 1, ref vars.buffer, vars.GPSSat, vars.GLONASSsat);
                else if (key == Functions.BIN_FULL_PLUS)
                    Functions.Process_Binary_Message_Full(packet, 1, ref vars.buffer, vars.GPSSat, vars.GLONASSsat);
                else if (key == Functions.BIN_COMPACT)
                    Functions.Process_Binary_Message_Compact(packet, 1, ref vars.buffer, vars.GPSSat);
                else if (key == Functions.BIN_GPS_SUPPLEMENT)
                    Functions.Process_Binary_Message_SupplementGPS(packet, 1,vars.GPSSat);
                else if (key == Functions.BIN_DEBUG) { }
                //MessageBox.Show(Functions.Process_Binary_Message_Debug(packet));
                else if (key == Functions.BIN_RAW_DATA)
                    Functions.Process_Binary_Message_RawData(packet, 1, ref vars.rbuffer);
                else if (key == Functions.BIN_LICENCE)
                    Functions.Process_Binary_Message_Licence(packet, 1, vars.licenses);
                else if (key == Functions.BIN_SETTING)
                    Functions.Process_Binary_Message_Setting(packet, 1);
                else if (key == Functions.BIN_ATTITUDE_INFO)
                    Functions.Process_Binary_Message_Attitude_Info(packet, 1, ref vars.buffer, ref vars.abuffer);
                else { }
            }
            catch
            {

            }
                //WriteText("Couldnt Find the matching processor");

            //WriteText(DateTime.Now + "  :  " + Encoding.UTF8.GetString(packet));
        }
        public static void handle_packet(string packet, Globals vars)
        {
            if (packet.StartsWith("$GP"))
                Functions.Process_Packet(packet, vars.Field);
            else
            {
                byte[] d = Encoding.UTF8.GetBytes(packet);
                handle_packet(d,ref vars);
            }

        }
        static SingleDataBuffer dbuffer = new SingleDataBuffer();
        public static SingleDataBuffer handle_packet(byte[] packet, Globals vars)
        {
            //SingleDataBuffer dbuffer = new SingleDataBuffer();
            try
            {
                var key = packet[1];
                if (key == Functions.BIN_FULL)
                    dbuffer = Functions.Process_Binary_Message_Full(packet, 1, vars.GPSSat, vars.GLONASSsat);
                else if (key == Functions.BIN_FULL_PLUS)
                    dbuffer = Functions.Process_Binary_Message_Full(packet, 1, vars.GPSSat, vars.GLONASSsat);
                else if (key == Functions.BIN_COMPACT)
                    dbuffer = Functions.Process_Binary_Message_Compact(packet, 1 , vars.GPSSat);
            }
            catch
            {

            }
            return dbuffer;
        }

    }
}
