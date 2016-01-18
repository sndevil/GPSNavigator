using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPSNavigator.Classes;
using GPSNavigator.Source;

namespace GPSNavigator
{
    public class Globals
    {
        public List<string> Field = new List<string>();
        public DataBuffer buffer = new DataBuffer();
        public BinaryRawDataBuffer rbuffer = new BinaryRawDataBuffer();
        public AttitudeInformation abuffer = new AttitudeInformation();
        public List<Satellite[]> GPSlist = new List<Satellite[]>();
        public List<Satellite[]> GLONASSlist = new List<Satellite[]>();
        public Satellite[] GPSSat = new Satellite[32];
        public Satellite[] GLONASSsat = new Satellite[32];
        public List<byte[]> licenses = new List<byte[]>();

//        public List<int> stationsInGridIndex = new List<int>();
        //public BaseStationInfo[] strBaseStationInfo = new BaseStationInfo[Functions.TOTAL_BASE_STATIONS_COUNT];
        public DateTime PacketTime = new DateTime();
        public static byte[] GPSNAN = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        public const int Databuffercount = 3000;


        public Globals()
        {
            for (int i = 0; i <32; i++)
            {
                GPSSat[i] = new Satellite();
                GLONASSsat[i] = new Satellite();
                if (i<16)
                    licenses.Add(new byte[16]);
            }
            //GPSlist.Add(GPSSat);
            //GLONASSlist.Add(GLONASSsat);
        }

        public void Clear_buffer()
        {
            buffer = new DataBuffer();
        }
    }
}
