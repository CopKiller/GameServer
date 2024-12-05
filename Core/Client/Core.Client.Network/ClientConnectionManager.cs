using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Connection;

namespace Core.Client.Network;

public class ClientConnectionManager(
    IConnectionManager connectionManager) : IClientConnectionManager
{
    public void Disconnect() => connectionManager.DisconnectAll();

    public ICustomNetPeer GetServerPeer() => connectionManager.GetFirstPeer();
}