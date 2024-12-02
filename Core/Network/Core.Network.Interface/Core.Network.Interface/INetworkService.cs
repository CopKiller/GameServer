using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface INetworkService
{
    NetworkMode Mode { get; }
    void Register();
    bool Initialize(NetworkMode mode, int port = 0, string? address = null);
    void Update();
    void Stop();
    void Dispose();
}