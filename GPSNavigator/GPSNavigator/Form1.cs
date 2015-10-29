using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using GPSNavigator.Source;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class Form1 : Form
    {

        #region definitions
        bool isRecording = true;
        bool isPlaying = true;
        Globals vars = new Globals();
        #endregion

        public Form1()
        {
            InitializeComponent();

            try
            {
                serialPort1.Open();
            }
            catch
            {
                MessageBox.Show("Serial Port Not Found!");
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (isPlaying)
            {
                string input;
                try
                {
                    input = serialPort1.ReadLine();
                }
                catch
                {
                    input = "  ";
                }

               // handle_packet(input);


                //  If Logging was enabled, received message should be written to file


                //  if the data packet was a valid packet, it should be proccessed
                // 
                // try
                //{
                

                //}
                //catch (Exception ex)
                //{
                //     MessageBox.Show(ex.Message,"Error in processing NMEA data");
                //}


                WriteText(DateTime.Now + "  :  " + input);
            }
        }

        delegate void SetTextCallback(string text);

        private void WriteText(string text)
        {
            if (this.logger.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(WriteText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.logger.Text = text + "\r\n \r\n" + this.logger.Text;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                isPlaying = false;
                button2.Text = "Play";
            }
            else
            {
                isPlaying = true;
                button2.Text = "Pause";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void exit()
        {
            isPlaying = false;
            button2.Text = "Play";

            serialPort1.Close();
            this.Close();
            Application.Exit();
        }

        private void openLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opendialog.FileName = "";
            opendialog.Filter = "GLF Files|*.glf|All Files|*.*";
            opendialog.ShowDialog();
        }

        public void OpenLogFile(string path)
        {
            LogFileManager file = new LogFileManager(path,ref vars);
           // file.ClearBuffer();
            //var temp = file.Readbuffer();
            Grapher graphform = new Grapher(vars.buffer,file.end,file);
            graphform.ShowDialog(this);
            graphform.BringToFront();
           /* using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("~"))
                    {
                       // sr.read
                    }
                }
            }

            FileStream temp = new FileStream(path, FileMode.OpenOrCreate);

            while (temp.Position < temp.Length && temp.Position < 10000000 )   //// 10MB is the maximum load
            {
                if (temp.ReadByte() == '~')
                {
                    int msgType = temp.ReadByte();

                    int msgSize = Functions.checkMsgSize(msgType);
                    if (msgSize == -1)          //packet not valid
                        continue;

                    if (temp.Position + msgSize - 2 > temp.Length)
                        break;

                    byte[] byt = new byte[msgSize];
                    byt[0] = (byte)'~';
                    byt[1] = (byte)msgType;
                    temp.Read(byt, 2, msgSize - 2);
                    handle_packet(byt);
                    //Process_Received_BinaryBytes(byt, radioGroupDevice.SelectedIndex);
                }
            }
            Grapher graphform = new Grapher(buffer);
            graphform.ShowDialog(this);
            graphform.BringToFront();*/
        }

        private void opendialog_FileOk(object sender, CancelEventArgs e)
        {
            OpenLogFile(opendialog.FileName);
        }




        
    }
}
