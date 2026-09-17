namespace Create_UI.View
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lblSerial = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.btnComStatus = new System.Windows.Forms.Button();
            this.btnPLCStatus = new System.Windows.Forms.Button();
            this.btnSFCStatus = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(80)))));
            this.lblTitle.Location = new System.Drawing.Point(284, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "AOI";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft YaHei", 14F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(870, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(60, 40);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "⚙";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // lblSerial
            // 
            this.lblSerial.Font = new System.Drawing.Font("Microsoft YaHei", 14F, System.Drawing.FontStyle.Bold);
            this.lblSerial.Location = new System.Drawing.Point(110, 119);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(100, 35);
            this.lblSerial.TabIndex = 2;
            this.lblSerial.Text = "SERIAL:";
            this.lblSerial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSerial
            // 
            this.txtSerial.BackColor = System.Drawing.Color.White;
            this.txtSerial.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.txtSerial.ForeColor = System.Drawing.Color.Black;
            this.txtSerial.Location = new System.Drawing.Point(220, 114);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(580, 36);
            this.txtSerial.TabIndex = 3;
            this.txtSerial.Text = "Waiting...";
            this.txtSerial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnComStatus
            // 
            this.btnComStatus.BackColor = System.Drawing.Color.Red;
            this.btnComStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComStatus.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnComStatus.ForeColor = System.Drawing.Color.White;
            this.btnComStatus.Location = new System.Drawing.Point(92, 545);
            this.btnComStatus.Name = "btnComStatus";
            this.btnComStatus.Size = new System.Drawing.Size(220, 40);
            this.btnComStatus.TabIndex = 4;
            this.btnComStatus.Text = "🔴 COM: DISCONNECTED";
            this.btnComStatus.UseVisualStyleBackColor = false;
            // 
            // btnPLCStatus
            // 
            this.btnPLCStatus.BackColor = System.Drawing.Color.Red;
            this.btnPLCStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPLCStatus.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnPLCStatus.ForeColor = System.Drawing.Color.White;
            this.btnPLCStatus.Location = new System.Drawing.Point(360, 545);
            this.btnPLCStatus.Name = "btnPLCStatus";
            this.btnPLCStatus.Size = new System.Drawing.Size(220, 40);
            this.btnPLCStatus.TabIndex = 5;
            this.btnPLCStatus.Text = "🔴 PLC: DISCONNECTED";
            this.btnPLCStatus.UseVisualStyleBackColor = false;
            // 
            // btnSFCStatus
            // 
            this.btnSFCStatus.BackColor = System.Drawing.Color.Red;
            this.btnSFCStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSFCStatus.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnSFCStatus.ForeColor = System.Drawing.Color.White;
            this.btnSFCStatus.Location = new System.Drawing.Point(640, 545);
            this.btnSFCStatus.Name = "btnSFCStatus";
            this.btnSFCStatus.Size = new System.Drawing.Size(220, 40);
            this.btnSFCStatus.TabIndex = 6;
            this.btnSFCStatus.Text = "🔴 SFC: DISCONNECTED";
            this.btnSFCStatus.UseVisualStyleBackColor = false;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.ForeColor = System.Drawing.Color.LightGray;
            this.rtbLog.Location = new System.Drawing.Point(50, 172);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(844, 348);
            this.rtbLog.TabIndex = 7;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(942, 597);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnSFCStatus);
            this.Controls.Add(this.btnPLCStatus);
            this.Controls.Add(this.btnComStatus);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.lblSerial);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AOI - System Control";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Button btnComStatus;
        private System.Windows.Forms.Button btnPLCStatus;
        private System.Windows.Forms.Button btnSFCStatus;
        private System.Windows.Forms.RichTextBox rtbLog;
    }
}