using Create_UI.Models;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Create_UI.Controller
{
    public class PLCController
    {
        public const int DefaultCipPort = 44818;

        // Sau bao nhiêu lần đọc/ghi lỗi LIÊN TIẾP thì coi như mất kết nối
        private const int MaxConsecutiveErrors = 3;

        private OmronCipNet _omron;
        private PLCModel _plcModel;
        private readonly object _plcLock = new object();

        private volatile bool _isConnected;
        private int _errorCount;          // số lần lỗi liên tiếp (chỉ sửa trong lock)
        private string _lastErrorMessage; // chống spam: cùng 1 lỗi chỉ log 1 lần cho mỗi lần kết nối

        /// <summary>true = đang có phiên kết nối CIP tới PLC.</summary>
        public bool IsConnected { get { return _isConnected; } }

        /// <summary>Bật lên để log cả các lần ghi thành công (debug).</summary>
        public bool VerboseLog { get; set; } = false;

        /// <summary>Log ra màn hình. Nội dung đã có sẵn tiền tố "[PLC]".</summary>
        public event Action<string, Color> OnLog;

        /// <summary>Chỉ bắn khi trạng thái kết nối THAY ĐỔI (true = kết nối, false = mất).</summary>
        public event Action<bool> OnConnected;

        public PLCController(PLCModel model)
        {
            _plcModel = model ?? new PLCModel();
        }

        #region Connect / Disconnect
        public bool Connect()
        {
            PLCModel m = _plcModel;
            WarnIfConfigLooksWrong(m);

            bool ok = false;
            string error = null;

            lock (_plcLock)
            {
                try
                {
                    var newClient = new OmronCipNet(m.PLC_Id, m.PLC_Port)
                    {
                        ConnectTimeOut = 2000,
                        ReceiveTimeOut = 2000
                    };

                    OperateResult result = newClient.ConnectServer();
                    if (result.IsSuccess)
                    {
                        if (_omron != null) _omron.ConnectClose();
                        _omron = newClient;
                        _errorCount = 0;
                        _lastErrorMessage = null;
                        ok = true;
                    }
                    else
                    {
                        newClient.ConnectClose();
                        error = result.Message;
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }

            // Gọi event SAU KHI đã nhả lock
            if (ok)
            {
                SetConnected(true, string.Format("[PLC] Kết nối {0}:{1} (CIP) thành công", m.PLC_Id, m.PLC_Port));
            }
            else
            {
                SetConnected(false, null);
                LogOnce(string.Format("[PLC] Kết nối {0}:{1} thất bại: {2}", m.PLC_Id, m.PLC_Port, error), Color.Red);
            }
            return ok;
        }

        public async Task<bool> ConnectAsync(int maxRetries = 3, int delayMs = 1500)
        {
            for (int i = 1; i <= maxRetries; i++)
            {
                bool ok = await Task.Run(() => Connect());
                if (ok) return true;
                if (i < maxRetries) await Task.Delay(delayMs);
            }
            return false;
        }

        public void Disconnect()
        {
            lock (_plcLock)
            {
                if (_omron != null)
                {
                    try { _omron.ConnectClose(); } catch { }
                    _omron = null;
                }
            }
            SetConnected(false, "[PLC] Ngắt kết nối");
        }

        /// <summary>
        /// Cập nhật cấu hình mới (sau khi bấm Lưu ở SettingsForm).
        /// Nếu IP hoặc Port thay đổi thì ngắt kết nối để vòng polling tự kết nối lại bằng thông số mới.
        /// </summary>
        public void UpdateModel(PLCModel model)
        {
            if (model == null) return;
            PLCModel old = _plcModel;
            _plcModel = model;

            bool endpointChanged = old == null
                || !string.Equals(old.PLC_Id, model.PLC_Id, StringComparison.OrdinalIgnoreCase)
                || old.PLC_Port != model.PLC_Port;

            if (endpointChanged && IsConnected)
            {
                Log("[PLC] Đổi IP/Port => kết nối lại với thông số mới", Color.Orange);
                Disconnect();
            }
            _configWarned = false; // cấu hình mới => kiểm tra & cảnh báo lại
            lock (_plcLock) { _lastErrorMessage = null; }
            WarnIfConfigLooksWrong(model);
        }

        /// <summary>
        /// Kiểm tra PLC còn sống: đọc thử tag DM_NoScan (tag chắc chắn có trong PLC).
        /// Bản cũ đọc tag "0" => với CIP tag này không tồn tại nên luôn báo Offline.
        /// </summary>
        public bool CheckPLC()
        {
            short dummy;
            return TryReadInt16(_plcModel.DM_NoScan, out dummy);
        }
        #endregion

        #region Read (kiểu Try..: không ném exception, trả về true/false)
        public bool TryReadInt16(string tag, out short value)
        {
            return TryRead(tag, c => c.ReadInt16(tag), out value);
        }

        public bool TryReadUInt16(string tag, out ushort value)
        {
            return TryRead(tag, c => c.ReadUInt16(tag), out value);
        }

        public bool TryReadFloat(string tag, out float value)
        {
            return TryRead(tag, c => c.ReadFloat(tag), out value);
        }

        public bool TryReadBit(string tag, out bool value)
        {
            return TryRead(tag, c => c.ReadBool(tag), out value);
        }
        #endregion

        #region Read (kiểu cũ: lỗi thì ném exception) - giữ lại để code cũ vẫn chạy
        public int ReadInt16(string tag)
        {
            short v;
            if (!TryReadInt16(tag, out v)) throw new InvalidOperationException("Đọc PLC lỗi: " + tag);
            return v;
        }

        public uint ReadValue(string tag)
        {
            ushort v;
            if (!TryReadUInt16(tag, out v)) throw new InvalidOperationException("Đọc PLC lỗi: " + tag);
            return v;
        }

        public float ReadFloat(string tag)
        {
            float v;
            if (!TryReadFloat(tag, out v)) throw new InvalidOperationException("Đọc PLC lỗi: " + tag);
            return v;
        }

        public bool ReadBit(string tag)
        {
            bool v;
            if (!TryReadBit(tag, out v)) throw new InvalidOperationException("Đọc PLC lỗi: " + tag);
            return v;
        }
        #endregion

        #region Write
        /// <summary>Ghi số nguyên 16 bit có dấu (biến PLC kiểu INT). Đây là hàm nên dùng cho các thanh ghi D.</summary>
        public bool WriteInt16(string tag, short value)
        {
            return DoWrite(tag, value.ToString(), c => c.Write(tag, value));
        }

        /// <summary>Ghi số nguyên 16 bit không dấu (biến PLC kiểu UINT / WORD).</summary>
        public bool WriteUInt16(string tag, ushort value)
        {
            return DoWrite(tag, value.ToString(), c => c.Write(tag, value));
        }

        // Các overload cũ giữ lại cho tương thích.
        // Chú ý: WriteValue(tag, 1) với số nguyên sẽ rơi vào bản "short" (INT).
        public bool WriteValue(string tag, short value) { return WriteInt16(tag, value); }
        public bool WriteValue(string tag, uint value) { return WriteUInt16(tag, (ushort)value); }

        public bool WriteValue(string tag, float value)
        {
            return DoWrite(tag, value.ToString(), c => c.Write(tag, value));
        }

        public bool WriteValue(string tag, bool value)
        {
            return DoWrite(tag, value.ToString(), c => c.Write(tag, value));
        }

        /// <summary>Ghi state rồi ghi !state ngay sau đó (bit dạng xung).</summary>
        public bool ToggleBit(string tag, bool state)
        {
            return WriteValue(tag, state) && WriteValue(tag, !state);
        }

        /// <summary>Reset tất cả tín hiệu về 0 (hiện chưa dùng, giữ nguyên như bản cũ).</summary>
        public bool ResetSignals()
        {
            return true;
        }
        #endregion

        #region Helper
        private bool TryRead<T>(string tag, Func<OmronCipNet, OperateResult<T>> reader, out T value)
        {
            value = default(T);
            if (string.IsNullOrWhiteSpace(tag)) return false;

            string error = null;
            bool lost = false;

            lock (_plcLock)
            {
                if (!_isConnected || _omron == null) return false;
                try
                {
                    OperateResult<T> r = reader(_omron);
                    if (r.IsSuccess)
                    {
                        value = r.Content;
                        _errorCount = 0;
                        return true;
                    }
                    error = DescribeError(r);
                    lost = IsPlcAnswer(r) ? false : RegisterErrorLocked();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    lost = RegisterErrorLocked();
                }
            }

            LogOnce(string.Format("[PLC] Đọc tag '{0}' lỗi: {1}", tag, error), Color.Red);
            if (lost) SetConnected(false, "[PLC] Mất kết nối (lỗi liên tiếp), sẽ tự kết nối lại...");
            return false;
        }

        private bool DoWrite(string tag, string valueText, Func<OmronCipNet, OperateResult> writer)
        {
            if (string.IsNullOrWhiteSpace(tag)) return false;

            string error = null;
            bool lost = false;

            lock (_plcLock)
            {
                if (!_isConnected || _omron == null)
                {
                    error = "PLC chưa kết nối";
                }
                else
                {
                    try
                    {
                        OperateResult r = writer(_omron);
                        if (r.IsSuccess)
                        {
                            _errorCount = 0;
                        }
                        else
                        {
                            error = DescribeError(r);
                            lost = IsPlcAnswer(r) ? false : RegisterErrorLocked();
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        lost = RegisterErrorLocked();
                    }
                }
            }

            if (error == null)
            {
                if (VerboseLog) Log(string.Format("[PLC] Ghi {0}={1} OK", tag, valueText), Color.Green);
                return true;
            }

            // Lỗi ghi luôn được log (bản cũ chỉ log khi VerboseLog => lỗi bị "nuốt")
            Log(string.Format("[PLC] Ghi {0}={1} lỗi: {2}", tag, valueText, error), Color.Red);
            if (lost) SetConnected(false, "[PLC] Mất kết nối (lỗi liên tiếp), sẽ tự kết nối lại...");
            return false;
        }

        /// <summary>
        /// ErrorCode &gt; 0 là mã trạng thái CIP do CHÍNH PLC trả về => đường truyền vẫn sống,
        /// chỉ là sai tên tag / sai kiểu dữ liệu... => KHÔNG tính là mất kết nối.
        /// ErrorCode &lt;= 0 là lỗi socket / timeout của thư viện => mới tính là mất kết nối.
        /// </summary>
        private static bool IsPlcAnswer(OperateResult r)
        {
            return r != null && r.ErrorCode > 0;
        }

        /// <summary>Thêm gợi ý tiếng Việt cho các mã lỗi CIP hay gặp.</summary>
        private static string DescribeError(OperateResult r)
        {
            if (r == null) return "Không rõ lỗi";
            switch (r.ErrorCode)
            {
                case 0x04:
                case 0x05:
                    return r.Message + " => PLC không có tag này (sai tên, hoặc biến chưa bật Network Publish trong Sysmac Studio)";
                case 0x08:
                    return r.Message + " => PLC không hỗ trợ lệnh này";
                case 0x0F:
                    return r.Message + " => Không có quyền truy cập tag (kiểm tra Network Publish: Input/Output)";
                case 0x13:
                case 0x15:
                case 0xFF:
                    return r.Message + " => Có thể sai KIỂU DỮ LIỆU: biến trong PLC phải là INT";
                default:
                    return r.Message;
            }
        }

        /// <summary>Gọi trong lock. Trả về true nếu vừa vượt ngưỡng lỗi => coi như mất kết nối.</summary>
        private bool RegisterErrorLocked()
        {
            _errorCount++;
            if (_errorCount < MaxConsecutiveErrors) return false;

            if (_omron != null)
            {
                try { _omron.ConnectClose(); } catch { }
                _omron = null;
            }
            _errorCount = 0;
            return true;
        }

        /// <summary>Đổi trạng thái kết nối; chỉ bắn event + log khi trạng thái thực sự thay đổi.</summary>
        private void SetConnected(bool connected, string message)
        {
            bool changed;
            lock (_plcLock)
            {
                changed = _isConnected != connected;
                _isConnected = connected;
            }
            if (!changed) return;

            if (!string.IsNullOrEmpty(message))
                Log(message, connected ? Color.Lime : Color.Red);

            var h = OnConnected;
            if (h != null) h(connected);
        }

        private void LogOnce(string message, Color color)
        {
            lock (_plcLock)
            {
                if (message == _lastErrorMessage) return;
                _lastErrorMessage = message;
            }
            Log(message, color);
        }

        private void Log(string message, Color color)
        {
            var h = OnLog;
            if (h != null) h(message, color);
        }

        private bool _configWarned;
        private void WarnIfConfigLooksWrong(PLCModel m)
        {
            if (m == null || _configWarned) return;
            _configWarned = true;

            if (m.PLC_Port != DefaultCipPort)
                Log(string.Format("[PLC] Cảnh báo: Port = {0}. EtherNet/IP (CIP) của Omron dùng cổng {1}.",
                    m.PLC_Port, DefaultCipPort), Color.Orange);

            CheckTagName("DM_Start", m.DM_Start);
            CheckTagName("DM_AOIResult", m.DM_AOIResult);
            CheckTagName("DM_NoScan", m.DM_NoScan);
        }

        private void CheckTagName(string field, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                Log(string.Format("[PLC] Cảnh báo: {0} đang để trống.", field), Color.Orange);
                return;
            }
            // Tên biến IEC 61131-3 không được bắt đầu bằng chữ số.
            if (char.IsDigit(tag.Trim()[0]))
            {
                Log(string.Format("[PLC] Cảnh báo: {0} = '{1}' không phải tên biến hợp lệ cho CIP. " +
                    "Hãy nhập đúng TÊN BIẾN trong Sysmac Studio (ví dụ: D{1} hoặc PC_Start).", field, tag.Trim()),
                    Color.Orange);
            }
        }
        #endregion
    }
}
