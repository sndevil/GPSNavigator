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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MinCheck = new System.Windows.Forms.CheckBox();
            this.MaxCheck = new System.Windows.Forms.CheckBox();
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
            "X_Processed",
            "Y",
            "Y_Processed",
            "Z",
            "Z_Processed",
            "Latitude",
            "Latitude_Processed",
            "Longitude",
            "Longitude_Processed",
            "Altitude",
            "Altitude_Processed",
            "Vx",
            "Vx_Processed",
            "Vy",
            "Vy_Processed",
            "Vz",
            "Vz_Processed",
            "V",
            "Ax",
            "Ay",
            "Az",
            "A",
            "PDOP",
            "State",
            "Temperature",
            "UsedStats",
            "VisibleStats"});
            this.comboBox1.Location = new System.Drawing.Point(820, 448);
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
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Y";
            // 
            // rangecontrol
            // 
            this.rangecontrol.EditValue = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 10);
            this.rangecontrol.Location = new System.Drawing.Point(12, 439);
            this.rangecontrol.Name = "rangecontrol";
            this.rangecontrol.Size = new System.Drawing.Size(802, 45);
            this.rangecontrol.TabIndex = 9;
            this.rangecontrol.Value = new DevExpress.XtraEditors.Repository.TrackBarRange(0, 10);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(688, 485);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(114, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(78, 485);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(114, 20);
            this.textBox2.TabIndex = 11;
            this.textBox2.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(628, 488);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Maximum:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 488);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Minimum:";
            // 
            // MinCheck
            // 
            this.MinCheck.AutoSize = true;
            this.MinCheck.Checked = true;
            this.MinCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MinCheck.Location = new System.Drawing.Point(820, 488);
            this.MinCheck.Name = "MinCheck";
            this.MinCheck.Size = new System.Drawing.Size(73, 17);
            this.MinCheck.TabIndex = 14;
            this.MinCheck.Text = "Show Min";
            this.MinCheck.UseVisualStyleBackColor = true;
            this.MinCheck.CheckedChanged += new System.EventHandler(this.MinCheck_CheckedChanged);
            // 
            // MaxCheck
            // 
            this.MaxCheck.AutoSize = true;
            this.MaxCheck.Checked = true;
            this.MaxCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MaxCheck.Location = new System.Drawing.Point(899, 488);
            this.MaxCheck.Name = "MaxCheck";
            this.MaxCheck.Size = new System.Drawing.Size(76, 17);
            this.MaxCheck.TabIndex = 15;
            this.MaxCheck.Text = "Show Max";
            this.MaxCheck.UseVisualStyleBackColor = true;
            this.MaxCheck.CheckedChanged += new System.EventHandler(this.MaxCheck_CheckedChanged);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(994, 517);
            this.Controls.Add(this.MaxCheck);
            this.Controls.Add(this.MinCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.rangecontrol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Grapher";
            this.Text = "Grapher";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Grapher_FormClosed);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox MinCheck;
        private System.Windows.Forms.CheckBox MaxCheck;
    }
}