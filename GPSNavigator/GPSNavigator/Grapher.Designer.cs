namespace GPSNavigator
{
    partial class Grapher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grapher));
            this.Chart1 = new C1.Win.C1Chart.C1Chart();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rangecontrol = new DevExpress.XtraEditors.RangeTrackBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.Chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart1
            // 
            this.Chart1.Location = new System.Drawing.Point(0, 0);
            this.Chart1.Name = "Chart1";
            this.Chart1.PropBag = resources.GetString("Chart1.PropBag");
            this.Chart1.Size = new System.Drawing.Size(996, 433);
            this.Chart1.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "X",
            "Y",
            "Z",
            "Latitude",
            "Longitude",
            "Altitude",
            "Vx",
            "Vy",
            "Vz",
            "Ax",
            "Ay",
            "Az",
            "PDOP",
            "State",
            "Temperature",
            "UsedStats",
            "VisibleStats"});
            this.comboBox1.Location = new System.Drawing.Point(820, 454);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(162, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(459, 391);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "label3";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "label4";
            // 
            // rangecontrol
            // 
            this.rangecontrol.EditValue = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 10);
            this.rangecontrol.Location = new System.Drawing.Point(36, 439);
            this.rangecontrol.Name = "rangecontrol";
            this.rangecontrol.Size = new System.Drawing.Size(697, 45);
            this.rangecontrol.TabIndex = 9;
            this.rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 10);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 496);
            this.Controls.Add(this.rangecontrol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Grapher";
            this.Text = "Grapher";
            this.Load += new System.EventHandler(this.Grapher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Chart.C1Chart Chart1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.RangeTrackBarControl rangecontrol;
    }
}