namespace Core.Network.Interface;

public interface INetworkService
{
    void Register();
    void StartServer(int port);
    void StartClient();
    void ConnectToServer(string address, int port);
    void Update();
    void Stop();
    void Dispose();
}