using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientConnectionManager : IClientConnectionManager
{
    private readonly IConnectionManager _connectionManager;
    private readonly INetworkEventsListener _listener;
    private readonly ILogger<ClientConnectionManager> _logger;
    public int Latency { get; private set; }

    private ICustomNetPeer? CurrentPeer { get; set; }
    
    public ClientConnectionManager(
        IConnectionManager connectionManager,
        INetworkEventsListener listener,
        ILogger<ClientConnectionManager> logger)
    {
        _connectionManager = connectionManager;
        _listener = listener;
        _logger = logger;
        
        _listener.OnNetworkLatencyUpdate += OnNetworkLatencyUpdate;
        _listener.OnPeerDisconnected += OnPeerDisconnected;
    }
    
    private void OnNetworkLatencyUpdate(ICustomNetPeer peer, int latency)
    {
        Latency = latency;
        
        _logger.LogInformation($"Latency updated: {latency}");
    }
    
    private void OnPeerDisconnected(ICustomNetPeer peer, ICustomDisconnectInfo disconnectInfo)
    {
        CurrentPeer = null;
    }
    
    public void Disconnect()
    {
        _connectionManager.DisconnectAll();
        CurrentPeer = null;
    }

    public ICustomNetPeer? GetServerPeer()
    {
        return CurrentPeer;
    }
    
    public void SetServerPeer(ICustomNetPeer peer)
    {
        CurrentPeer = peer;
    }
}