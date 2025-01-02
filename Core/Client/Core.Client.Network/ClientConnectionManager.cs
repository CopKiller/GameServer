using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientConnectionManager : IClientConnectionManager
{
    private readonly IConnectionManager _connectionManager;
    private readonly INetworkEventsListener _listener;
    private readonly INetworkManager _networkManager;
    private readonly INetworkConfiguration _networkConfiguration;
    private readonly ILogger<ClientConnectionManager> _logger;

    public bool IsConnected => CurrentPeer is { IsConnected: true };
    public ICustomNetPeer? CurrentPeer { get; set; }
    
    public ClientConnectionManager(
        IConnectionManager connectionManager,
        INetworkEventsListener listener,
        INetworkManager networkManager,
        INetworkConfiguration networkConfiguration,
        ILogger<ClientConnectionManager> logger)
    {
        _connectionManager = connectionManager;
        _listener = listener;
        _networkManager = networkManager;
        _networkConfiguration = networkConfiguration;
        _logger = logger;
        
        _networkConfiguration.AutoRecycle = true;
        _networkConfiguration.EnableStatistics = false;
        _networkConfiguration.UnconnectedMessagesEnabled = false;
        _networkConfiguration.UseNativeSockets = true;
        
        _listener.OnPeerDisconnected += OnPeerDisconnected;
    }
    
    public void ConnectToServer()
    {
        if (CurrentPeer is not null)
            if (CurrentPeer.IsConnected)
            {
                _logger.LogError("Server peer is already connected.");
                return;
            }

        CurrentPeer = _networkManager
            .ConnectToServer(
                _networkConfiguration.Address,
                _networkConfiguration.Port, 
                _networkConfiguration.Key);
    }
    
    private void OnPeerDisconnected(ICustomNetPeer peer, ICustomDisconnectInfo disconnectInfo)
    {
        CurrentPeer = null;
    }
    
    public void DisconnectFromServer()
    {
        _connectionManager.DisconnectAll();
        CurrentPeer = null;
    }
}