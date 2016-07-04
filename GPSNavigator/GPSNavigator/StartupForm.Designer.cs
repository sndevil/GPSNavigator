namespace GPSNavigator
{
    partial class StartupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupForm));
            this.GPSButton = new System.Windows.Forms.Button();
            this.NorthFinderButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.NorthfinderLabel = new System.Windows.Forms.Label();
            this.ExitLabel = new System.Windows.Forms.Label();
            this.BTSButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.GPSLabel = new System.Windows.Forms.Label();
            this.RTKButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GPSButton
            // 
            this.GPSButton.BackColor = System.Drawing.Color.Gainsboro;
            this.GPSButton.BackgroundImage = global::GPSNavigator.Properties.Resources.GPS;
            this.GPSButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GPSButton.Location = new System.Drawing.Point(21, 24);
            this.GPSButton.Name = "GPSButton";
            this.GPSButton.Size = new System.Drawing.Size(143, 143);
            this.GPSButton.TabIndex = 0;
            this.GPSButton.UseVisualStyleBackColor = false;
            this.GPSButton.Click += new System.EventHandler(this.GPSButton_Click);
            // 
            // NorthFinderButton
            // 
            this.NorthFinderButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.NorthFinderButton.BackgroundImage = global::GPSNavigator.Properties.Resources.Compass;
            this.NorthFinderButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.NorthFinderButton.Location = new System.Drawing.Point(189, 24);
            this.NorthFinderButton.Name = "NorthFinderButton";
            this.NorthFinderButton.Size = new System.Drawing.Size(143, 143);
            this.NorthFinderButton.TabIndex = 1;
            this.NorthFinderButton.UseVisualStyleBackColor = false;
            this.NorthFinderButton.Click += new System.EventHandler(this.NorthFinderButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ExitButton.BackgroundImage = global::GPSNavigator.Properties.Resources.Exit;
            this.ExitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExitButton.Location = new System.Drawing.Point(356, 239);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(143, 143);
            this.ExitButton.TabIndex = 3;
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NorthfinderLabel
            // 
            this.NorthfinderLabel.AutoSize = true;
            this.NorthfinderLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.NorthfinderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NorthfinderLabel.Location = new System.Drawing.Point(213, 170);
            this.NorthfinderLabel.Name = "NorthfinderLabel";
            this.NorthfinderLabel.Size = new System.Drawing.Size(97, 20);
            this.NorthfinderLabel.TabIndex = 3;
            this.NorthfinderLabel.Text = "North Finder";
            // 
            // ExitLabel
            // 
            this.ExitLabel.AutoSize = true;
            this.ExitLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.ExitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitLabel.Location = new System.Drawing.Point(409, 385);
            this.ExitLabel.Name = "ExitLabel";
            this.ExitLabel.Size = new System.Drawing.Size(41, 24);
            this.ExitLabel.TabIndex = 4;
            this.ExitLabel.Text = "Exit";
            // 
            // BTSButton
            // 
            this.BTSButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BTSButton.BackgroundImage = global::GPSNavigator.Properties.Resources.Basestation;
            this.BTSButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BTSButton.Location = new System.Drawing.Point(356, 24);
            this.BTSButton.Name = "BTSButton";
            this.BTSButton.Size = new System.Drawing.Size(143, 143);
            this.BTSButton.TabIndex = 2;
            this.BTSButton.UseVisualStyleBackColor = false;
            this.BTSButton.Click += new System.EventHandler(this.BTSButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(379, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Base Station";
            // 
            // GPSLabel
            // 
            this.GPSLabel.AutoSize = true;
            this.GPSLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPSLabel.Location = new System.Drawing.Point(71, 170);
            this.GPSLabel.Name = "GPSLabel";
            this.GPSLabel.Size = new System.Drawing.Size(43, 20);
            this.GPSLabel.TabIndex = 7;
            this.GPSLabel.Text = "GPS";
            // 
            // RTKButton
            // 
            this.RTKButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.RTKButton.Location = new System.Drawing.Point(21, 239);
            this.RTKButton.Name = "RTKButton";
            this.RTKButton.Size = new System.Drawing.Size(143, 143);
            this.RTKButton.TabIndex = 8;
            this.RTKButton.UseVisualStyleBackColor = false;
            this.RTKButton.Click += new System.EventHandler(this.RTKButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(72, 388);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "RTK";
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(520, 422);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RTKButton);
            this.Controls.Add(this.GPSLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTSButton);
            this.Controls.Add(this.ExitLabel);
            this.Controls.Add(this.NorthfinderLabel);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.NorthFinderButton);
            this.Controls.Add(this.GPSButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartupForm";
            this.Text = "Choose Desired Program";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GPSButton;
        private System.Windows.Forms.Button NorthFinderButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label NorthfinderLabel;
        private System.Windows.Forms.Label ExitLabel;
        private System.Windows.Forms.Button BTSButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label GPSLabel;
        private System.Windows.Forms.Button RTKButton;
        private System.Windows.Forms.Label label2;
    }
}