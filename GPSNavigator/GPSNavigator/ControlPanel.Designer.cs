namespace GPSNavigator
{
    partial class ControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.programGroup = new DevExpress.XtraEditors.GroupControl();
            this.ProgramBtn = new System.Windows.Forms.Button();
            this.VerifyBtn = new System.Windows.Forms.Button();
            this.EraseBtn = new System.Windows.Forms.Button();
            this.openProgramFile = new System.Windows.Forms.OpenFileDialog();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.programGroup)).BeginInit();
            this.programGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // programGroup
            // 
            this.programGroup.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.programGroup.Appearance.Options.UseBackColor = true;
            this.programGroup.AppearanceCaption.Options.UseTextOptions = true;
            this.programGroup.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.programGroup.Controls.Add(this.label1);
            this.programGroup.Controls.Add(this.StatusLabel);
            this.programGroup.Controls.Add(this.ProgramBtn);
            this.programGroup.Controls.Add(this.VerifyBtn);
            this.programGroup.Controls.Add(this.EraseBtn);
            this.programGroup.Location = new System.Drawing.Point(12, 12);
            this.programGroup.Name = "programGroup";
            this.programGroup.Size = new System.Drawing.Size(285, 316);
            this.programGroup.TabIndex = 70;
            this.programGroup.Text = "Program Chip";
            // 
            // ProgramBtn
            // 
            this.ProgramBtn.Location = new System.Drawing.Point(17, 127);
            this.ProgramBtn.Name = "ProgramBtn";
            this.ProgramBtn.Size = new System.Drawing.Size(248, 38);
            this.ProgramBtn.TabIndex = 2;
            this.ProgramBtn.Text = "Program Chip";
            this.ProgramBtn.UseVisualStyleBackColor = true;
            this.ProgramBtn.Click += new System.EventHandler(this.ProgramBtn_Click);
            // 
            // VerifyBtn
            // 
            this.VerifyBtn.Location = new System.Drawing.Point(17, 84);
            this.VerifyBtn.Name = "VerifyBtn";
            this.VerifyBtn.Size = new System.Drawing.Size(248, 38);
            this.VerifyBtn.TabIndex = 1;
            this.VerifyBtn.Text = "Verify Chip";
            this.VerifyBtn.UseVisualStyleBackColor = true;
            this.VerifyBtn.Click += new System.EventHandler(this.VerifyBtn_Click);
            // 
            // EraseBtn
            // 
            this.EraseBtn.Location = new System.Drawing.Point(17, 38);
            this.EraseBtn.Name = "EraseBtn";
            this.EraseBtn.Size = new System.Drawing.Size(248, 38);
            this.EraseBtn.TabIndex = 0;
            this.EraseBtn.Text = "Erase Chip";
            this.EraseBtn.UseVisualStyleBackColor = true;
            this.EraseBtn.Click += new System.EventHandler(this.EraseBtn_Click);
            // 
            // openProgramFile
            // 
            this.openProgramFile.FileOk += new System.ComponentModel.CancelEventHandler(this.openProgramFile_FileOk);
            // 
            // StatusLabel
            // 
            this.StatusLabel.BackColor = System.Drawing.Color.Gainsboro;
            this.StatusLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StatusLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Location = new System.Drawing.Point(14, 243);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(251, 44);
            this.StatusLabel.TabIndex = 71;
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(14, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Status:";
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(965, 578);
            this.Controls.Add(this.programGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ControlPanel";
            this.Text = "ControlPanel";
            ((System.ComponentModel.ISupportInitialize)(this.programGroup)).EndInit();
            this.programGroup.ResumeLayout(false);
            this.programGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl programGroup;
        private System.Windows.Forms.Button ProgramBtn;
        private System.Windows.Forms.Button VerifyBtn;
        private System.Windows.Forms.Button EraseBtn;
        private System.Windows.Forms.OpenFileDialog openProgramFile;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label label1;
    }
}