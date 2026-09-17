using System;
using System.Threading.Tasks;

namespace Create_UI.Interfaces
{
    public interface IScanner
    {
        bool IsConnected { get; }
        event Action<string> OnDataReceived;
        event Action<bool> OnConnectionChanged;
        event Action<string, ConsoleColor> OnLog;

        Task<bool> ConnectAsync();
        void Disconnect();
        bool TriggerScan();
    }
}