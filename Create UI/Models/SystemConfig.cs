using System;
using System.IO;
using System.Text.Json;

namespace Create_UI.Models
{
    [Serializable]
    public class SystemConfig
    {
        public ScannerModel Scanner { get; set; } = new ScannerModel();
        public PLCModel PLC { get; set; } = new PLCModel();
        public MessageModel Message { get; set; } = new MessageModel();
        public ValidationModel Validation { get; set; } = new ValidationModel(); // THÊM

        /// <summary>Lỗi của lần Load gần nhất (null = đọc OK). Để form hiển thị, tránh lỗi "im lặng".</summary>
        public static string LastLoadError { get; private set; }

        public static SystemConfig Load(string path)
        {
            LastLoadError = null;
            if (!File.Exists(path))
            {
                var config = new SystemConfig();
                config.Save(path);
                return config;
            }

            try
            {
                var json = File.ReadAllText(path);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };
                return JsonSerializer.Deserialize<SystemConfig>(json, options) ?? new SystemConfig();
            }
            catch (Exception ex)
            {
                // File JSON hỏng => dùng mặc định, nhưng ghi lại lý do để hiện lên log
                LastLoadError = ex.Message;
                return new SystemConfig();
            }
        }

        public void Save(string path)
        {
            var dir = Path.GetDirectoryName(path) ?? AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(path, json);
        }
    }
}