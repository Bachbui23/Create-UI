using System;

namespace Create_UI.Interfaces
{
    public interface IPLCH3U
    {
        bool IsConnected { get; }
        event Action<bool> OnConnectionChanged;
        event Action<string, ConsoleColor> OnLog;
        event Action OnScanRequest;      // PLC yêu cầu quét
        event Action OnJudgeComplete;    // PLC báo AOI đã xong

        bool Connect();
        void Disconnect();
        bool CheckConnection();

        // Gửi tín hiệu cho PLC
        bool SendScanResult(bool isOK);
        bool SendAOIResult(bool isOK);
        bool SendAOIReady(bool ready);
        bool ResetSignals();

        // Đọc tín hiệu từ PLC
        bool ReadScanRequest();
        bool ReadJudgeComplete();
        int ReadAOIResult();
    }
}