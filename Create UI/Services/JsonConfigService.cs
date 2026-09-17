using System;
using System.IO;
using System.Text.Json;

namespace Create_UI.Services
{
    public class JsonConfigService<T> where T : class, new()
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonConfigService(string filePath)
        {
            _filePath = filePath;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public T Load()
        {
            if (!File.Exists(_filePath))
            {
                var defaultConfig = new T();
                Save(defaultConfig);
                return defaultConfig;
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<T>(json, _options) ?? new T();
            }
            catch
            {
                return new T();
            }
        }

        public void Save(T config)
        {
            var dir = Path.GetDirectoryName(_filePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(config, _options);
            File.WriteAllText(_filePath, json);
        }

        public bool Exists() => File.Exists(_filePath);
    }
}