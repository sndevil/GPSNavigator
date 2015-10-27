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

        public static void Process_Binary_Message_Full(byte[] data, int SerialNum, DataBuffer buffer, List<Satellite> GPS, List<Satellite> GLONASS)
        {
            DataBuffer dbuf;
            List<Satellite> GPS_satellite;
            List<Satellite> GLONASS_satellite;
            int buffercounter = -1;
           // if (SerialNum == 0)
            //{
                dbuf = buffer;
                GPS_satellite = GPS;
                GLONASS_satellite = GLONASS;
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
            index++;

            // Temperature
            dbuf.Temperature.Add(data[index]);
            buffercounter = dbuf.Temperature.Count-1;
            dbuf.Temperature[buffercounter] = data[index];
            index++;

            Int64 a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            dbuf.NumOfVisibleSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS_satellite[i].Signal_Status = 1;       //visible
                    dbuf.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GPS_satellite[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;
            dbuf.NumOfUsedSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS_satellite[i].Signal_Status = 2;       //Used
                    dbuf.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS_satellite[i].Signal_Status = 1;       //visible
                    dbuf.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GLONASS_satellite[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 28; ++i)
            {
                if (a % 2 == 1)
                {
                    GLONASS_satellite[i].Signal_Status = 2;       //Used
                    dbuf.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.X.Add(formatFloat(a));
                //dbuf.X[dbuf.counter] = formatFloat(a);
            index += 4;

            // X Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.X_Processed.Add(formatFloat(a));
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Y.Add(formatFloat(a));
            index += 4;

            // Y Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Y_Processed.Add(formatFloat(a));
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Z.Add(formatFloat(a));
            index += 4;

            // Z Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Z_Processed.Add(formatFloat(a));
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Latitude.Add(formatFloat(a));
            index += 4;

            //Latitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Latitude = formatFloat(a);
                dbuf.Latitude_Processed.Add(Latitude);
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Longitude.Add(formatFloat(a));
            index += 4;

            //Longitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Longitude = formatFloat(a);
                dbuf.Longitude_Processed.Add(Longitude);
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Altitude.Add(formatFloat(a));
            index += 4;

            //Altitude Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                var Altitude = formatFloat(a);
                dbuf.Altitude_Processed.Add(Altitude);
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vx.Add(formatFloat(a));
            index += 4;

            // Vx Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vx_Processed.Add(formatFloat(a));
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vy.Add(formatFloat(a));
            index += 4;

            // Vy Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vy_Processed.Add(formatFloat(a));
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vz.Add(formatFloat(a));
            index += 4;

            // Vz Processed
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vz_Processed.Add(formatFloat(a));
            index += 4;

            //Ax acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Ax.Add(formatFloat(a));
            index += 4;

            //Ay acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Ay.Add(formatFloat(a));
            index += 4;

            //Az acceleration
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Az.Add(formatFloat(a));
            index += 4;

            //V, V_Processed, A
            if (state == 1)
            {
                dbuf.V[buffercounter] = Math.Sqrt(Math.Pow(dbuf.Vx[dbuf.counter], 2) + Math.Pow(dbuf.Vy[dbuf.counter], 2) + Math.Pow(dbuf.Vz[dbuf.counter], 2));
                dbuf.V_Processed[buffercounter] = Math.Sqrt(Math.Pow(dbuf.Vx_Processed[dbuf.counter], 2) + Math.Pow(dbuf.Vy_Processed[dbuf.counter], 2) + Math.Pow(dbuf.Vz_Processed[dbuf.counter], 2));
                dbuf.A[buffercounter] = Math.Sqrt(Math.Pow(dbuf.Ax[dbuf.counter], 2) + Math.Pow(dbuf.Ay[dbuf.counter], 2) + Math.Pow(dbuf.Az[dbuf.counter], 2));
            }

            //SNR GPS
            int readSNR = 0;
            for (int i = 0; i < 32; ++i)
            {
                GPS_satellite[i].SNR = 0;

                if (GPS_satellite[i].Signal_Status != 0)      //visible
                {
                    GPS_satellite[i].SNR = data[index];
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
                GLONASS_satellite[i].SNR = 0;

                if (GLONASS_satellite[i].Signal_Status != 0)      //visible
                {
                    GLONASS_satellite[i].SNR = data[index];
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
            dbuf.PDOP.Add(0);

            if (state == 1)
            {
                GDOP = Decompress_DOP(data[index]);
                index++;
                dbuf.PDOP[buffercounter] = Decompress_DOP(data[index]);
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

        public static void Process_Binary_Message_Compact(byte[] data, int SerialNum, DataBuffer buffer, List<Satellite> GPS)
        {
            DataBuffer dbuf;
            List<Satellite> GPS_satellite;
            int buffercounter = 0;
                dbuf = buffer;
                GPS_satellite = GPS;

            Int64 a;
            byte checksum0 = 0, checksum1 = 0;

            int checksum = calcrc(data, BIN_COMPACT_MSG_SIZE - 4);
            checksum0 = (byte)(checksum & 0xFF);
            checksum1 = (byte)((checksum >> 8) & 0xFF);


            int index = 1;      //header
            index++;        //messageType

            // State
            int state = data[index];
            index++;

            // Temperature
            dbuf.Temperature.Add(data[index]);
            buffercounter = dbuf.Temperature.Count - 1;
            index++;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            double TOW = a;     //millisecond
            index += 4;

            dbuf.NumOfVisibleSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS_satellite[i].Signal_Status = 1;       //visible
                    dbuf.NumOfVisibleSats[buffercounter]++;
                }
                else
                    GPS_satellite[i].Signal_Status = 0;       //not visible
                a >>= 1;
            }
            index += 4;

            dbuf.NumOfUsedSats.Add(0);
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            for (int i = 0; i < 32; ++i)
            {
                if (a % 2 == 1)
                {
                    GPS_satellite[i].Signal_Status = 2;       //Used
                    dbuf.NumOfUsedSats[buffercounter]++;
                }
                a >>= 1;
            }
            index += 4;

            // X
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.X.Add(formatFloat(a));
                dbuf.X_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Y
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Y.Add(formatFloat(a));
                dbuf.Y_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Z
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Z.Add(formatFloat(a));
                dbuf.Z_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Latitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Latitude.Add(formatFloat(a));
                dbuf.Latitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Longitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Longitude.Add(formatFloat(a));
                dbuf.Longitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            //Altitude
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Altitude.Add(formatFloat(a));
                dbuf.Altitude_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Vx
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.Vx[dbuf.counter] = dbuf.Vx_Processed[dbuf.counter] = formatFloat(a);
            index += 4;

            // Vy
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Vy.Add(formatFloat(a));
                dbuf.Vy_Processed.Add(formatFloat(a));
            }
            index += 4;

            // Vz
            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
            {
                dbuf.Vz.Add(formatFloat(a));
                dbuf.Vz_Processed.Add(formatFloat(a));
            }
            index += 4;

            //V
            if (state == 1)
            {
                var temp = Math.Sqrt(Math.Pow(dbuf.Vx[dbuf.counter], 2) + Math.Pow(dbuf.Vy[dbuf.counter], 2) + Math.Pow(dbuf.Vz[dbuf.counter], 2));
                dbuf.V.Add(temp);
                dbuf.V_Processed.Add(temp);
            }
            // DOP
            double GDOP = 0, TDOP = 0, HDOP = 0, VDOP = 0;
            dbuf.PDOP[dbuf.counter] = 0;

            a = data[index + 3]; for (int i = 2; i >= 0; --i) a = a * 256 + data[index + i];
            if (state == 1)
                dbuf.PDOP.Add(formatFloat(a));
            index += 4;

            int readSNR = 0;
            bool highBits = false;
            for (int i = 0; i < 32; ++i)
            {
                GPS_satellite[i].SNR = 0;

                if (GPS_satellite[i].Signal_Status != 0)      //visible
                {
                    if (highBits)
                    {
                        highBits = false;
                        GPS_satellite[i].SNR = (data[index] & 0xF) + 40;
                        index++;
                        readSNR++;
                        if (readSNR > 8)
                            break;
                    }
                    else
                    {
                        highBits = true;
                        GPS_satellite[i].SNR = ((data[index] & 0xF0) >> 4) + 40;
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

    }
}
