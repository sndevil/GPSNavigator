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
        public List<Satellite> GPSSat = new List<Satellite>();
        public List<Satellite> GLONASSsat = new List<Satellite>();
        public List<byte[]> licenses = new List<byte[]>();


        public const int Databuffercount = 3000;


        public Globals()
        {
            for (int i = 0; i < 32; i++)
            {
                GPSSat.Add(new Satellite());
                GLONASSsat.Add(new Satellite());
                if (i < 16)
                    licenses.Add(new byte[16]);
            }
        }

        public void Clear_buffer()
        {
            buffer = new DataBuffer();
        }
    }
}
