using Create_UI.Models;
using Create_UI.Services;
using System;
using System.ComponentModel.Composition.Primitives;
using System.IO.Ports;
using System.Windows.Forms;

namespace Create_UI
{
    public partial class SettingsForm : Form
    {
        private readonly string _configPath;
        private SystemConfig _config;
        private ValidationGroupConfig _year;
        private ValidationGroupConfig _month;
        private ValidationGroupConfig _day;

        public SettingsForm(string configPath)
        {
            InitializeComponent();
            _configPath = configPath;

            LoadConfig();
        }

        private void LoadConfig()
        {
            _year = ValidationGroupConfig.CreateValiYear();
            _month = ValidationGroupConfig.CreateValiMonth();
            _day = ValidationGroupConfig.CreateValiDay();
            _config = SystemConfig.Load(_configPath);

            // ===== SCANNER =====
            txtPort.Text = _config.Scanner.PortName;
            txtBaudrate.Text = _config.Scanner.BaudRate.ToString();
            txtDataBits.Text = _config.Scanner.DataBits.ToString();
            cboStopBits.SelectedItem = _config.Scanner.StopBits.ToString();
            cboParity.SelectedItem = _config.Scanner.Parity.ToString();

            // ===== PLC =====
            txtPLCIP.Text = _config.PLC.PLC_Id;
            txtPLCPort.Text = _config.PLC.PLC_Port.ToString();
            txtDMStart.Text = _config.PLC.DM_Start;
            txtDMAOIResult.Text = _config.PLC.DM_AOIResult;
            txt_NoScan.Text = _config.PLC.DM_NoScan;
            txtPolling.Text = _config.PLC.PollingInterval.ToString();

            // ===== SFC / HTTP =====
            txtIpSfc.Text = _config.Message.IpSfc;
            txtUrlGet.Text = _config.Message.UrlGet;
            txtUrlPostSFC.Text = _config.Message.UrlPost;
            chkEnableGet.Checked = _config.Message.EnableGet;
            chkEnablePost.Checked = _config.Message.EnablePost;
            txtStation.Text = _config.Message.StationName;
            txt_version.Text = _config.Message.Version;

            // ===== VALIDATION DŨ =====
            chkEnableValidation.Checked = _config.Validation.EnableValidation;
            txtLength.Text = _config.Validation.Length.ToString();
            txtStartChar.Text = _config.Validation.StartChar;
            txtRegex.Text = _config.Validation.RegexPattern;
            txt_Year.Text = _config.Validation.YearChar;
            txt_Month.Text = _config.Validation.MonthChar;
            txt_Day.Text = _config.Validation.DayChar;
            ck_SetTime.Checked = _config.Validation.SetDateTime;

            // Đồng bộ trạng thái enable/disable của 3 combobox theo checkbox
            // (LoadConfig set ck_SetTime.Checked ở trên KHÔNG tự bắn CheckedChanged
            // nếu giá trị mới == giá trị cũ, nên phải gọi tay để chắc chắn đúng trạng thái)
            UpdateDateTimeControlsState();

            // ===== ĐỒNG BỘ HIỂN THỊ META CẤU HÌNH MỚI LÊN UI =====
            chkIsMetaAsn.Checked = _config.Validation.IsMetaAsnFormat;
            txtAllowedVendor.Text = _config.Validation.AllowedVendor;
            txtAllowedParts.Text = _config.Validation.AllowedParts;
            txtForbiddenChars.Text = _config.Validation.ForbiddenMetaChars;
        }

        /// <summary>
        /// Bật/tắt cbx_Year, cbx_Month, cbx_Day theo trạng thái ck_SetTime.
        /// Tích = cho chọn thủ công. Bỏ tích = khóa lại, dùng DateTime.Now.
        /// </summary>
        private void UpdateDateTimeControlsState()
        {
            bool manualSet = ck_SetTime.Checked;
            cbx_Year.Enabled = manualSet;
            cbx_Month.Enabled = manualSet;
            cbx_Day.Enabled = manualSet;
        }
        private DateTime? GetSelectedDate()
        {
            if (!int.TryParse(cbx_Year.Text, out int year))
                return null;

            if (!int.TryParse(cbx_Month.Text, out int month))
                return null;

            if (!int.TryParse(cbx_Day.Text, out int day))
                return null;

            try
            {
                return new DateTime(year, month, day);
            }
            catch
            {
                return null;
            }
        }
        private void SetorCheckTime()
        {
            string year;
            string month;
            string day;

            if (_config.Validation.SetDateTime)
            {
                DateTime? selectedDate = GetSelectedDate();

                if (selectedDate == null)
                    throw new Exception("Ngày tháng năm không hợp lệ!");

                if (selectedDate.Value.Date > DateTime.Today)
                    throw new Exception("Không được chọn ngày lớn hơn ngày hiện tại!");

                _config.Validation.ExpireDate =
                    selectedDate.Value.ToString("yyyy-MM-dd");

                year = selectedDate.Value.ToString("yyyy");
                month = selectedDate.Value.ToString("MM");
                day = selectedDate.Value.ToString("dd");
            }
            else
            {
                _config.Validation.ExpireDate = "";

                DateTime now = DateTime.Now;

                year = now.ToString("yyyy");
                month = now.ToString("MM");
                day = now.ToString("dd");
            }

            _config.Validation.YearChar =
                _year._dataYear.TryGetValue(year, out string y) ? y : "";

            _config.Validation.MonthChar =
                _month._dataMoth.TryGetValue(month, out string m) ? m : "";

            _config.Validation.DayChar =
                _day._dataDay.TryGetValue(day, out string d) ? d : "";
        }
        private void SaveConfig()
        {
            try
            {
                // ===== SCANNER =====
                _config.Scanner.PortName = txtPort.Text;
                _config.Scanner.BaudRate = int.Parse(txtBaudrate.Text);
                _config.Scanner.DataBits = int.Parse(txtDataBits.Text);
                _config.Scanner.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.SelectedItem.ToString());
                _config.Scanner.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.SelectedItem.ToString());

                // ===== PLC =====
                // Với CIP: DM_... là TÊN BIẾN trong Sysmac Studio (có phân biệt khoảng trắng => Trim)
                _config.PLC.PLC_Id = txtPLCIP.Text.Trim();
                _config.PLC.PLC_Port = int.Parse(txtPLCPort.Text.Trim());
                _config.PLC.DM_Start = txtDMStart.Text.Trim();
                _config.PLC.DM_AOIResult = txtDMAOIResult.Text.Trim();
                _config.PLC.DM_NoScan = txt_NoScan.Text.Trim();
                _config.PLC.PollingInterval = int.Parse(txtPolling.Text.Trim());

                if (_config.PLC.PLC_Port != 44818)
                {
                    var ans = MessageBox.Show(
                        $"Port PLC đang là {_config.PLC.PLC_Port}.\nOmron EtherNet/IP (CIP) dùng cổng 44818.\n\nVẫn lưu?",
                        "Kiểm tra Port PLC", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (ans != DialogResult.Yes) return;
                }

                // ===== SFC / HTTP =====
                _config.Message.IpSfc = txtIpSfc.Text;
                _config.Message.UrlGet = txtUrlGet.Text;
                _config.Message.UrlPost = txtUrlPostSFC.Text;
                _config.Message.EnableGet = chkEnableGet.Checked;
                _config.Message.EnablePost = chkEnablePost.Checked;
                _config.Message.StationName = txtStation.Text;
                _config.Message.Version = txt_version.Text;

                // ===== VALIDATION CŨ =====
                _config.Validation.EnableValidation = chkEnableValidation.Checked;
                _config.Validation.Length = int.Parse(txtLength.Text);
                _config.Validation.StartChar = txtStartChar.Text;
                _config.Validation.RegexPattern = txtRegex.Text;

                // ===== LƯU DỮ LIỆU CẤU HÌNH META MỚI TỪ UI XUỐNG FILE =====
                _config.Validation.IsMetaAsnFormat = chkIsMetaAsn.Checked;
                _config.Validation.AllowedVendor = txtAllowedVendor.Text.Trim();
                _config.Validation.AllowedParts = txtAllowedParts.Text.Trim();
                _config.Validation.ForbiddenMetaChars = txtForbiddenChars.Text.Trim();

                // Đồng bộ trạng thái checkbox lên config TRƯỚC khi SetorCheckTime() dùng nó,
                // nếu không SetorCheckTime sẽ đọc giá trị SetDateTime cũ (lúc LoadConfig).
                _config.Validation.SetDateTime = ck_SetTime.Checked;

                SetorCheckTime();
                // Lưu xuống file json/xml config
                _config.Save(_configPath);
                MessageBox.Show("✅ Lưu cấu hình thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi lưu cấu hình: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetConfig()
        {
            var result = MessageBox.Show("Bạn có chắc muốn reset về mặc định?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _config = new SystemConfig();
                _config.Save(_configPath);
                LoadConfig();
                MessageBox.Show("✅ Đã reset về cấu hình mặc định!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetConfig();
        }

        private void Ck_SetTime_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDateTimeControlsState();
        }
    }
}