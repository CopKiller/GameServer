using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Event;
using Core.Server.Network.Interface;

namespace Core.Server.Network;

public class ServerConnectionManager(
    IConnectionManager connectionManager, 
    INetworkSettings networkSettings) : IServerConnectionManager
{

    public void ConfigureNetworkSettings()
    {
        networkSettings.AutoRecycle = true;
        networkSettings.EnableStatistics = false;
        networkSettings.UnconnectedMessagesEnabled = false;
        networkSettings.UseNativeSockets = true;
        
        networkSettings.Address = "127.0.0.1";
        networkSettings.Port = 9050;
        networkSettings.Key = "key";
        
        connectionManager.RegisterEvents();
    }
    
    public bool StartListener()
    {
        return connectionManager.StartListener(networkSettings.Port);
    }

    public void DisconnectPeer(IAdapterNetPeer peer, string reason = "Disconnected")
    {
        connectionManager.DisconnectPeer(peer);
    }

    public void DisconnectAll()
    {
        connectionManager.DisconnectAll();
    }

    public bool HasConnectedPeers => connectionManager.HasConnectedPeers;

    public IAdapterNetPeer? GetPeerById(int id)
    {
        return connectionManager.GetPeerById(id);
    }
}