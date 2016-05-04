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
            DeviceProgrammer = new Programmer(Parentform, this);
        }

        private void ProgramBtn_Click(object sender, EventArgs e)
        {        
            if (DeviceProgrammer.programloaded)
            {
                Task<bool> asyncprogrammer = StartProgram(115200, false, false);
            }
            else
                MessageBox.Show("Load a program first");
        }


        public Task<bool> StartProgram(int Baudrate, bool erase, bool verify)
        {

            return Task.Factory.StartNew(() =>
            {

                DeviceProgrammer.StartProgram(Baudrate, erase, verify);
                return true;

            });
        }

        public Task<bool> LoadFile(FileStream file)
        {
            return Task.Factory.StartNew(() =>
            {
                DeviceProgrammer.LoadFile(file);
                return true;
            });
        }


        private void VerifyBtn_Click(object sender, EventArgs e)
        {
            if (DeviceProgrammer.programloaded)
            {
                Task<bool> asyncverify = StartProgram(115200, false, true);
            }
            else
                MessageBox.Show("Load a program first");
        }

        private void EraseBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure?", "Programmer", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
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

        public delegate void FilenameSetter(string text);
        public void SetFilenameText(string text)
        {
            if (this.LoadedfileLbl.InvokeRequired)
            {
                FilenameSetter d = new FilenameSetter(SetFilenameText);
                this.LoadedfileLbl.Invoke(d, new object[] { text });
            }
            else
                this.LoadedfileLbl.Text = text;
        }

        private void LoadfileBtn_Click(object sender, EventArgs e)
        {
            if (openProgramFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream file = new FileStream(openProgramFile.FileName, FileMode.Open, FileAccess.Read);
                Task<bool> asynceraser = LoadFile(file);
            }
        }


    }
}
