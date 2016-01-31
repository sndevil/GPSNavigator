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

namespace GPSNavigator
{
    public partial class ControlPanel : Form
    {
        public Form1 Parentform;
        public SettingBuffer Settings;

        public ControlPanel(Form1 Parent)
        {
            InitializeComponent();
            Parentform = Parent;
            openProgramFile.Filter = "Hex File (*.HEX)|*.HEX|All Files|*.*";
            //opendialog.Filter = "GPS LogPackage File (*.GLP)|*.GLP|All Files|*.*";
        }

        private void ProgramBtn_Click(object sender, EventArgs e)
        {
            if (openProgramFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileStream file = new FileStream(openProgramFile.FileName, FileMode.Open, FileAccess.Read);

            }
        }


    }
}
