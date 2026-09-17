# Ghi chú kết nối PLC (Omron EtherNet/IP - CIP) và các sửa đổi

## 1. Chương trình nói chuyện với PLC như thế nào

```
Scanner ──(mã)──> Form1.Scanner_OnDataReceived
                     │ kiểm tra định dạng + hỏi SFC (GetSN)
                     ├─ sai  → ghi DM_Start = 2   (NG)
                     └─ đúng → ghi DM_AOIResult = 0, rồi DM_Start = 1 (OK)

Vòng lặp nền PLCPollingLoop (mỗi PollingInterval ms):
    đọc DM_NoScan  (vừa là tín hiệu, vừa để biết PLC còn sống)
    nếu DM_NoScan == 0 và đang có mã chờ:
        đọc DM_AOIResult: 1 = OK, số khác 0 = NG
        → POST kết quả lên SFC → ghi mã vào file Excel → ghi DM_AOIResult = 0
```

## 2. Bắt buộc phải kiểm tra khi dùng CIP (khác với FINS cũ)

| Mục | FINS (bản cũ, SYS.INI) | CIP (bản hiện tại) |
|---|---|---|
| Class HslCommunication | `OmronFinsNet` | `OmronCipNet` |
| Cổng | 9600 | **44818** |
| Địa chỉ | Vùng nhớ: `D300` | **Tên biến (tag)** trong Sysmac Studio |

1. **Cổng (Port) = 44818.** File `bin\Debug\Config\appsettings.json` đang để `502` (cổng Modbus), đã sửa thành `44818`.
2. **DM_Start / DM_AOIResult / DM_NoScan phải là TÊN BIẾN**, không phải số.
   Giá trị `"40"`, `"42"`, `"44"` hiện tại **không đọc được** qua CIP (tên biến không được bắt đầu bằng chữ số).
   Cách làm trong Sysmac Studio:
   - Tạo biến toàn cục (Global Variables), ví dụ `PC_Start`, `PC_AOIResult`, `PC_NoScan`.
     Nếu muốn giữ vùng D cũ thì đặt AT = `%D40`, `%D42`, `%D44` và đặt tên là `D40`, `D42`, `D44`.
   - Kiểu dữ liệu: **INT**.
   - Cột **Network Publish**: chọn `Input`/`Output` (hoặc `Publish Only`). Không bật thì PC không thấy biến.
   - Nhập đúng các tên đó vào nút ⚙ Settings của chương trình.
3. **Bản quyền HslCommunication.** Đây là thư viện thương mại. Theo tài liệu đi kèm thư viện, nếu không kích hoạt
   thì chỉ chạy được vài giờ rồi **mọi giao tiếp sẽ lỗi** (PLC báo mất kết nối mà không rõ lý do).
   Nếu có mã, điền vào `"HslAuthCode"` trong `appsettings.json` (phần `PLC`).

Khi chương trình khởi động, log sẽ tự cảnh báo (màu cam) nếu Port khác 44818 hoặc tên tag bắt đầu bằng số.

## 3. Các lỗi đã sửa

### PLCController.cs (viết lại)
- **Kiểm tra PLC bằng tag `"0"`** → với CIP tag này không tồn tại nên bản cũ luôn báo *Offline* rồi kết nối lại liên tục.
  Nay dùng chính lần đọc `DM_NoScan` làm "heartbeat".
- **Nguy cơ treo chương trình (deadlock):** bản cũ gọi event (log, đổi màu nút) *trong khi đang giữ khoá PLC*,
  mà giao diện dùng `Invoke` (chờ). Nếu đúng lúc đó giao diện cũng đang chờ khoá (ví dụ lúc đóng form) → treo cứng.
  Nay mọi event được gọi *sau khi* nhả khoá.
- **Phân biệt 2 loại lỗi:**
  - PLC có trả lời nhưng báo lỗi (sai tên tag, sai kiểu) → vẫn coi là còn kết nối, log 1 lần kèm gợi ý tiếng Việt.
  - Lỗi mạng/timeout → sau 3 lần liên tiếp mới coi là mất kết nối và tự kết nối lại.
- Lỗi ghi PLC **luôn được log** (bản cũ chỉ log khi `VerboseLog = true` nên lỗi bị "nuốt").
- Thêm `WriteInt16` / `TryReadInt16` rõ kiểu INT. Các hàm cũ (`WriteValue`, `ReadValue`...) vẫn giữ để code cũ chạy.
- Thêm `UpdateModel()`: đổi IP/Port trong Settings là kết nối lại ngay, không cần tắt chương trình.

### Form1.cs
- `Scanner_OnDataReceived` là `async void`: nếu có exception thì **sập cả chương trình**, và `_isProcessing` bị kẹt
  `true` mãi → không quét được nữa. Nay có `try/catch/finally`.
- Ghi PLC thất bại (PLC chưa kết nối) mà bản cũ vẫn log "Gửi tín hiệu OK" → nay kiểm tra kết quả và báo đỏ.
- Xoá `DM_AOIResult` **trước** khi báo `DM_Start = 1`, tránh đọc nhầm kết quả cũ cho mã mới.
- `CheckStationResult`: nếu ghi Excel lỗi, bản cũ không xoá mã → **POST lặp lại mỗi 3 giây**. Nay lấy mã ra và xoá ngay,
  và luôn trả `DM_AOIResult` về 0 (khối `finally`).
- Log: dùng `BeginInvoke` (không chờ), cắt bớt dòng cũ mà **không mất màu**, bỏ log bị lặp đôi (`[PLC] [PLC] ...`).
- Màu log mặc định từng là `Color.Black` trên nền đen (không đọc được) → đổi sang `LightGray`.
- Sau khi lưu Settings, cấu hình mới được **áp dụng ngay** cho PLC, SFC, COM (bản cũ chỉ đọc lại file,
  các controller vẫn dùng cấu hình cũ cho đến khi khởi động lại).
- Đóng form: dừng 2 vòng lặp nền bằng `CancellationToken`.
- Chặn lỗi `Substring` khi `Length` trong Settings đặt nhỏ hơn 14 mà vẫn bật Meta ASN.

### Các file khác
- `ScannerController.cs`: bỏ `throw ex` trong sự kiện USB (có thể làm sập chương trình), thêm `UpdateModel`.
- `MessageController.cs`: thêm `UpdateModel`; lần ping đầu tiên luôn log trạng thái SFC.
- `SystemConfig.cs`: nếu `appsettings.json` hỏng, log lý do thay vì lặng lẽ dùng cấu hình mặc định.
- `SettingsForm.cs`: `Trim()` tên tag/IP; hỏi lại nếu Port khác 44818.
- `PLCModel.cs`: thêm chú thích + trường `HslAuthCode`.

## 4. Chưa sửa (cần bạn quyết định)
- `RegexPattern` có trong Settings nhưng **chưa được dùng** để kiểm tra mã.
- `start_time` và `stop_time` gửi lên SFC đang bằng nhau (cùng thời điểm có kết quả AOI).
- Nếu POST SFC thất bại thì kết quả không được gửi lại (chỉ báo đỏ trong log).
- Nếu quét mã mới khi mã trước chưa có kết quả AOI, mã trước bị thay (nay đã có log cảnh báo màu cam).
- Thư mục `Interfaces\` và `Models\AOIModel.cs` không nằm trong project (không được biên dịch).
- Nếu `OmronCipNet` chạy không ổn định với NX1P2/NX102, có thể thử `OmronConnectedCipNet` (cùng thư viện).
