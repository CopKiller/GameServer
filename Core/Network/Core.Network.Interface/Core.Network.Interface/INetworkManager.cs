namespace Core.Network.Interface;

public interface INetworkManager
{
    void Register();
    void Start();
    void Stop();
    void Update();
    void Dispose();
}