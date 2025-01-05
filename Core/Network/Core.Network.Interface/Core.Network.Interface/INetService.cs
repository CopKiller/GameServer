using System.Net;

namespace Core.Network.Interface;

public interface INetService
{
    void Start();
    void Stop();
    void PollEvents();
    void TriggerEvents();
    void Stop(bool sendDisconnectMessages);
}