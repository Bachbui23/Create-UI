using Create_UI.Controller;
using Create_UI.Models;
using Create_UI.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsbScan_Frame4_C22;

namespace Create_UI.View
{
    public partial class Form1 : Form
    {
        #region Variables
        private readonly string _configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "appsettings.json");

        private ScanUSBC22 usbC22;
        private SystemConfig _config;
        private ScannerController _scanner;
        private PLCController _plc;
        private MessageController _message;

        // Dùng để dừng 2 vòng lặp nền (PLC polling, SFC ping) khi đóng form
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // 0 = rảnh, 1 = đang xử lý 1 mã. Dùng Interlocked để 2 luồng không cùng lọt vào.
        private int _processingFlag = 0;

        // Trạng thái trước đó để tránh log spam
        private bool _lastComStatus = false;

        private string _currentSerial = "";      // mã vừa scan, CHƯA chắc hợp lệ
        private string _verifiedSerial = "";     // mã ĐÃ qua GetSN thành công — chỉ cái này được phép POST

        private const int MaxLogLines = 1000;
        private const int KeepLogLines = 500;

        private static readonly Dictionary<ConsoleColor, Color> ConsoleColorMap = new Dictionary<ConsoleColor, Color>
        {
            [ConsoleColor.Red] = Color.Red,
            [ConsoleColor.Green] = Color.Lime,
            [ConsoleColor.Yellow] = Color.Yellow,
            [ConsoleColor.Cyan] = Color.Cyan,
            [ConsoleColor.Gray] = Color.Gray,
            [ConsoleColor.White] = Color.White,
            [ConsoleColor.DarkYellow] = Color.Orange,
            [ConsoleColor.Magenta] = Color.Magenta,
            [ConsoleColor.Blue] = Color.DeepSkyBlue,
            [ConsoleColor.DarkRed] = Color.DarkRed,
            [ConsoleColor.DarkGreen] = Color.Green,
            [ConsoleColor.DarkBlue] = Color.DodgerBlue
        };
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region UI Methods
        /// <summary>
        /// Chạy 1 đoạn code trên luồng giao diện.
        /// Dùng BeginInvoke (không chờ) thay cho Invoke (chờ) để luồng nền không bao giờ bị treo
        /// khi luồng giao diện đang bận / đang chờ PLC.
        /// </summary>
        private void RunOnUI(Action action)
        {
            if (IsDisposed || Disposing) return;

            if (InvokeRequired)
            {
                try { BeginInvoke(action); }
                catch (InvalidOperationException) { /* form đã đóng (gồm cả ObjectDisposedException) */ }
                return;
            }
            action();
        }

        private void ShowLog(string msg, Color color)
        {
            RunOnUI(() =>
            {
                // Đưa con trỏ về cuối trước khi đặt màu (nếu người dùng click vào log, màu không bị lệch)
                rtbLog.SelectionStart = rtbLog.TextLength;
                rtbLog.SelectionLength = 0;
                rtbLog.SelectionColor = color;
                rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss:fff}] {msg}\r\n");

                // Giới hạn số dòng: xoá bớt dòng cũ nhưng GIỮ MÀU (bản cũ gán lại .Text làm mất hết màu)
                int lineCount = rtbLog.GetLineFromCharIndex(rtbLog.TextLength);
                if (lineCount > MaxLogLines)
                {
                    int cutTo = rtbLog.GetFirstCharIndexFromLine(lineCount - KeepLogLines);
                    if (cutTo > 0)
                    {
                        rtbLog.ReadOnly = false;
                        rtbLog.Select(0, cutTo);
                        rtbLog.SelectedText = "";
                        rtbLog.ReadOnly = true;
                    }
                }

                rtbLog.SelectionStart = rtbLog.TextLength;
                rtbLog.ScrollToCaret();
            });
        }

        private void UpdateSerial(string serial, Color backColor)
        {
            RunOnUI(() =>
            {
                txtSerial.Text = serial;
                txtSerial.BackColor = backColor;
            });
        }

        private void UpdateStatus(Button btn, bool connected, string label)
        {
            RunOnUI(() =>
            {
                btn.BackColor = connected ? Color.Green : Color.Red;
                btn.Text = connected ? $"🟢 {label}: CONNECTED" : $"🔴 {label}: DISCONNECTED";
            });
        }

        private void FocusTextBox()
        {
            txtSerial.Focus();
            txtSerial.SelectionStart = txtSerial.Text.Length;
            txtSerial.SelectionLength = 0;
        }
        #endregion

        #region Initialize
        private void Initialize()
        {
            _config = SystemConfig.Load(_configPath);
            if (!string.IsNullOrEmpty(SystemConfig.LastLoadError))
                ShowLog("[CONFIG] Đọc appsettings.json lỗi, đang dùng cấu hình mặc định: " + SystemConfig.LastLoadError, Color.Red);

            ActivateHslLicense();

            // ===== SCANNER =====
            usbC22 = new ScanUSBC22();
            _scanner = new ScannerController(_config.Scanner, usbC22);
            _scanner.OnLog += (msg, color) => ShowLog(msg, color);   // msg đã có sẵn "[SCAN]"
            _scanner.OnDataReceived += Scanner_OnDataReceived;
            _scanner.OnConnected += (connected) =>
            {
                UpdateStatus(btnComStatus, connected, "COM");
                if (_lastComStatus != connected)
                {
                    _lastComStatus = connected;
                    ShowLog($"[SCAN] {(connected ? "Kết nối" : "Mất kết nối")}",
                        connected ? Color.Lime : Color.Red);
                }
            };

            // ===== PLC =====
            // PLCController tự log khi đổi trạng thái => ở đây chỉ cập nhật nút, không log lại (tránh log đôi)
            _plc = new PLCController(_config.PLC);
            _plc.OnLog += (msg, color) => ShowLog(msg, color);         // msg đã có sẵn "[PLC]"
            _plc.OnConnected += (connected) => UpdateStatus(btnPLCStatus, connected, "PLC");

            // ===== MESSAGE =====
            _message = new MessageController(_config.Message);
            _message.OnLog += (msg, color) =>
            {
                Color c;
                ShowLog(msg, ConsoleColorMap.TryGetValue(color, out c) ? c : Color.LightGray);
            };

            ShowLog($"Station: {_config.Message.StationName}", Color.Cyan);
            ShowLog($"[PLC] {_config.PLC.PLC_Id}:{_config.PLC.PLC_Port} (CIP) | Start='{_config.PLC.DM_Start}' " +
                    $"AOIResult='{_config.PLC.DM_AOIResult}' NoScan='{_config.PLC.DM_NoScan}'", Color.Cyan);
        }

        /// <summary>
        /// HslCommunication là thư viện thương mại. Nếu không kích hoạt, theo tài liệu của thư viện
        /// nó chỉ chạy được vài giờ rồi ngừng giao tiếp => PLC báo mất kết nối mà không rõ lý do.
        /// Nhập mã vào "HslAuthCode" trong Config/appsettings.json.
        /// </summary>
        private void ActivateHslLicense()
        {
            string code = _config.PLC.HslAuthCode;
            if (string.IsNullOrWhiteSpace(code))
            {
                ShowLog("[PLC] Chưa có mã bản quyền HslCommunication (HslAuthCode). " +
                        "Thư viện chạy chế độ dùng thử và có thể ngừng giao tiếp sau vài giờ.", Color.Orange);
                return;
            }

            try
            {
                bool ok = HslCommunication.Authorization.SetAuthorizationCode(code.Trim());
                ShowLog(ok ? "[PLC] Kích hoạt HslCommunication OK" : "[PLC] Mã HslAuthCode không hợp lệ",
                        ok ? Color.Lime : Color.Red);
            }
            catch (Exception ex)
            {
                ShowLog("[PLC] Lỗi kích hoạt HslCommunication: " + ex.Message, Color.Red);
            }
        }
        #endregion

        #region Validation
        private bool ValidateSerial(string serial)
        {
            if (string.IsNullOrEmpty(serial))
            {
                ShowLog("[VALIDATE] Serial rỗng", Color.Red);
                return false;
            }

            var val = _config.Validation;

            if (!val.EnableValidation) return true;

            if (serial.Length != val.Length)
            {
                ShowLog($"[VALIDATE] Độ dài không hợp lệ: {serial.Length}", Color.Red);
                return false;
            }

            if (!string.IsNullOrEmpty(val.StartChar) && !serial.StartsWith(val.StartChar, StringComparison.OrdinalIgnoreCase))
            {
                ShowLog($"[VALIDATE] Project fail '{val.StartChar}'", Color.Red);
                return false;
            }

            // 4. KIỂM TRA ĐỊNH DẠNG QUẢN CHẾ META ASN (Nếu bật)
            if (val.IsMetaAsnFormat)
            {
                // Mã Meta ASN cần tối thiểu 14 ký tự để cắt chuỗi bên dưới.
                // (Nếu Length trong cài đặt bị đặt < 14 thì Substring sẽ lỗi => chặn trước)
                if (serial.Length < 14)
                {
                    ShowLog($"[VALIDATE] Mã quá ngắn cho định dạng Meta ASN: {serial.Length}", Color.Red);
                    return false;
                }

                // Bóc tách cụm theo chuẩn hình ảnh
                string project = serial.Substring(0, 2);
                string vendor = serial.Substring(2, 2);
                string partConfig = serial.Substring(4, 3);

                char year = serial[7];
                char month = serial[8];
                char day = serial[9];
                string uniqueId = serial.Substring(10, 4);

                // Check Vendor
                if (!string.IsNullOrEmpty(val.AllowedVendor) && !vendor.Equals(val.AllowedVendor, StringComparison.OrdinalIgnoreCase))
                {
                    ShowLog($"[VALIDATE] Vendor fail '{vendor}'", Color.Red);
                    return false;
                }

                // Check Part and Configuration (231, 232, 233, 234...)
                if (!string.IsNullOrEmpty(val.AllowedParts) && !partConfig.Equals(val.AllowedParts, StringComparison.OrdinalIgnoreCase))
                {
                    ShowLog($"[VALIDATE] Part fail '{partConfig}' ", Color.Red);
                    return false;
                }

                // Check Key Message: Năm, Tháng, Ngày, Unique KHÔNG ĐƯỢC chứa A, E, I, O, U
                //if (!string.IsNullOrEmpty(val.ForbiddenMetaChars))
                //{
                //    char[] forbiddenArr = val.ForbiddenMetaChars.ToUpper().ToCharArray();

                //    if (forbiddenArr.Contains(char.ToUpper(year)) ||
                //        forbiddenArr.Contains(char.ToUpper(month)) ||
                //        forbiddenArr.Contains(char.ToUpper(day)))
                //    {
                //        ShowLog($"[VALIDATE] Ký tự Ngày/Tháng/Năm chứa ký tự cấm ({val.ForbiddenMetaChars})", Color.Red);
                //        return false;
                //    }

                //    if (year.ToString() != val.YearChar || month.ToString() != val.MonthChar || day.ToString() != val.DayChar)
                //    {
                //        ShowLog($"[VALIDATE] Future Time fail", Color.Red);
                //        return false;
                //    }
                //}
            }

            ShowLog($"[VALIDATE] ✅ Mã {serial} hợp lệ định dạng", Color.Lime);
            return true;
        }
        #endregion

        #region Event Handlers - Scanner
        private async void Scanner_OnDataReceived(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return;
            data = data.Trim();

            // Chỉ cho 1 mã được xử lý tại 1 thời điểm (an toàn khi nhiều luồng cùng gọi)
            if (Interlocked.CompareExchange(ref _processingFlag, 1, 0) != 0)
            {
                ShowLog($"[SCAN] Đang xử lý mã trước, bỏ qua mã: {data}", Color.Orange);
                return;
            }

            // async void: exception thoát ra ngoài sẽ làm SẬP chương trình => bắt hết ở đây,
            // và finally đảm bảo luôn mở khoá, không bị kẹt "đang xử lý" mãi mãi.
            try
            {
                _currentSerial = data;
                ShowLog($"[SCAN] Nhận mã từ Scanner: {data}", Color.Cyan);
                UpdateSerial(data, Color.LightYellow);

                if (!ValidateSerial(data))
                {
                    SendScanNG("mã sai định dạng");
                    UpdateSerial(data, Color.LightPink);
                    return;
                }

                bool isExist = await Task.Run(() => _message.GetSN(data, _config.Message.StationName));
                if (!isExist)
                {
                    SendScanNG("SFC từ chối");
                    UpdateSerial(data, Color.LightPink);
                    return;
                }

                string pending = _verifiedSerial;
                if (!string.IsNullOrEmpty(pending) && pending != data)
                    ShowLog($"[SCAN] Cảnh báo: mã trước '{pending}' chưa nhận kết quả AOI, sẽ bị thay bằng mã mới", Color.Orange);

                // Xoá kết quả AOI cũ TRƯỚC, rồi mới báo OK => không đọc nhầm kết quả cũ cho mã mới
                _plc.WriteInt16(_config.PLC.DM_AOIResult, 0);
                _verifiedSerial = data;

                if (_plc.WriteInt16(_config.PLC.DM_Start, 1))
                {
                    ShowLog($"[PLC] Gửi tín hiệu OK (1) vào '{_config.PLC.DM_Start}'", Color.Lime);
                    UpdateSerial(data, Color.LightGreen);
                }
                else
                {
                    // PLC không nhận được lệnh => máy sẽ không chạy, không giữ mã này chờ kết quả
                    _verifiedSerial = "";
                    ShowLog($"[PLC] KHÔNG gửi được tín hiệu OK cho mã {data} (PLC chưa kết nối / sai tên tag)", Color.Red);
                    UpdateSerial(data, Color.LightPink);
                }
            }
            catch (Exception ex)
            {
                ShowLog($"[SCAN] Lỗi xử lý mã {data}: {ex.Message}", Color.Red);
                UpdateSerial(data, Color.LightPink);
            }
            finally
            {
                Interlocked.Exchange(ref _processingFlag, 0);
            }
        }

        private void SendScanNG(string reason)
        {
            if (_plc.WriteInt16(_config.PLC.DM_Start, 2))
                ShowLog($"[PLC] Gửi tín hiệu NG (2) vào '{_config.PLC.DM_Start}' - {reason}", Color.Red);
            else
                ShowLog($"[PLC] KHÔNG gửi được tín hiệu NG ({reason}) - PLC chưa kết nối / sai tên tag", Color.Red);
        }
        #endregion

        #region PLC Polling
        private async Task PLCPollingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!_plc.IsConnected)
                    {
                        // Mất kết nối: đợi 3s rồi thử lại (PLCController tự log khi kết nối lại được)
                        await Task.Delay(3000, token);
                        _plc.Connect();
                        continue;
                    }

                    // Đọc DM_NoScan mỗi chu kỳ: vừa lấy tín hiệu, vừa đóng vai trò "heartbeat".
                    // (Bản cũ đọc thêm tag "0" để kiểm tra - với CIP tag đó không tồn tại nên luôn báo Offline)
                    short noScan;
                    if (_plc.TryReadInt16(_config.PLC.DM_NoScan, out noScan)
                        && noScan == 0
                        && !string.IsNullOrEmpty(_verifiedSerial))
                    {
                        await CheckStationResult(_config.PLC.DM_AOIResult);
                    }

                    await Task.Delay(Math.Max(20, _config.PLC.PollingInterval), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    ShowLog("[PLC] Lỗi vòng polling: " + ex.Message, Color.Red);
                    try { await Task.Delay(3000, token); }
                    catch (OperationCanceledException) { break; }
                }
            }
        }

        private async Task CheckStationResult(string address)
        {
            short result;
            if (!_plc.TryReadInt16(address, out result) || result == 0) return;

            // Lấy mã ra và xoá NGAY => dù bước sau có lỗi cũng không bị POST lặp lại nhiều lần
            string serial = Interlocked.Exchange(ref _verifiedSerial, "");
            if (string.IsNullOrEmpty(serial)) return;

            bool isOK = (result == 1);

            try
            {
                ShowLog($"[AOI] Kết quả: {(isOK ? "OK" : "NG")} (giá trị {result}) - mã {serial}", isOK ? Color.Lime : Color.Red);
                UpdateSerial(serial, isOK ? Color.LightGreen : Color.LightPink);

                string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // 1. Post kết quả lên MES
                bool postResult = await _message.PostData(
                    serial,
                    _config.Message.StationName,
                    isOK ? "PASS" : "FAIL",
                    now,
                    now
                );
                ShowLog($"[SFC] Post {serial} {(postResult ? "OK" : "FAIL")}", postResult ? Color.Lime : Color.Red);

                // 2. Ghi serial vào file Excel AOI (file đổi theo ngày: <thư mục>\yyyy-MM-dd\yyyy-MM-dd.xlsx)
                try
                {
                    string excelPath = ExcelService.BuildFilePath(_config.PLC.ExcelFolderPath, "yyyy-MM-dd", "yyyy-MM-dd");

                    bool wroteP1 = ExcelService.AddSerialToRow(excelPath, "P1", serial);
                    ShowLog($"[EXCEL] Ghi mã P1: {(wroteP1 ? "OK" : "Không tìm thấy file/dòng trống")}",
                        wroteP1 ? Color.Lime : Color.Orange);

                    bool wroteP2 = ExcelService.AddSerialToRow(excelPath, "P2", serial);
                    ShowLog($"[EXCEL] Ghi mã P2: {(wroteP2 ? "OK" : "Không tìm thấy file/dòng trống")}",
                        wroteP2 ? Color.Lime : Color.Orange);
                }
                catch (Exception ex)
                {
                    ShowLog("[EXCEL] Lỗi ghi file: " + ex.Message, Color.Red);
                }
            }
            finally
            {
                // Luôn trả thanh ghi kết quả về 0 để PLC biết PC đã xử lý xong
                if (!_plc.WriteInt16(address, 0))
                    ShowLog($"[PLC] Không reset được '{address}' về 0", Color.Red);
            }
        }

        private async Task SFCPingLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // MessageController tự log khi đổi trạng thái => ở đây chỉ cập nhật nút
                    bool ok = _message.pingSFC();
                    UpdateStatus(btnSFCStatus, ok, "SFC");
                    await Task.Delay(5000, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch
                {
                    UpdateStatus(btnSFCStatus, false, "SFC");
                    try { await Task.Delay(5000, token); }
                    catch (OperationCanceledException) { break; }
                }
            }
        }
        #endregion

        #region Form Events
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Initialize();

                await _scanner.ConnectAsync();
                await Task.Run(() => _plc.Connect());

                CancellationToken token = _cts.Token;
                _ = Task.Run(() => PLCPollingLoop(token));
                _ = Task.Run(() => SFCPingLoop(token));
                FocusTextBox();
            }
            catch (Exception ex)
            {
                ShowLog($"❌ Lỗi khởi tạo: {ex.Message}", Color.Red);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _cts.Cancel();
            try { _scanner?.Disconnect(); } catch { }
            try { _plc?.Disconnect(); } catch { }
            try { _message?.Dispose(); } catch { }
        }

        private async void BtnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (var settingsForm = new SettingsForm(_configPath))
                {
                    if (settingsForm.ShowDialog() == DialogResult.OK)
                    {
                        SystemConfig oldConfig = _config;
                        SystemConfig newConfig = SystemConfig.Load(_configPath);
                        _config = newConfig;

                        // Bản cũ chỉ đọc lại _config, còn các controller vẫn giữ cấu hình CŨ
                        // (đổi IP PLC, URL SFC... phải khởi động lại mới ăn). Giờ áp dụng ngay:
                        _plc?.UpdateModel(newConfig.PLC);
                        _message?.UpdateModel(newConfig.Message);

                        if (_scanner != null)
                        {
                            bool comChanged = ScannerChanged(oldConfig?.Scanner, newConfig.Scanner);
                            _scanner.UpdateModel(newConfig.Scanner);
                            if (comChanged)
                            {
                                ShowLog("[SCAN] Đổi thông số COM => kết nối lại", Color.Orange);
                                await _scanner.ConnectAsync();
                            }
                        }

                        ShowLog("✅ Đã tải lại và áp dụng cấu hình mới!", Color.Lime);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowLog("❌ Lỗi áp dụng cấu hình: " + ex.Message, Color.Red);
            }
            finally
            {
                FocusTextBox();
            }
        }

        private static bool ScannerChanged(ScannerModel a, ScannerModel b)
        {
            if (a == null || b == null) return true;
            return !string.Equals(a.PortName, b.PortName, StringComparison.OrdinalIgnoreCase)
                || a.BaudRate != b.BaudRate
                || a.DataBits != b.DataBits
                || a.StopBits != b.StopBits
                || a.Parity != b.Parity;
        }
        #endregion
    }
}
