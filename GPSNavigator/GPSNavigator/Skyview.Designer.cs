namespace GPSNavigator
{
    partial class Skyview
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Skyview));
            this.ultraPictureBoxSkyView = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.SuspendLayout();
            // 
            // ultraPictureBoxSkyView
            // 
            this.ultraPictureBoxSkyView.BorderShadowColor = System.Drawing.Color.Empty;
            this.ultraPictureBoxSkyView.Image = ((object)(resources.GetObject("ultraPictureBoxSkyView.Image")));
            this.ultraPictureBoxSkyView.Location = new System.Drawing.Point(-1, -4);
            this.ultraPictureBoxSkyView.Name = "ultraPictureBoxSkyView";
            this.ultraPictureBoxSkyView.Size = new System.Drawing.Size(330, 330);
            this.ultraPictureBoxSkyView.TabIndex = 42;
            // 
            // toolTipController1
            // 
            this.toolTipController1.AutoPopDelay = 3600000;
            this.toolTipController1.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // Skyview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 321);
            this.Controls.Add(this.ultraPictureBoxSkyView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Skyview";
            this.Text = "Skyview";
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraPictureBox ultraPictureBoxSkyView;
        private DevExpress.Utils.ToolTipController toolTipController1;

    }
}