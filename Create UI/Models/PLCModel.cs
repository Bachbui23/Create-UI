using System;

namespace Create_UI.Models
{
    [Serializable]
    public class PLCModel
    {
        public string PLC_Id { get; set; } = "192.168.250.1";

        public int PLC_Port { get; set; } = 44818;

        public string DM_Start { get; set; } = "40";
        public string DM_AOIResult { get; set; } = "42";
        public string DM_NoScan { get; set; } = "44";
        public int PollingInterval { get; set; } = 100;
        public string ExcelFolderPath { get; set; } = @"D:\DATA_META";
        public string HslAuthCode { get; set; } = "";
    }
}