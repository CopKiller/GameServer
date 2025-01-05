using System.Net;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network.Connection;

/// <summary>
/// Gerenciador de conexões.
/// </summary>
public class ConnectionManager(
    IAdapterNetManager netManager, 
    INetworkEventsListener listener,
    INetworkSettings settings,
    ILogger<ConnectionManager> logger) : IConnectionManager
{
    
    private readonly Dictionary<int, IAdapterNetPeer> _peers = [];
    
    /// <summary>
    /// Verifica se há peers conectados.
    /// </summary>
    public bool HasConnectedPeers => _peers.Count != 0;
    
    public void RegisterEvents()
    {
        listener.OnPeerConnected += AddPeer;
        listener.OnPeerDisconnected += RemovePeer;
        listener.OnConnectionRequest += ConnectionRequest;
    }

    /// <summary>
    /// Processa uma solicitação de conexão.
    /// </summary>
    /// <param name="request"></param>
    private void ConnectionRequest(IAdapterConnectionRequest request)
    {
        logger.LogInformation($"Connection request from {request.RemoteEndPoint}");

        request.AcceptIfKey(settings.Key);
    }


    /// <summary>
    /// Processa a conexão de um peer.
    /// </summary>
    /// <param name="peer"></param>
    private void AddPeer(IAdapterNetPeer peer)
    {
        logger.LogDebug($"Peer connected - id: {peer.Id} address: {peer.EndPoint}");
        _peers.Add(peer.Id, peer);
    }


    /// <summary>
    /// Processa a desconexão de um peer.
    /// </summary>
    /// <param name="peer"></param>
    /// <param name="disconnectInfo"></param>
    private void RemovePeer(IAdapterNetPeer peer, IAdapterDisconnectInfo? disconnectInfo = null)
    {
        logger.LogDebug(disconnectInfo != null
            ? $"Peer disconnected - id: {peer.Id} address: {peer.EndPoint} ({disconnectInfo.Reason}}})"
            : $"Peer disconnected - id: {peer.Id} address: {peer.EndPoint}");

        _peers.Remove(peer.Id);
    }


    public void DisconnectPeer(IAdapterNetPeer peer, byte[] data, int start, int count)
    {
        netManager.DisconnectPeer(peer, data, start, count);
    }

    /// <summary>
    /// Lista de peers atualmente conectados.
    /// </summary>
    /// <summary>
    /// Lista de peers customizados.
    /// </summary>
    public IReadOnlyDictionary<int, IAdapterNetPeer> CustomPeers =>
        _peers;


    /// <summary>
    /// Desconecta um peer específico.
    /// </summary>
    public void DisconnectPeer(IAdapterNetPeer peer, string reason)
    {
        netManager.DisconnectPeer(peer);

        RemovePeer(peer);
    }


    /// <summary>
    /// Desconecta todos os peers conectados.
    /// </summary>
    public void DisconnectAll()
    {
        netManager.DisconnectAll();
    }

    public void DisconnectAll(byte[] data, int start, int count)
    {
        netManager.DisconnectAll(data, start, count);
    }

    public void DisconnectPeerForce(IAdapterNetPeer peer)
    {
        netManager.DisconnectPeerForce(peer);
    }

    public void DisconnectPeer(IAdapterNetPeer peer)
    {
        netManager.DisconnectPeer(peer);
    }

    public void DisconnectPeer(IAdapterNetPeer peer, byte[] data)
    {
        netManager.DisconnectPeer(peer, data);
    }

    public void DisconnectPeer(IAdapterNetPeer peer, IAdapterDataWriter writer)
    {
        netManager.DisconnectPeer(peer, writer);
    }


    public bool StartListener(int port = 0)
    {
        return netManager.StartListener(port);
    }

    public IAdapterNetPeer ConnectToServer(string address, int port, string key)
    {
        return netManager.ConnectToServer(address, port, key);
    }

    public IAdapterNetPeer ConnectToServer(IPEndPoint target, string key)
    {
        return netManager.ConnectToServer(target, key);
    }

    public int GetPeersCount(CustomConnectionState peerState)
    {
        return netManager.GetPeersCount(peerState);
    }

    /// <summary>
    /// Obtém um peer pelo seu ID.
    /// </summary>
    public IAdapterNetPeer? GetPeerById(int id)
    {
        return _peers.FirstOrDefault(i => i.Key == id).Value;
    }
}