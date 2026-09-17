using System;
using System.Threading.Tasks;

namespace Create_UI.Interfaces
{
    public interface IMessageService
    {
        bool IsConnected { get; }
        event Action<string, ConsoleColor> OnLog;
        event Action<bool> OnConnectionChanged;

        Task<bool> CheckConnectionAsync();

        // Kiểm tra SN tồn tại trong hệ thống
        Task<bool> ValidateSerialAsync(string serial);

        // Post dữ liệu lên hệ thống
        Task<bool> PostDataAsync(PostDataRequest request);
    }

    public class PostDataRequest
    {
        public string Serial { get; set; }
        public string Timer { get; set; }
        public string StationName { get; set; }
        public string Employee { get; set; }
        public string Result { get; set; }
        public string Material1 { get; set; }
        public string Material2 { get; set; }
    }
}