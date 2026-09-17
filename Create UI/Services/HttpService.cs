using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Create_UI.Services
{
    public class HttpService : IDisposable
    {
        private readonly HttpClient _client;
        private bool _disposed;

        public HttpService(int timeoutSeconds = 30)
        {
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };
            _client.DefaultRequestHeaders.Add("User-Agent", "CreateUI/1.0");
        }

        public void SetApiKey(string apiKey)
        {
            if (!string.IsNullOrEmpty(apiKey))
                _client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
        }

        public void SetBearerToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> GetAsync(string url)
        {
            try
            {
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"HTTP GET failed: {ex.Message}", ex);
            }
        }

        public async Task<string> PostAsync(string url, string data, string contentType = "application/x-www-form-urlencoded")
        {
            try
            {
                var content = new StringContent(data, Encoding.UTF8, contentType);
                var response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"HTTP POST failed: {ex.Message}", ex);
            }
        }

        public async Task<string> PostJsonAsync(string url, string jsonData)
        {
            return await PostAsync(url, jsonData, "application/json");
        }

        public async Task<string> PostJsonAsync<T>(string url, T data)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            return await PostJsonAsync(url, json);
        }

        public string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value ?? "");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _client?.Dispose();
        }
    }
}