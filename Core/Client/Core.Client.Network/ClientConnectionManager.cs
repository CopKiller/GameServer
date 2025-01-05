using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Event;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientConnectionManager(
    IConnectionManager connectionManager, 
    INetworkEventsListener listener,
    INetworkSettings networkSettings,
    ILogger<ClientConnectionManager> logger) : IClientConnectionManager
{

    public bool IsConnected => CurrentPeer is { IsConnected: true };
    public IAdapterNetPeer? CurrentPeer { get; set; }

    public void ConfigureNetworkSettings()
    {
        networkSettings.AutoRecycle = true;
        networkSettings.EnableStatistics = false;
        networkSettings.UnconnectedMessagesEnabled = false;
        networkSettings.UseNativeSockets = true;

        networkSettings.Address = "127.0.0.1";
        networkSettings.Port = 9050;
        networkSettings.Key = "key";
        
        listener.OnPeerDisconnected += OnPeerDisconnected;
    }

    public void ConnectToServer()
    {
        if (CurrentPeer is not null)
            if (CurrentPeer.IsConnected)
            {
                logger.LogError("Server peer is already connected.");
                return;
            }

        CurrentPeer = connectionManager
            .ConnectToServer(
                networkSettings.Address,
                networkSettings.Port, 
                networkSettings.Key);
    }
    
    private void OnPeerDisconnected(IAdapterNetPeer peer, IAdapterDisconnectInfo disconnectInfo)
    {
        CurrentPeer = null;
    }
    
    public void DisconnectFromServer()
    {
        connectionManager.DisconnectAll();
        CurrentPeer = null;
    }
}