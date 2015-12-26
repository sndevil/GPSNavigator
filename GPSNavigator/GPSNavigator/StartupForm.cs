﻿using System;
using System.Windows.Forms;
using GPSNavigator.Classes;

namespace GPSNavigator
{
    public partial class StartupForm : Form
    {
        Form1 GPS;
        public StartupForm()
        {
            InitializeComponent();
        }

        private void GPSButton_Click(object sender, EventArgs e)
        {
            GPS = new Form1(this,AppModes.GPS);
            GPS.Show();
            this.Hide();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NorthFinderButton_Click(object sender, EventArgs e)
        {
            GPS = new Form1(this, AppModes.NorthFinder);
            GPS.Show();
            this.Hide();
        }

        public void ShowStartup()
        {
            GPS.Hide();
            this.Show();
        }
    }
}
