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
            this.label5 = new System.Windows.Forms.Label();
            this.FileAdressLabel = new System.Windows.Forms.Label();
            this.SaveImage = new System.Windows.Forms.Button();
            this.ExportData = new System.Windows.Forms.Button();
            this.ImportData = new System.Windows.Forms.Button();
            this.ImageSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.DataImportDialog = new System.Windows.Forms.OpenFileDialog();
            this.DataExportDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.Chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangecontrol.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart1
            // 
            this.Chart1.BackColor = System.Drawing.Color.Gainsboro;
            this.Chart1.Location = new System.Drawing.Point(0, 0);
            this.Chart1.Name = "Chart1";
            this.Chart1.PropBag = resources.GetString("Chart1.PropBag");
            this.Chart1.Size = new System.Drawing.Size(996, 433);
            this.Chart1.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(820, 448);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(162, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Location = new System.Drawing.Point(731, 388);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "X";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLightLight;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(6, 415);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(691, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Click : Zoom In          Right Click : Zoom Out          Click (Hold) + Drag : Zo" +
                "om In To the selected Region          Alt + Click : Open In Moment Detail";
            // 
            // FileAdressLabel
            // 
            this.FileAdressLabel.BackColor = System.Drawing.Color.Transparent;
            this.FileAdressLabel.Location = new System.Drawing.Point(24, 6);
            this.FileAdressLabel.Name = "FileAdressLabel";
            this.FileAdressLabel.Size = new System.Drawing.Size(951, 19);
            this.FileAdressLabel.TabIndex = 17;
            this.FileAdressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SaveImage
            // 
            this.SaveImage.Location = new System.Drawing.Point(211, 483);
            this.SaveImage.Name = "SaveImage";
            this.SaveImage.Size = new System.Drawing.Size(130, 23);
            this.SaveImage.TabIndex = 18;
            this.SaveImage.Text = "Save Graph Image";
            this.SaveImage.UseVisualStyleBackColor = true;
            this.SaveImage.Click += new System.EventHandler(this.SaveImage_Click);
            // 
            // ExportData
            // 
            this.ExportData.Location = new System.Drawing.Point(347, 483);
            this.ExportData.Name = "ExportData";
            this.ExportData.Size = new System.Drawing.Size(130, 23);
            this.ExportData.TabIndex = 19;
            this.ExportData.Text = "Export Graph Data";
            this.ExportData.UseVisualStyleBackColor = true;
            this.ExportData.Click += new System.EventHandler(this.ExportData_Click);
            // 
            // ImportData
            // 
            this.ImportData.Location = new System.Drawing.Point(483, 483);
            this.ImportData.Name = "ImportData";
            this.ImportData.Size = new System.Drawing.Size(130, 23);
            this.ImportData.TabIndex = 20;
            this.ImportData.Text = "Import Graph Data";
            this.ImportData.UseVisualStyleBackColor = true;
            this.ImportData.Click += new System.EventHandler(this.ImportData_Click);
            // 
            // ImageSaveDialog
            // 
            this.ImageSaveDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.ImageSaveDialog_FileOk);
            // 
            // DataImportDialog
            // 
            this.DataImportDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.DataImportDialog_FileOk);
            // 
            // DataExportDialog
            // 
            this.DataExportDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.DataExportDialog_FileOk);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(994, 517);
            this.Controls.Add(this.ImportData);
            this.Controls.Add(this.ExportData);
            this.Controls.Add(this.SaveImage);
            this.Controls.Add(this.FileAdressLabel);
            this.Controls.Add(this.label5);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label FileAdressLabel;
        private System.Windows.Forms.Button SaveImage;
        private System.Windows.Forms.Button ExportData;
        private System.Windows.Forms.Button ImportData;
        private System.Windows.Forms.SaveFileDialog ImageSaveDialog;
        private System.Windows.Forms.OpenFileDialog DataImportDialog;
        private System.Windows.Forms.SaveFileDialog DataExportDialog;
    }
}