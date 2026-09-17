using System;

namespace Create_UI.Models
{
    [Serializable]
    public class MessageModel
    {
        // Cấu hình
        public string StationName { get; set; } = "FXVY_A03-2FT-01_1_AOI";
        public bool EnablePost { get; set; } = true;

        // Version phần mềm
        public string Version { get; set; } = "1.0d48 Beta6";

        // SFC
        public string IpSfc { get; set; } = "10.222.49.90";
        public string UrlGet { get; set; } = "http://10.222.49.90:8888/v2/pass/mes/vnaec_meta/bobcatAPI?c=QUERY_RECORD&sn=SERIALNAME&station_id=StationID&p=unit_process_check";
        public string UrlPost { get; set; } = "http://10.222.49.90:8888/v2/pass/mes/vnaec_meta/bobcatAPI?c=ADD_RECORD&result=RESULT&sn=SERIALNAME&station_id=StationID&sw_version=VERSION&start_time=START_TIME&stop_time=STOP_TIME";
        public bool EnableGet { get; set; } = true;
    }
}