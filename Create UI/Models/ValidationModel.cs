using System;
using System.Collections.Generic;

namespace Create_UI.Models
{
    [Serializable]
    public class ValidationModel
    {
        public string ExpireDate { get; set; }
        public bool EnableValidation { get; set; } = true;
        public int Length { get; set; } = 14; // Mặc định Meta cố định 15
        public string StartChar { get; set; } = "4S"; // Ký tự bắt đầu dự án
        public string RegexPattern { get; set; } = @"^[A-Z0-9]+$";
        public string AllowedChars { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public bool CaseSensitive { get; set; } = false;

        // ===== CẤU HÌNH QUẢN CHẾ MỚI CHO META ASN =====
        public bool IsMetaAsnFormat { get; set; } = true; // Bật tắt cơ chế quản chế Meta
        public string AllowedVendor { get; set; } = "BS"; // Cấu hình nhà cung cấp
        public string AllowedParts { get; set; } = "231"; // Các Part cho phép, cách nhau dấu phẩy
        public string ForbiddenMetaChars { get; set; } = "AEIOU"; // Ký tự cấm xuất hiện tại Năm/Tháng/Ngày/Unique
        public string YearChar { get; set; }
        public string MonthChar { get; set; }
        public string DayChar { get; set; }
        public bool SetDateTime { get; set; } = true;
    }
}