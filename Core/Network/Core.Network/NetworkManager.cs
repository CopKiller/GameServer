using System.Net;
using Core.Network.Event;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetworkManager : INetworkManager
{
    private readonly NetManager _networkManager;

    public NetworkManager(INetworkEventsListener networkEventsListener, ILogger<NetworkManager> logger)
    {
        if (networkEventsListener is not NetworkEventsListener listener)
            throw new ArgumentException("NetworkEventsListener is not NetworkEventsListener");

        _networkManager = new NetManager(listener.GetListener());

        var loggerAdapter = new LiteNetLibLoggerAdapter<NetworkManager>(logger);
        NetDebug.Logger = loggerAdapter;
    }

    internal NetManager GetManager()
    {
        return _networkManager;
    }

    public bool StartListener(int port = 0)
    {
        return _networkManager.Start(port);
    }

    public void StartClient()
    {
        _networkManager.Start();
    }

    public ICustomNetPeer ConnectToServer(string address, int port, string key)
    {
        return new CustomNetPeer(_networkManager.Connect(address, port, key));
    }

    public ICustomNetPeer ConnectToServer(IPEndPoint target, string key)
    {
        return new CustomNetPeer(_networkManager.Connect(target, key));
    }

    public void Stop()
    {
        _networkManager.Stop();
    }

    public void PollEvents()
    {
        _networkManager.PollEvents();
    }

    public void TriggerEvents()
    {
        _networkManager.TriggerUpdate();
    }

    public void Stop(bool sendDisconnectMessages)
    {
        _networkManager.Stop(sendDisconnectMessages);
    }

    public int GetPeersCount(CustomConnectionState peerState)
    {
        return _networkManager.GetPeersCount(Extensions.ConvertToConnectionState(peerState));
    }

    public IEnumerable<ICustomNetPeer> GetPeers(CustomConnectionState state)
    {
        foreach (var peer in _networkManager.ConnectedPeerList)
            if (peer.ConnectionState == Extensions.ConvertToConnectionState(state))
                yield return new CustomNetPeer(peer);
    }

    public ICustomNetPeer GetFirstPeer()
    {
        return new CustomNetPeer(_networkManager.FirstPeer);
    }

    public void DisconnectAll()
    {
        _networkManager.DisconnectAll();
    }

    public void DisconnectAll(byte[] data, int start, int count)
    {
        _networkManager.DisconnectAll(data, start, count);
    }

    public void DisconnectPeerForce(ICustomNetPeer peer)
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _networkManager.DisconnectPeerForce(customNetPeer.Peer);
    }

    public void DisconnectPeer(ICustomNetPeer peer)
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _networkManager.DisconnectPeer(customNetPeer.Peer);
    }

    public void DisconnectPeer(ICustomNetPeer peer, byte[] data)
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _networkManager.DisconnectPeer(customNetPeer.Peer, data);
    }

    public void DisconnectPeer(ICustomNetPeer peer, ICustomDataWriter writer)
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        if (writer is not CustomDataWriter customDataWriter)
            throw new ArgumentException("Writer is not CustomDataWriter");

        _networkManager.DisconnectPeer(customNetPeer.Peer, customDataWriter.GetNetDataWriter());
    }

    public void DisconnectPeer(ICustomNetPeer peer, byte[] data, int start, int count)
    {
        if (peer is not CustomNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _networkManager.DisconnectPeer(customNetPeer.Peer, data, start, count);
    }

    public void CreateNtpRequest(IPEndPoint endPoint)
    {
        _networkManager.CreateNtpRequest(endPoint);
    }

    public void CreateNtpRequest(string ntpServerAddress, int port)
    {
        _networkManager.CreateNtpRequest(ntpServerAddress, port);
    }

    public void CreateNtpRequest(string ntpServerAddress)
    {
        _networkManager.CreateNtpRequest(ntpServerAddress);
    }
}