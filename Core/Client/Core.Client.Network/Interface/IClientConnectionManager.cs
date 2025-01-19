using Core.Network.Interface;

namespace Core.Client.Network.Interface;

public interface IClientConnectionManager
{
    void ConfigureNetworkSettings();
    bool IsConnected { get; }
    IAdapterNetPeer? CurrentPeer { get; set; }
    void ConnectToServer();
    void DisconnectFromServer();
}