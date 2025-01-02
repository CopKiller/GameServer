using Core.Network.Interface;

namespace Core.Client.Network.Interface;

public interface IClientConnectionManager
{
    bool IsConnected { get; }
    ICustomNetPeer? CurrentPeer { get; set; }
    void ConnectToServer();
    void DisconnectFromServer();
}