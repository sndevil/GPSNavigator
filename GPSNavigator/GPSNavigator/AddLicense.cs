using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GPSNavigator
{
    public partial class AddLicense : Form
    {
        public AddLicense()
        {
            InitializeComponent();
        }

        private void LicenseText_TextChanged(object sender, EventArgs e)
        {
            if (LicenseText.Text.Length == 32)
                OkButton.Enabled = true;
            else
                OkButton.Enabled = false;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
