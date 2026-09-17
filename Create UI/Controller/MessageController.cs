using Create_UI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Create_UI.Controller
{
    public class MessageController
    {
        private MessageModel _model;
        private readonly HttpClient _client;
        private bool? _lastSfcStatus = null;   // null = chưa ping lần nào => lần đầu luôn log
        public volatile bool StatusConnectSFC = false;

        public event Action<string, ConsoleColor> OnLog;

        public MessageController(MessageModel model)
        {
            _model = model ?? new MessageModel();
            _client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        }

        /// <summary>Cập nhật cấu hình mới (URL, IP SFC, station...) sau khi lưu Settings.</summary>
        public void UpdateModel(MessageModel model)
        {
            if (model == null) return;
            _model = model;
            _lastSfcStatus = null;
        }

        /// <summary>
        /// Mã hóa URL
        /// </summary>
        private string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value ?? "");
        }

        /// <summary>
        /// Ping SFC kiểm tra kết nối
        /// </summary>
        public bool pingSFC()
        {
            try
            {
                if (string.IsNullOrEmpty(_model.IpSfc))
                {
                    StatusConnectSFC = false;
                    if (_lastSfcStatus != false)
                    {
                        _lastSfcStatus = false;
                        OnLog?.Invoke("[SFC] Chưa cấu hình IP SFC", ConsoleColor.Red);
                    }
                    return false;
                }

                IPAddress ip;
                if (!IPAddress.TryParse(_model.IpSfc, out ip))
                {
                    StatusConnectSFC = false;
                    if (_lastSfcStatus != false)
                    {
                        _lastSfcStatus = false;
                        OnLog?.Invoke("[SFC] IP SFC không hợp lệ", ConsoleColor.Red);
                    }
                    return false;
                }

                using (Ping p = new Ping())
                {
                    PingReply reply = p.Send(ip, 1000);
                    bool success = (reply != null && reply.Status == IPStatus.Success);

                    if (_lastSfcStatus != success)
                    {
                        _lastSfcStatus = success;
                        if (!success) OnLog?.Invoke("[SFC] Mất kết nối mạng tới " + _model.IpSfc, ConsoleColor.Red);
                        else OnLog?.Invoke("[SFC] Kết nối SFC OK (" + _model.IpSfc + ")", ConsoleColor.Green);
                    }

                    StatusConnectSFC = success;
                    return success;
                }
            }
            catch (Exception ex)
            {
                StatusConnectSFC = false;
                if (_lastSfcStatus != false)
                {
                    _lastSfcStatus = false;
                    OnLog?.Invoke("[SFC] Không thể kết nối mạng: " + ex.Message, ConsoleColor.Red);
                }
                return false;
            }
        }

        /// <summary>
        /// GET: Kiểm tra Serial trên hệ thống
        /// </summary>
        public async Task<bool> GetSN(string serial, string stationId)
        {
            try
            {
                if (!_model.EnableGet) return true;

                if (!StatusConnectSFC)
                {
                    OnLog?.Invoke("[SFC] Mất kết nối SFC", ConsoleColor.Red);
                    return false;
                }

                string url = (_model.UrlGet ?? "").Replace("SERIALNAME", (serial ?? "").Trim());
                url = url.Replace("StationID", (stationId ?? "").Trim());

                string resp = await GetAsync(url);

                if (string.IsNullOrEmpty(resp))
                {
                    OnLog?.Invoke("[SFC] Không nhận được phản hồi từ SFC", ConsoleColor.Red);
                    return false;
                }

                if (resp.Contains("unit_process_check=OK"))
                {
                    OnLog?.Invoke($"[SFC] Get SFC {serial} : success", ConsoleColor.Green);
                    return true;
                }
                if (resp.Contains("ASN in black list"))
                {
                    OnLog?.Invoke("[SFC] Serial nằm trong danh sách đen", ConsoleColor.Red);
                    return false;
                }
                if (resp.Contains("Go to ST19"))
                {
                    OnLog?.Invoke("[SFC] Đã qua trạm AOI", ConsoleColor.Red);
                    return false;
                }
                if (resp.Contains("Over fail count"))
                {
                    OnLog?.Invoke("[SFC] Fail 3 lần", ConsoleColor.Red);
                    return false;
                }
                if (resp.Contains("Not passed at initial workstation"))
                {
                    OnLog?.Invoke("[SFC] Chưa qua công năng", ConsoleColor.Red);
                    return false;
                }

                OnLog?.Invoke($"[SFC] Response không xác định: {resp}", ConsoleColor.Red);
                return false;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[SFC] Exception GetSN: {ex.Message}", ConsoleColor.Red);
                return false;
            }
        }

        public async Task<bool> PostData(string qrCode, string stationId, string result, string startTime, string stopTime)
        {
            try
            {
                if (!_model.EnablePost) return true;

                if (!StatusConnectSFC)
                {
                    OnLog?.Invoke("[SFC] Mất kết nối SFC", ConsoleColor.Red);
                    return false;
                }

                string url = _model.UrlPost ?? "";
                url = url.Replace("RESULT", UrlEncode(result ?? ""));
                url = url.Replace("SERIALNAME", UrlEncode((qrCode ?? "").Trim()));
                url = url.Replace("StationID", UrlEncode((stationId ?? "").Trim()));
                url = url.Replace("VERSION", UrlEncode(_model.Version ?? ""));
                url = url.Replace("START_TIME", UrlEncode(startTime ?? ""));
                url = url.Replace("STOP_TIME", UrlEncode(stopTime ?? ""));

                string resp = await GetAsync(url);

                if (string.IsNullOrEmpty(resp))
                {
                    OnLog?.Invoke($"[SFC] Post SFC '{qrCode}' không nhận được phản hồi", ConsoleColor.Red);
                    return false;
                }

                if (resp.Contains("SFC_OK"))
                {
                    OnLog?.Invoke($"[SFC] Post SFC {qrCode} success", ConsoleColor.Green);
                    return true;
                }

                OnLog?.Invoke($"[SFC] Post SFC '{qrCode}' fail. Response={resp}", ConsoleColor.Red);
                return false;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[SFC] Exception PostData: {ex.Message}", ConsoleColor.Red);
                return false;
            }
        }
        public async Task<string> GetAsync(string url)
        {
            try
            {
                //OnLog?.Invoke($"[HTTP] {url}", ConsoleColor.Cyan);
                var response = await _client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                //OnLog?.Invoke($"[HTTP] Response: {content}", ConsoleColor.Gray);
                return content;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[HTTP] GET Lỗi: {ex.Message}", ConsoleColor.Red);
                return null;
            }
        }

        /// <summary>
        /// POST: Gửi dữ liệu dạng Form URL Encoded
        /// </summary>
        public async Task<string> PostAsync(string url, Dictionary<string, string> data)
        {
            try
            {
                //OnLog?.Invoke($"[HTTP] POST: {url}", ConsoleColor.Cyan);

                var content = new FormUrlEncodedContent(data);
                var response = await _client.PostAsync(url, content);
                var responseText = await response.Content.ReadAsStringAsync();

                //OnLog?.Invoke($"[HTTP] POST Response: {responseText}", ConsoleColor.Gray);
                return responseText;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[HTTP] POST Lỗi: {ex.Message}", ConsoleColor.Red);
                return null;
            }
        }

        /// <summary>
        /// POST JSON
        /// </summary>
        public async Task<string> PostJsonAsync(string url, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                //OnLog?.Invoke($"[HTTP] POST JSON: {url}", ConsoleColor.Cyan);

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(url, content);
                var responseText = await response.Content.ReadAsStringAsync();

                //OnLog?.Invoke($"[HTTP] POST Response: {responseText}", ConsoleColor.Gray);
                return responseText;
            }
            catch (Exception ex)
            {
                OnLog?.Invoke($"[HTTP] POST Lỗi: {ex.Message}", ConsoleColor.Red);
                return null;
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}