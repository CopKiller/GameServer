using System.Collections;
using System.Net;
using System.Runtime.InteropServices;
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
    
    private readonly List<IAdapterNetPeer> _peers = [];
    
    /// <summary>
    /// Verifica se há peers conectados.
    /// </summary>
    public bool HasConnectedPeers => netManager.HasConnectedPeers() > 0;
    
    public ReadOnlySpan<IAdapterNetPeer> GetPeers()
    {
        return CollectionsMarshal.AsSpan(_peers);
    }
    
    public void RegisterEvents()
    {
        listener.OnConnectionRequest += ConnectionRequest;
        listener.OnPeerConnected += AddPeer;
        listener.OnPeerDisconnected += RemovePeer;
    }
    
    private void AddPeer(IAdapterNetPeer peer)
    {
        _peers.Add(peer);
    }
    
    private void RemovePeer(IAdapterNetPeer peer, IAdapterDisconnectInfo info)
    {
        _peers.Remove(peer);
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
    
    public void DisconnectPeer(IAdapterNetPeer peer, byte[] data, int start, int count)
    {
        netManager.DisconnectPeer(peer, data, start, count);
    }

    /// <summary>
    /// Desconecta um peer específico.
    /// </summary>
    public void DisconnectPeer(IAdapterNetPeer peer)
    {
        netManager.DisconnectPeer(peer);
    }
    
    public void DisconnectPeer(int peerId)
    {
        var peer = GetPeerById(peerId);
        if (peer != null)
        {
            netManager.DisconnectPeer(peer);
        }
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
        return netManager.GetPeerById(id);
    }
}