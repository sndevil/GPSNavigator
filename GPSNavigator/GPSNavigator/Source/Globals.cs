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
        public Satellite[] GPSSat = new Satellite[32];
        public Satellite[] GLONASSsat = new Satellite[32];
        public List<byte[]> licenses = new List<byte[]>();
        public DateTime PacketTime = new DateTime();


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
        }

        public void Clear_buffer()
        {
            buffer = new DataBuffer();
        }
    }
}
