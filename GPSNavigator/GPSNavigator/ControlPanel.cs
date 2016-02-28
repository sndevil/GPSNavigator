using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using GPSNavigator.Source;
using GPSNavigator.Classes;
using System.Threading.Tasks;

namespace GPSNavigator
{
    public partial class ControlPanel : Form
    {
        public Form1 Parentform;
        public SettingBuffer Settings;
        public Programmer DeviceProgrammer;

        public ControlPanel(Form1 Parent)
        {         
            InitializeComponent();
            Parentform = Parent;
            openProgramFile.Filter = "MCS File (*.MCS)|*.MCS|All Files|*.*";
            //opendialog.Filter = "GPS LogPackage File (*.GLP)|*.GLP|All Files|*.*";
        }

        private void ProgramBtn_Click(object sender, EventArgs e)
        {        
            if (openProgramFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream file = new FileStream(openProgramFile.FileName, FileMode.Open, FileAccess.Read);
                DeviceProgrammer = new Programmer(file, Parentform,this);
                Task<bool> asyncprogrammer = StartProgram(115200,false,false);
            }
        }


        public Task<bool> StartProgram(int Baudrate, bool erase, bool verify)
        {

            return Task.Factory.StartNew(() =>
            {

                DeviceProgrammer.StartProgram(Baudrate, erase, verify);
                return true;

            });
        }


        private void VerifyBtn_Click(object sender, EventArgs e)
        {
            if (openProgramFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream file = new FileStream(openProgramFile.FileName, FileMode.Open, FileAccess.Read);
                DeviceProgrammer = new Programmer(file, Parentform,this);
                Task<bool> asyncverify = StartProgram(115200, false, true);
            }
        }

        private void EraseBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure?", "Programmer", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                DeviceProgrammer = new Programmer(null, Parentform, this);
                Task<bool> asynceraser = StartProgram(115200, true, false);
            }
        }

        private void openProgramFile_FileOk(object sender, CancelEventArgs e)
        {

        }


        public delegate void StatusSetter(string text);
        public void SetStatusText(string text)
        {
            if (this.StatusLabel.InvokeRequired)
            {
                StatusSetter d = new StatusSetter(SetStatusText);
                this.StatusLabel.Invoke(d, new object[] { text });
            }
            else
                this.StatusLabel.Text = text;

        }


    }
}
