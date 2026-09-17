using System;
using System.Collections.Generic;

namespace Create_UI.Models
{
    [Serializable]
    public class AOIModel
    {
        public string Serial { get; set; } = "";
        public string Result { get; set; } = "";  // "OK" hoặc "NG"
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // ===== DỮ LIỆU ĐO LƯỜNG =====
        public Dictionary<string, object> Measurements { get; set; } = new Dictionary<string, object>();

        // ===== THÔNG TIN FILE =====
        public string ExcelFilePath { get; set; } = "";

        // ===== HÀM TIỆN ÍCH =====
        public bool IsOK => Result == "OK";

        public override string ToString()
        {
            return $"Serial: {Serial}, Result: {Result}, Measurements: {Measurements.Count}";
        }

        /// <summary>
        /// Lấy giá trị đo theo tên cột
        /// </summary>
        public T GetMeasurement<T>(string columnName)
        {
            if (Measurements.TryGetValue(columnName, out var value))
            {
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Lấy tất cả giá trị đo dạng số
        /// </summary>
        public Dictionary<string, double> GetNumericMeasurements()
        {
            var result = new Dictionary<string, double>();
            foreach (var kv in Measurements)
            {
                if (kv.Value is double d)
                    result[kv.Key] = d;
                else if (kv.Value is int i)
                    result[kv.Key] = i;
                else if (kv.Value is float f)
                    result[kv.Key] = f;
                else if (double.TryParse(kv.Value?.ToString(), out double parsed))
                    result[kv.Key] = parsed;
            }
            return result;
        }
    }
}