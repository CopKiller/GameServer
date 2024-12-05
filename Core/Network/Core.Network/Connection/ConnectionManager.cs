using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network.Connection;

/// <summary>
/// Gerenciador de conexões.
/// </summary>
public class ConnectionManager : IConnectionManager
{
    private readonly INetworkEventsListener _listener;
    private readonly INetworkManager _networkManager;
    private readonly INetworkConfiguration _configuration;
    private readonly ILogger<ConnectionManager> _logger;

    private readonly Dictionary<int, ICustomNetPeer> _peers = [];

    public ConnectionManager(INetworkManager networkManager, INetworkEventsListener listener,
        INetworkConfiguration configuration, ILogger<ConnectionManager> logger)
    {
        _listener = listener ?? throw new ArgumentNullException(nameof(listener));
        _networkManager = networkManager ?? throw new ArgumentNullException(nameof(networkManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _listener.OnPeerConnected += AddPeer;
        _listener.OnPeerDisconnected += RemovePeer;
        _listener.OnConnectionRequest += ConnectionRequest;
    }

    /// <summary>
    /// Verifica se há peers conectados.
    /// </summary>
    public bool HasConnectedPeers => _peers.Count != 0;


    /// <summary>
    /// Processa uma solicitação de conexão.
    /// </summary>
    /// <param name="request"></param>
    private void ConnectionRequest(ICustomConnectionRequest request)
    {
        _logger.LogInformation($"Connection request from {request.RemoteEndPoint}");

        request.AcceptIfKey(_configuration.Key);
    }


    /// <summary>
    /// Processa a conexão de um peer.
    /// </summary>
    /// <param name="peer"></param>
    private void AddPeer(ICustomNetPeer peer)
    {
        _logger.LogDebug($"Peer connected - id: {peer.Id} address: {peer.EndPoint}");
        _peers.Add(peer.Id, peer);
    }


    /// <summary>
    /// Processa a desconexão de um peer.
    /// </summary>
    /// <param name="peer"></param>
    /// <param name="disconnectInfo"></param>
    private void RemovePeer(ICustomNetPeer peer, ICustomDisconnectInfo disconnectInfo = null)
    {
        _logger.LogDebug($"Peer disconnected - id: {peer.Id} address: {peer.EndPoint} ({disconnectInfo.Reason}}})");
        _peers.Remove(peer.Id);
    }


    /// <summary>
    /// Lista de peers atualmente conectados.
    /// </summary>
    /// <summary>
    /// Lista de peers customizados.
    /// </summary>
    public IReadOnlyDictionary<int, ICustomNetPeer> CustomPeers =>
        _peers;


    /// <summary>
    /// Desconecta um peer específico.
    /// </summary>
    public void DisconnectPeer(ICustomNetPeer peer, string reason = "Disconnected")
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer must implement CustomNetPeer");

        _networkManager.DisconnectPeer(peer);

        RemovePeer(peer);
    }


    /// <summary>
    /// Desconecta todos os peers conectados.
    /// </summary>
    public void DisconnectAll()
    {
        _networkManager.DisconnectAll();
    }


    /// <summary>
    /// Obtém um peer pelo seu ID.
    /// </summary>
    public ICustomNetPeer? GetPeerById(int id)
    {
        return _peers.FirstOrDefault(i => i.Key == id).Value;
    }


    /// <summary>
    /// Obter todos os peers conectados.
    /// </summary>
    public IEnumerable<ICustomNetPeer> GetPeers()
    {
        return _peers.Values;
    }


    /// <summary>
    /// Obter o primeiro peer conectado. (é o do servidor)
    /// </summary>
    /// <returns></returns>
    public ICustomNetPeer GetFirstPeer()
    {
        return _networkManager.GetFirstPeer();
    }
}