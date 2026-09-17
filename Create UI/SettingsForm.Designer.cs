namespace Create_UI
{
    partial class SettingsForm
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

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabScanner = new System.Windows.Forms.TabPage();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblBaudrate = new System.Windows.Forms.Label();
            this.txtBaudrate = new System.Windows.Forms.TextBox();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.txtDataBits = new System.Windows.Forms.TextBox();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.lblParity = new System.Windows.Forms.Label();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.tabPLC = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_NoScan = new System.Windows.Forms.TextBox();
            this.lblPLCIP = new System.Windows.Forms.Label();
            this.txtPLCIP = new System.Windows.Forms.TextBox();
            this.lblPLCPort = new System.Windows.Forms.Label();
            this.txtPLCPort = new System.Windows.Forms.TextBox();
            this.lblDMStart = new System.Windows.Forms.Label();
            this.txtDMStart = new System.Windows.Forms.TextBox();
            this.lblDMAOIResult = new System.Windows.Forms.Label();
            this.txtDMAOIResult = new System.Windows.Forms.TextBox();
            this.lblPolling = new System.Windows.Forms.Label();
            this.txtPolling = new System.Windows.Forms.TextBox();
            this.tabSFC = new System.Windows.Forms.TabPage();
            this.lblStation = new System.Windows.Forms.Label();
            this.txtStation = new System.Windows.Forms.TextBox();
            this.lblIpSfc = new System.Windows.Forms.Label();
            this.txtIpSfc = new System.Windows.Forms.TextBox();
            this.lblUrlGet = new System.Windows.Forms.Label();
            this.txtUrlGet = new System.Windows.Forms.TextBox();
            this.lblUrlPostSFC = new System.Windows.Forms.Label();
            this.txtUrlPostSFC = new System.Windows.Forms.TextBox();
            this.chkEnableGet = new System.Windows.Forms.CheckBox();
            this.chkEnablePost = new System.Windows.Forms.CheckBox();
            this.tabValidation = new System.Windows.Forms.TabPage();
            this.txt_Day = new System.Windows.Forms.TextBox();
            this.txt_Month = new System.Windows.Forms.TextBox();
            this.txt_Year = new System.Windows.Forms.TextBox();
            this.cbx_Day = new System.Windows.Forms.ComboBox();
            this.cbx_Month = new System.Windows.Forms.ComboBox();
            this.cbx_Year = new System.Windows.Forms.ComboBox();
            this.ck_SetTime = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnableValidation = new System.Windows.Forms.CheckBox();
            this.lblMinLength = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lblStartChar = new System.Windows.Forms.Label();
            this.txtStartChar = new System.Windows.Forms.TextBox();
            this.lblRegex = new System.Windows.Forms.Label();
            this.txtRegex = new System.Windows.Forms.TextBox();
            this.chkIsMetaAsn = new System.Windows.Forms.CheckBox();
            this.lblAllowedVendor = new System.Windows.Forms.Label();
            this.txtAllowedVendor = new System.Windows.Forms.TextBox();
            this.lblAllowedParts = new System.Windows.Forms.Label();
            this.txtAllowedParts = new System.Windows.Forms.TextBox();
            this.lblForbiddenChars = new System.Windows.Forms.Label();
            this.txtForbiddenChars = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_version = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabScanner.SuspendLayout();
            this.tabPLC.SuspendLayout();
            this.tabSFC.SuspendLayout();
            this.tabValidation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabScanner);
            this.tabControl.Controls.Add(this.tabPLC);
            this.tabControl.Controls.Add(this.tabSFC);
            this.tabControl.Controls.Add(this.tabValidation);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(700, 450);
            this.tabControl.TabIndex = 0;
            // 
            // tabScanner
            // 
            this.tabScanner.Controls.Add(this.lblPort);
            this.tabScanner.Controls.Add(this.txtPort);
            this.tabScanner.Controls.Add(this.lblBaudrate);
            this.tabScanner.Controls.Add(this.txtBaudrate);
            this.tabScanner.Controls.Add(this.lblDataBits);
            this.tabScanner.Controls.Add(this.txtDataBits);
            this.tabScanner.Controls.Add(this.lblStopBits);
            this.tabScanner.Controls.Add(this.cboStopBits);
            this.tabScanner.Controls.Add(this.lblParity);
            this.tabScanner.Controls.Add(this.cboParity);
            this.tabScanner.Location = new System.Drawing.Point(4, 26);
            this.tabScanner.Name = "tabScanner";
            this.tabScanner.Size = new System.Drawing.Size(692, 420);
            this.tabScanner.TabIndex = 0;
            this.tabScanner.Text = "Scanner";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(30, 30);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(120, 25);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(160, 30);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 23);
            this.txtPort.TabIndex = 1;
            // 
            // lblBaudrate
            // 
            this.lblBaudrate.Location = new System.Drawing.Point(30, 70);
            this.lblBaudrate.Name = "lblBaudrate";
            this.lblBaudrate.Size = new System.Drawing.Size(120, 25);
            this.lblBaudrate.TabIndex = 2;
            this.lblBaudrate.Text = "Baudrate:";
            this.lblBaudrate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBaudrate
            // 
            this.txtBaudrate.Location = new System.Drawing.Point(160, 70);
            this.txtBaudrate.Name = "txtBaudrate";
            this.txtBaudrate.Size = new System.Drawing.Size(200, 23);
            this.txtBaudrate.TabIndex = 3;
            // 
            // lblDataBits
            // 
            this.lblDataBits.Location = new System.Drawing.Point(30, 110);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(120, 25);
            this.lblDataBits.TabIndex = 4;
            this.lblDataBits.Text = "DataBits:";
            this.lblDataBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDataBits
            // 
            this.txtDataBits.Location = new System.Drawing.Point(160, 110);
            this.txtDataBits.Name = "txtDataBits";
            this.txtDataBits.Size = new System.Drawing.Size(200, 23);
            this.txtDataBits.TabIndex = 5;
            // 
            // lblStopBits
            // 
            this.lblStopBits.Location = new System.Drawing.Point(30, 150);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(120, 25);
            this.lblStopBits.TabIndex = 6;
            this.lblStopBits.Text = "StopBits:";
            this.lblStopBits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboStopBits
            // 
            this.cboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopBits.Items.AddRange(new object[] {
            "One",
            "Two",
            "OnePointFive"});
            this.cboStopBits.Location = new System.Drawing.Point(160, 150);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(200, 25);
            this.cboStopBits.TabIndex = 7;
            // 
            // lblParity
            // 
            this.lblParity.Location = new System.Drawing.Point(30, 190);
            this.lblParity.Name = "lblParity";
            this.lblParity.Size = new System.Drawing.Size(120, 25);
            this.lblParity.TabIndex = 8;
            this.lblParity.Text = "Parity:";
            this.lblParity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboParity
            // 
            this.cboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cboParity.Location = new System.Drawing.Point(160, 190);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(200, 25);
            this.cboParity.TabIndex = 9;
            // 
            // tabPLC
            // 
            this.tabPLC.Controls.Add(this.label1);
            this.tabPLC.Controls.Add(this.txt_NoScan);
            this.tabPLC.Controls.Add(this.lblPLCIP);
            this.tabPLC.Controls.Add(this.txtPLCIP);
            this.tabPLC.Controls.Add(this.lblPLCPort);
            this.tabPLC.Controls.Add(this.txtPLCPort);
            this.tabPLC.Controls.Add(this.lblDMStart);
            this.tabPLC.Controls.Add(this.txtDMStart);
            this.tabPLC.Controls.Add(this.lblDMAOIResult);
            this.tabPLC.Controls.Add(this.txtDMAOIResult);
            this.tabPLC.Controls.Add(this.lblPolling);
            this.tabPLC.Controls.Add(this.txtPolling);
            this.tabPLC.Location = new System.Drawing.Point(4, 26);
            this.tabPLC.Name = "tabPLC";
            this.tabPLC.Size = new System.Drawing.Size(692, 420);
            this.tabPLC.TabIndex = 1;
            this.tabPLC.Text = "PLC";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(30, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 25);
            this.label1.TabIndex = 12;
            this.label1.Text = "DM_NoScan:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_NoScan
            // 
            this.txt_NoScan.Location = new System.Drawing.Point(190, 189);
            this.txt_NoScan.Name = "txt_NoScan";
            this.txt_NoScan.Size = new System.Drawing.Size(250, 23);
            this.txt_NoScan.TabIndex = 13;
            // 
            // lblPLCIP
            // 
            this.lblPLCIP.Location = new System.Drawing.Point(30, 30);
            this.lblPLCIP.Name = "lblPLCIP";
            this.lblPLCIP.Size = new System.Drawing.Size(150, 25);
            this.lblPLCIP.TabIndex = 2;
            this.lblPLCIP.Text = "IP Address:";
            this.lblPLCIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPLCIP
            // 
            this.txtPLCIP.Location = new System.Drawing.Point(190, 30);
            this.txtPLCIP.Name = "txtPLCIP";
            this.txtPLCIP.Size = new System.Drawing.Size(250, 23);
            this.txtPLCIP.TabIndex = 3;
            // 
            // lblPLCPort
            // 
            this.lblPLCPort.Location = new System.Drawing.Point(30, 70);
            this.lblPLCPort.Name = "lblPLCPort";
            this.lblPLCPort.Size = new System.Drawing.Size(150, 25);
            this.lblPLCPort.TabIndex = 4;
            this.lblPLCPort.Text = "Port:";
            this.lblPLCPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPLCPort
            // 
            this.txtPLCPort.Location = new System.Drawing.Point(190, 70);
            this.txtPLCPort.Name = "txtPLCPort";
            this.txtPLCPort.Size = new System.Drawing.Size(250, 23);
            this.txtPLCPort.TabIndex = 5;
            // 
            // lblDMStart
            // 
            this.lblDMStart.Location = new System.Drawing.Point(30, 110);
            this.lblDMStart.Name = "lblDMStart";
            this.lblDMStart.Size = new System.Drawing.Size(150, 25);
            this.lblDMStart.TabIndex = 6;
            this.lblDMStart.Text = "DM_Start:";
            this.lblDMStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDMStart
            // 
            this.txtDMStart.Location = new System.Drawing.Point(190, 110);
            this.txtDMStart.Name = "txtDMStart";
            this.txtDMStart.Size = new System.Drawing.Size(250, 23);
            this.txtDMStart.TabIndex = 7;
            // 
            // lblDMAOIResult
            // 
            this.lblDMAOIResult.Location = new System.Drawing.Point(30, 149);
            this.lblDMAOIResult.Name = "lblDMAOIResult";
            this.lblDMAOIResult.Size = new System.Drawing.Size(150, 25);
            this.lblDMAOIResult.TabIndex = 8;
            this.lblDMAOIResult.Text = "DM_AOIResult:";
            this.lblDMAOIResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDMAOIResult
            // 
            this.txtDMAOIResult.Location = new System.Drawing.Point(190, 149);
            this.txtDMAOIResult.Name = "txtDMAOIResult";
            this.txtDMAOIResult.Size = new System.Drawing.Size(250, 23);
            this.txtDMAOIResult.TabIndex = 9;
            // 
            // lblPolling
            // 
            this.lblPolling.Location = new System.Drawing.Point(30, 228);
            this.lblPolling.Name = "lblPolling";
            this.lblPolling.Size = new System.Drawing.Size(150, 25);
            this.lblPolling.TabIndex = 10;
            this.lblPolling.Text = "Polling (ms):";
            this.lblPolling.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPolling
            // 
            this.txtPolling.Location = new System.Drawing.Point(190, 228);
            this.txtPolling.Name = "txtPolling";
            this.txtPolling.Size = new System.Drawing.Size(250, 23);
            this.txtPolling.TabIndex = 11;
            // 
            // tabSFC
            // 
            this.tabSFC.Controls.Add(this.label5);
            this.tabSFC.Controls.Add(this.txt_version);
            this.tabSFC.Controls.Add(this.lblStation);
            this.tabSFC.Controls.Add(this.txtStation);
            this.tabSFC.Controls.Add(this.lblIpSfc);
            this.tabSFC.Controls.Add(this.txtIpSfc);
            this.tabSFC.Controls.Add(this.lblUrlGet);
            this.tabSFC.Controls.Add(this.txtUrlGet);
            this.tabSFC.Controls.Add(this.lblUrlPostSFC);
            this.tabSFC.Controls.Add(this.txtUrlPostSFC);
            this.tabSFC.Controls.Add(this.chkEnableGet);
            this.tabSFC.Controls.Add(this.chkEnablePost);
            this.tabSFC.Location = new System.Drawing.Point(4, 26);
            this.tabSFC.Name = "tabSFC";
            this.tabSFC.Size = new System.Drawing.Size(692, 420);
            this.tabSFC.TabIndex = 2;
            this.tabSFC.Text = "SFC / HTTP";
            // 
            // lblStation
            // 
            this.lblStation.Location = new System.Drawing.Point(30, 150);
            this.lblStation.Name = "lblStation";
            this.lblStation.Size = new System.Drawing.Size(150, 25);
            this.lblStation.TabIndex = 0;
            this.lblStation.Text = "Station Name:";
            this.lblStation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStation
            // 
            this.txtStation.Location = new System.Drawing.Point(190, 150);
            this.txtStation.Name = "txtStation";
            this.txtStation.Size = new System.Drawing.Size(250, 23);
            this.txtStation.TabIndex = 1;
            // 
            // lblIpSfc
            // 
            this.lblIpSfc.Location = new System.Drawing.Point(30, 30);
            this.lblIpSfc.Name = "lblIpSfc";
            this.lblIpSfc.Size = new System.Drawing.Size(150, 25);
            this.lblIpSfc.TabIndex = 2;
            this.lblIpSfc.Text = "IP SFC:";
            this.lblIpSfc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIpSfc
            // 
            this.txtIpSfc.Location = new System.Drawing.Point(190, 30);
            this.txtIpSfc.Name = "txtIpSfc";
            this.txtIpSfc.Size = new System.Drawing.Size(250, 23);
            this.txtIpSfc.TabIndex = 3;
            // 
            // lblUrlGet
            // 
            this.lblUrlGet.Location = new System.Drawing.Point(30, 70);
            this.lblUrlGet.Name = "lblUrlGet";
            this.lblUrlGet.Size = new System.Drawing.Size(150, 25);
            this.lblUrlGet.TabIndex = 4;
            this.lblUrlGet.Text = "URL Get (SFC):";
            this.lblUrlGet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUrlGet
            // 
            this.txtUrlGet.Location = new System.Drawing.Point(190, 70);
            this.txtUrlGet.Name = "txtUrlGet";
            this.txtUrlGet.Size = new System.Drawing.Size(450, 23);
            this.txtUrlGet.TabIndex = 5;
            // 
            // lblUrlPostSFC
            // 
            this.lblUrlPostSFC.Location = new System.Drawing.Point(30, 110);
            this.lblUrlPostSFC.Name = "lblUrlPostSFC";
            this.lblUrlPostSFC.Size = new System.Drawing.Size(150, 25);
            this.lblUrlPostSFC.TabIndex = 6;
            this.lblUrlPostSFC.Text = "URL Post (SFC):";
            this.lblUrlPostSFC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUrlPostSFC
            // 
            this.txtUrlPostSFC.Location = new System.Drawing.Point(190, 110);
            this.txtUrlPostSFC.Name = "txtUrlPostSFC";
            this.txtUrlPostSFC.Size = new System.Drawing.Size(450, 23);
            this.txtUrlPostSFC.TabIndex = 7;
            // 
            // chkEnableGet
            // 
            this.chkEnableGet.Location = new System.Drawing.Point(190, 226);
            this.chkEnableGet.Name = "chkEnableGet";
            this.chkEnableGet.Size = new System.Drawing.Size(150, 25);
            this.chkEnableGet.TabIndex = 8;
            this.chkEnableGet.Text = "Enable Get";
            // 
            // chkEnablePost
            // 
            this.chkEnablePost.Location = new System.Drawing.Point(350, 226);
            this.chkEnablePost.Name = "chkEnablePost";
            this.chkEnablePost.Size = new System.Drawing.Size(150, 25);
            this.chkEnablePost.TabIndex = 9;
            this.chkEnablePost.Text = "Enable Post";
            // 
            // tabValidation
            // 
            this.tabValidation.Controls.Add(this.txt_Day);
            this.tabValidation.Controls.Add(this.txt_Month);
            this.tabValidation.Controls.Add(this.txt_Year);
            this.tabValidation.Controls.Add(this.cbx_Day);
            this.tabValidation.Controls.Add(this.cbx_Month);
            this.tabValidation.Controls.Add(this.cbx_Year);
            this.tabValidation.Controls.Add(this.ck_SetTime);
            this.tabValidation.Controls.Add(this.label4);
            this.tabValidation.Controls.Add(this.label3);
            this.tabValidation.Controls.Add(this.label2);
            this.tabValidation.Controls.Add(this.chkEnableValidation);
            this.tabValidation.Controls.Add(this.lblMinLength);
            this.tabValidation.Controls.Add(this.txtLength);
            this.tabValidation.Controls.Add(this.lblStartChar);
            this.tabValidation.Controls.Add(this.txtStartChar);
            this.tabValidation.Controls.Add(this.lblRegex);
            this.tabValidation.Controls.Add(this.txtRegex);
            this.tabValidation.Controls.Add(this.chkIsMetaAsn);
            this.tabValidation.Controls.Add(this.lblAllowedVendor);
            this.tabValidation.Controls.Add(this.txtAllowedVendor);
            this.tabValidation.Controls.Add(this.lblAllowedParts);
            this.tabValidation.Controls.Add(this.txtAllowedParts);
            this.tabValidation.Controls.Add(this.lblForbiddenChars);
            this.tabValidation.Controls.Add(this.txtForbiddenChars);
            this.tabValidation.Location = new System.Drawing.Point(4, 26);
            this.tabValidation.Name = "tabValidation";
            this.tabValidation.Size = new System.Drawing.Size(692, 420);
            this.tabValidation.TabIndex = 3;
            this.tabValidation.Text = "Validation";
            // 
            // txt_Day
            // 
            this.txt_Day.Location = new System.Drawing.Point(325, 369);
            this.txt_Day.Name = "txt_Day";
            this.txt_Day.Size = new System.Drawing.Size(100, 23);
            this.txt_Day.TabIndex = 30;
            // 
            // txt_Month
            // 
            this.txt_Month.Location = new System.Drawing.Point(325, 331);
            this.txt_Month.Name = "txt_Month";
            this.txt_Month.Size = new System.Drawing.Size(100, 23);
            this.txt_Month.TabIndex = 29;
            // 
            // txt_Year
            // 
            this.txt_Year.Location = new System.Drawing.Point(325, 292);
            this.txt_Year.Name = "txt_Year";
            this.txt_Year.Size = new System.Drawing.Size(100, 23);
            this.txt_Year.TabIndex = 28;
            // 
            // cbx_Day
            // 
            this.cbx_Day.FormattingEnabled = true;
            this.cbx_Day.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.cbx_Day.Location = new System.Drawing.Point(190, 367);
            this.cbx_Day.Name = "cbx_Day";
            this.cbx_Day.Size = new System.Drawing.Size(100, 25);
            this.cbx_Day.TabIndex = 27;
            // 
            // cbx_Month
            // 
            this.cbx_Month.FormattingEnabled = true;
            this.cbx_Month.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cbx_Month.Location = new System.Drawing.Point(190, 329);
            this.cbx_Month.Name = "cbx_Month";
            this.cbx_Month.Size = new System.Drawing.Size(100, 25);
            this.cbx_Month.TabIndex = 26;
            // 
            // cbx_Year
            // 
            this.cbx_Year.FormattingEnabled = true;
            this.cbx_Year.Items.AddRange(new object[] {
            "2020",
            "2021",
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030",
            "2031",
            "2032",
            "2033",
            "2034",
            "2035"});
            this.cbx_Year.Location = new System.Drawing.Point(190, 290);
            this.cbx_Year.Name = "cbx_Year";
            this.cbx_Year.Size = new System.Drawing.Size(100, 25);
            this.cbx_Year.TabIndex = 25;
            // 
            // ck_SetTime
            // 
            this.ck_SetTime.Location = new System.Drawing.Point(488, 289);
            this.ck_SetTime.Name = "ck_SetTime";
            this.ck_SetTime.Size = new System.Drawing.Size(150, 25);
            this.ck_SetTime.TabIndex = 24;
            this.ck_SetTime.Text = "Enable SetTime";
            this.ck_SetTime.CheckedChanged += new System.EventHandler(this.Ck_SetTime_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(30, 367);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 25);
            this.label4.TabIndex = 22;
            this.label4.Text = "Day:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(30, 328);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 25);
            this.label3.TabIndex = 20;
            this.label3.Text = "Month:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(30, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "Year:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkEnableValidation
            // 
            this.chkEnableValidation.Location = new System.Drawing.Point(190, 15);
            this.chkEnableValidation.Name = "chkEnableValidation";
            this.chkEnableValidation.Size = new System.Drawing.Size(150, 25);
            this.chkEnableValidation.TabIndex = 0;
            this.chkEnableValidation.Text = "Enable Validation";
            // 
            // lblMinLength
            // 
            this.lblMinLength.Location = new System.Drawing.Point(30, 50);
            this.lblMinLength.Name = "lblMinLength";
            this.lblMinLength.Size = new System.Drawing.Size(150, 25);
            this.lblMinLength.TabIndex = 1;
            this.lblMinLength.Text = "Min Length:";
            this.lblMinLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(190, 50);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(100, 23);
            this.txtLength.TabIndex = 2;
            // 
            // lblStartChar
            // 
            this.lblStartChar.Location = new System.Drawing.Point(30, 90);
            this.lblStartChar.Name = "lblStartChar";
            this.lblStartChar.Size = new System.Drawing.Size(150, 25);
            this.lblStartChar.TabIndex = 5;
            this.lblStartChar.Text = "Start Character:";
            this.lblStartChar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStartChar
            // 
            this.txtStartChar.Location = new System.Drawing.Point(190, 90);
            this.txtStartChar.Name = "txtStartChar";
            this.txtStartChar.ReadOnly = true;
            this.txtStartChar.Size = new System.Drawing.Size(100, 23);
            this.txtStartChar.TabIndex = 6;
            // 
            // lblRegex
            // 
            this.lblRegex.Location = new System.Drawing.Point(30, 130);
            this.lblRegex.Name = "lblRegex";
            this.lblRegex.Size = new System.Drawing.Size(150, 25);
            this.lblRegex.TabIndex = 9;
            this.lblRegex.Text = "Regex Pattern:";
            this.lblRegex.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRegex
            // 
            this.txtRegex.Location = new System.Drawing.Point(190, 130);
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.ReadOnly = true;
            this.txtRegex.Size = new System.Drawing.Size(450, 23);
            this.txtRegex.TabIndex = 10;
            // 
            // chkIsMetaAsn
            // 
            this.chkIsMetaAsn.Location = new System.Drawing.Point(350, 15);
            this.chkIsMetaAsn.Name = "chkIsMetaAsn";
            this.chkIsMetaAsn.Size = new System.Drawing.Size(200, 25);
            this.chkIsMetaAsn.TabIndex = 11;
            this.chkIsMetaAsn.Text = "Enable META ASN Format";
            // 
            // lblAllowedVendor
            // 
            this.lblAllowedVendor.Location = new System.Drawing.Point(30, 170);
            this.lblAllowedVendor.Name = "lblAllowedVendor";
            this.lblAllowedVendor.Size = new System.Drawing.Size(150, 25);
            this.lblAllowedVendor.TabIndex = 12;
            this.lblAllowedVendor.Text = "META Vendor:";
            this.lblAllowedVendor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAllowedVendor
            // 
            this.txtAllowedVendor.Location = new System.Drawing.Point(190, 170);
            this.txtAllowedVendor.Name = "txtAllowedVendor";
            this.txtAllowedVendor.ReadOnly = true;
            this.txtAllowedVendor.Size = new System.Drawing.Size(450, 23);
            this.txtAllowedVendor.TabIndex = 13;
            // 
            // lblAllowedParts
            // 
            this.lblAllowedParts.Location = new System.Drawing.Point(30, 210);
            this.lblAllowedParts.Name = "lblAllowedParts";
            this.lblAllowedParts.Size = new System.Drawing.Size(150, 25);
            this.lblAllowedParts.TabIndex = 14;
            this.lblAllowedParts.Text = "META Parts list:";
            this.lblAllowedParts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAllowedParts
            // 
            this.txtAllowedParts.Location = new System.Drawing.Point(190, 210);
            this.txtAllowedParts.Name = "txtAllowedParts";
            this.txtAllowedParts.Size = new System.Drawing.Size(450, 23);
            this.txtAllowedParts.TabIndex = 15;
            // 
            // lblForbiddenChars
            // 
            this.lblForbiddenChars.Location = new System.Drawing.Point(30, 250);
            this.lblForbiddenChars.Name = "lblForbiddenChars";
            this.lblForbiddenChars.Size = new System.Drawing.Size(150, 25);
            this.lblForbiddenChars.TabIndex = 16;
            this.lblForbiddenChars.Text = "Forbidden Chars:";
            this.lblForbiddenChars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtForbiddenChars
            // 
            this.txtForbiddenChars.Location = new System.Drawing.Point(190, 250);
            this.txtForbiddenChars.Name = "txtForbiddenChars";
            this.txtForbiddenChars.ReadOnly = true;
            this.txtForbiddenChars.Size = new System.Drawing.Size(450, 23);
            this.txtForbiddenChars.TabIndex = 17;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Green;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(490, 465);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 40);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "💾 Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(600, 465);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "✖ Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Orange;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(380, 465);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 40);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "↺ Reset";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(30, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "Version:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(190, 189);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(250, 23);
            this.txt_version.TabIndex = 11;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 520);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "⚙ Cài đặt hệ thống";
            this.tabControl.ResumeLayout(false);
            this.tabScanner.ResumeLayout(false);
            this.tabScanner.PerformLayout();
            this.tabPLC.ResumeLayout(false);
            this.tabPLC.PerformLayout();
            this.tabSFC.ResumeLayout(false);
            this.tabSFC.PerformLayout();
            this.tabValidation.ResumeLayout(false);
            this.tabValidation.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabScanner;
        private System.Windows.Forms.TabPage tabPLC;
        private System.Windows.Forms.TabPage tabValidation;

        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblBaudrate;
        private System.Windows.Forms.TextBox txtBaudrate;
        private System.Windows.Forms.Label lblDataBits;
        private System.Windows.Forms.TextBox txtDataBits;
        private System.Windows.Forms.Label lblStopBits;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.Label lblParity;
        private System.Windows.Forms.ComboBox cboParity;

        private System.Windows.Forms.Label lblPLCIP;
        private System.Windows.Forms.TextBox txtPLCIP;
        private System.Windows.Forms.Label lblPLCPort;
        private System.Windows.Forms.TextBox txtPLCPort;
        private System.Windows.Forms.Label lblDMStart;
        private System.Windows.Forms.TextBox txtDMStart;
        private System.Windows.Forms.Label lblDMAOIResult;
        private System.Windows.Forms.TextBox txtDMAOIResult;
        private System.Windows.Forms.Label lblPolling;
        private System.Windows.Forms.TextBox txtPolling;

        private System.Windows.Forms.CheckBox chkEnableValidation;
        private System.Windows.Forms.Label lblMinLength;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label lblStartChar;
        private System.Windows.Forms.TextBox txtStartChar;
        private System.Windows.Forms.Label lblRegex;
        private System.Windows.Forms.TextBox txtRegex;

        // Các biến META mới thêm vào Designer
        private System.Windows.Forms.CheckBox chkIsMetaAsn;
        private System.Windows.Forms.Label lblAllowedVendor;
        private System.Windows.Forms.TextBox txtAllowedVendor;
        private System.Windows.Forms.Label lblAllowedParts;
        private System.Windows.Forms.TextBox txtAllowedParts;
        private System.Windows.Forms.Label lblForbiddenChars;
        private System.Windows.Forms.TextBox txtForbiddenChars;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabPage tabSFC;
        private System.Windows.Forms.Label lblStation;
        private System.Windows.Forms.TextBox txtStation;
        private System.Windows.Forms.Label lblIpSfc;
        private System.Windows.Forms.TextBox txtIpSfc;
        private System.Windows.Forms.Label lblUrlGet;
        private System.Windows.Forms.TextBox txtUrlGet;
        private System.Windows.Forms.Label lblUrlPostSFC;
        private System.Windows.Forms.TextBox txtUrlPostSFC;
        private System.Windows.Forms.CheckBox chkEnableGet;
        private System.Windows.Forms.CheckBox chkEnablePost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ck_SetTime;
        private System.Windows.Forms.ComboBox cbx_Year;
        private System.Windows.Forms.ComboBox cbx_Day;
        private System.Windows.Forms.ComboBox cbx_Month;
        private System.Windows.Forms.TextBox txt_Day;
        private System.Windows.Forms.TextBox txt_Month;
        private System.Windows.Forms.TextBox txt_Year;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_NoScan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_version;
    }
}