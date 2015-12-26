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
            this.SuspendLayout();
            // 
            // GPSButton
            // 
            this.GPSButton.BackColor = System.Drawing.Color.Gainsboro;
            this.GPSButton.BackgroundImage = global::GPSNavigator.Properties.Resources.GPS;
            this.GPSButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GPSButton.Location = new System.Drawing.Point(30, 24);
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
            this.NorthFinderButton.Location = new System.Drawing.Point(200, 24);
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
            this.ExitButton.Location = new System.Drawing.Point(365, 282);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(143, 143);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // NorthfinderLabel
            // 
            this.NorthfinderLabel.AutoSize = true;
            this.NorthfinderLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.NorthfinderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NorthfinderLabel.Location = new System.Drawing.Point(277, 143);
            this.NorthfinderLabel.Name = "NorthfinderLabel";
            this.NorthfinderLabel.Size = new System.Drawing.Size(56, 12);
            this.NorthfinderLabel.TabIndex = 3;
            this.NorthfinderLabel.Text = "North Finder";
            // 
            // ExitLabel
            // 
            this.ExitLabel.AutoSize = true;
            this.ExitLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ExitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitLabel.Location = new System.Drawing.Point(458, 388);
            this.ExitLabel.Name = "ExitLabel";
            this.ExitLabel.Size = new System.Drawing.Size(41, 24);
            this.ExitLabel.TabIndex = 4;
            this.ExitLabel.Text = "Exit";
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(520, 437);
            this.Controls.Add(this.ExitLabel);
            this.Controls.Add(this.NorthfinderLabel);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.NorthFinderButton);
            this.Controls.Add(this.GPSButton);
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
    }
}