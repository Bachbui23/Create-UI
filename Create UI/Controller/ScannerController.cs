using Create_UI.Models;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using UsbScan_Frame4_C22;

namespace Create_UI.Controller
{
    public class ScannerController
    {
        private SerialPort sp;
        private ScanUSBC22 _usbC22;
        private ScannerModel _scannerModel;

        public event Action<string, Color> OnLog;
        public event Action<bool> OnConnected;
        public event Action<string> OnDataReceived;

        public bool IsConnected
        {
            get { return sp != null && sp.IsOpen; }
        }

        // Buffer để ghép gói, tách theo CR/LF
        private readonly StringBuilder _rxBuffer = new StringBuilder(256);
        private readonly object _rxLock = new object();

        // Throttle log “không quét được” để tránh spam
        private DateTime _lastEmptyLog = DateTime.MinValue;

        public ScannerController(ScannerModel model, ScanUSBC22 scan)
        {
            _scannerModel = model;
            _usbC22 = scan;

            string timemax = DateTime.Now.ToString("yyyy");
            int val = 0;
            int.TryParse(timemax, out val);
            val--;
            _usbC22.password = "USBC22_" + 2025;
            _usbC22.Event += usbC22_Event;
            _usbC22.Start();
        }
        public void usbC22_Event(ScanUSBC22.Data codes)
        {
            // Hàm này chạy trên luồng của thư viện USB. Nếu ném exception ra ngoài
            // (bản cũ dùng "throw ex") thì có thể làm sập cả chương trình => chỉ log lại.
            try
            {
                string data = (codes.Result ?? "").Trim();
                OnDataReceived?.Invoke(data);
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("[SCAN] Lỗi xử lý data USB: " + ex.Message, Color.Red);
            }
        }

        /// <summary>Cập nhật cấu hình COM mới (gọi Connect() sau đó để áp dụng).</summary>
        public void UpdateModel(ScannerModel model)
        {
            if (model != null) _scannerModel = model;
        }
        // C#7.3: không cần async ở đây vì không await gì thật sự
        public Task<bool> ConnectAsync()
        {
            return Task.Run(() => Connect());
        }

        public bool Connect()
        {
            try
            {
                Disconnect();

                sp = new SerialPort(
                    _scannerModel.PortName,
                    _scannerModel.BaudRate,
                    _scannerModel.Parity,
                    _scannerModel.DataBits,
                    _scannerModel.StopBits
                );

                sp.DtrEnable = true;
                sp.RtsEnable = true;

                // nếu scanner có CRLF, ta xử lý buffer bằng \r\n
                sp.NewLine = "\r\n";

                sp.DataReceived += Sp_DataReceived;
                sp.Open();
                OnConnected?.Invoke(true);
                return true;
            }
            catch (Exception ex)
            {
                OnConnected?.Invoke(false);
                OnLog?.Invoke("[SCAN] Không thể kết nối: " + ex.Message, Color.Red);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                if (sp != null)
                {
                    try { sp.DataReceived -= Sp_DataReceived; } catch { }
                    if (sp.IsOpen) sp.Close();
                    sp.Dispose();
                    sp = null;
                }
            }
            catch { /* ignore */ }
        }

        // Lưu ý: DataReceived chạy trên thread nền, không nên làm nặng ở đây.
        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (sp == null || !sp.IsOpen) return;

                string chunk = sp.ReadExisting();
                if (string.IsNullOrEmpty(chunk))
                {
                    // throttle
                    if ((DateTime.Now - _lastEmptyLog).TotalSeconds >= 2)
                    {
                        _lastEmptyLog = DateTime.Now;
                        OnLog?.Invoke("[SCAN] Không đọc được dữ liệu", Color.OrangeRed);
                    }
                    return;
                }

                // Ghép vào buffer, tách theo \r\n hoặc \n
                string[] frames = null;

                lock (_rxLock)
                {
                    _rxBuffer.Append(chunk);

                    // chuẩn hoá newline cho dễ tách
                    string all = _rxBuffer.ToString();
                    all = all.Replace("\r\n", "\n");

                    if (all.IndexOf('\n') < 0)
                    {
                        // chưa đủ 1 frame
                        return;
                    }

                    frames = all.Split(new char[] { '\n' }, StringSplitOptions.None);

                    // phần cuối có thể là frame chưa hoàn chỉnh
                    _rxBuffer.Clear();
                    string tail = frames[frames.Length - 1];
                    if (!string.IsNullOrEmpty(tail))
                        _rxBuffer.Append(tail);
                }

                // xử lý tất cả frame hoàn chỉnh (trừ tail)
                for (int i = 0; i < frames.Length - 1; i++)
                {
                    string data = (frames[i] ?? "").Trim();
                    if (data.Length == 0) continue;

                    // Không block thread serial: đẩy sang Task
                    Task.Run(() =>
                    {
                        try
                        {
                            OnLog?.Invoke("[SCAN] Dữ liệu quét: " + data, Color.Cyan);
                            OnDataReceived?.Invoke(data);
                        }
                        catch (Exception ex2)
                        {
                            OnLog?.Invoke("[SCAN] Lỗi xử lý data: " + ex2.Message, Color.Red);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                OnLog?.Invoke("[SCAN] Lỗi đọc dữ liệu: " + ex.Message, Color.Red);
            }
        }
    }
}
