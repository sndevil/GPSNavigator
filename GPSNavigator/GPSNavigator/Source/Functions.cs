using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPSNavigator.Classes;

namespace GPSNavigator.Source
{
    public class Functions
    {
        private const int nTRACKING_MODULES = 12;
        private const double C = 299792458.0;

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

    }
}
