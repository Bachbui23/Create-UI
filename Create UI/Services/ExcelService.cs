using ClosedXML.Excel;
using System;
using System.IO;

namespace Create_UI.Services
{
    public static class ExcelService
    {
        private const string SERIAL_HEADER = "SERIAL";
        private static readonly object _fileLock = new object();

        /// <summary>
        /// Build đường dẫn file Excel theo ngày hiện tại.
        /// </summary>
        public static string BuildFilePath(string folderPath, string fileNamePattern, string filecontact, DateTime? date = null)
        {
            var d = date ?? DateTime.Now;
            string dateFolderName = d.ToString(fileNamePattern);
            string fileName = dateFolderName + ".xlsx";

            return Path.Combine(folderPath, dateFolderName, fileName);
        }

        public static bool AddSerialToRow(string filePath, string position, string serial)
        {
            lock (_fileLock) // chống 2 trạm P1/P2 cùng ghi file 1 lúc gây lỗi mở file
            {
                if (!File.Exists(filePath))
                    return false;

                // Retry ngắn nếu AOI đang giữ file (chưa đóng kịp sau khi ghi)
                for (int attempt = 0; attempt < 5; attempt++)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook(filePath))
                        {
                            var ws = workbook.Worksheet(1);

                            var lastColUsed = ws.LastColumnUsed();
                            var lastRowUsed = ws.LastRowUsed();
                            if (lastColUsed == null || lastRowUsed == null)
                                return false; // sheet rỗng, không có gì để ghi

                            int lastCol = lastColUsed.ColumnNumber();
                            int lastRow = lastRowUsed.RowNumber();
                            if (lastRow < 2)
                                return false; // chỉ có header, chưa có dòng data

                            var headerRow = ws.Row(1);
                            int pCol = 1;
                            int serialCol = -1;

                            for (int c = 1; c <= lastCol; c++)
                            {
                                if (headerRow.Cell(c).GetString().Trim().Equals(SERIAL_HEADER, StringComparison.OrdinalIgnoreCase))
                                {
                                    serialCol = c;
                                    break;
                                }
                            }

                            bool headerAdded = false;
                            if (serialCol == -1)
                            {
                                serialCol = lastCol + 1;
                                ws.Cell(1, serialCol).Value = SERIAL_HEADER;
                                headerAdded = true;
                            }

                            for (int r = lastRow; r >= 2; r--)
                            {
                                string pValue = ws.Cell(r, pCol).GetString().Trim();
                                string currentSerial = ws.Cell(r, serialCol).GetString().Trim();

                                if (pValue.StartsWith(position, StringComparison.OrdinalIgnoreCase)
                                    && string.IsNullOrEmpty(currentSerial))
                                {
                                    ws.Cell(r, serialCol).Value = serial;
                                    workbook.Save();
                                    return true;
                                }
                            }

                            // Không tìm thấy dòng khớp: chỉ save nếu vừa thêm header mới,
                            // tránh mất thao tác thêm cột SERIAL cho lần gọi sau.
                            if (headerAdded)
                                workbook.Save();

                            return false;
                        }
                    }
                    catch (IOException)
                    {
                        // File đang bị khóa bởi AOI -> chờ rồi thử lại
                        System.Threading.Thread.Sleep(300);
                    }
                }
                return false;
            }
        }
    }
}