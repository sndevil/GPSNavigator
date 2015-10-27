using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using GPSNavigator.Source;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class Form1 : Form
    {
        bool isRecording = true;
        List<string> Field = new List<string>();
        DataBuffer buffer = new DataBuffer();
        List<Satellite> GPSSat = new List<Satellite>();
        List<Satellite> GLONASSsat = new List<Satellite>();

        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
            for (int i = 0; i < 32; i++)
            {
                GPSSat.Add(new Satellite());
                GLONASSsat.Add(new Satellite());
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            var input = serialPort1.ReadLine();


            ///  If Logging was enabled, received message should be written to file
            

            ///  if the data packet was a valid packet, it should be proccessed
            ///  
           // try
            //{
                if (input.StartsWith("$GP"))
                    Functions.Process_Packet(input, Field);
                else
                {
                    byte[] d = Encoding.UTF8.GetBytes(input);
                    var key = d[1];
                    if (key == Functions.BIN_FULL)
                        Functions.Process_Binary_Message_Full(d, 1, buffer, GPSSat, GLONASSsat);
                    else if (key == Functions.BIN_FULL_PLUS)
                        Functions.Process_Binary_Message_Full(d, 1,buffer,GPSSat,GLONASSsat);
                    else if (key == Functions.BIN_COMPACT)
                        Functions.Process_Binary_Message_Compact(d, 1,buffer,GPSSat);
                }
                
           //}
            //catch (Exception ex)
            //{
           //     MessageBox.Show(ex.Message,"Error in processing NMEA data");
            //}


            WriteText(DateTime.Now + "  :  " + input);
        }

        delegate void SetTextCallback(string text);

        private void WriteText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.logger.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.logger.Text = text + "\r\n" + this.logger.Text;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();         
            Application.Exit();
        }


        
    }
}
