using System.Net;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface.Connection;

public interface IConnectionManager
{
    bool StartListener(int port = 0);
    IAdapterNetPeer ConnectToServer(string address, int port, string key);
    IAdapterNetPeer ConnectToServer(IPEndPoint target, string key);
    int GetPeersCount(CustomConnectionState peerState);
    public IAdapterNetPeer? GetPeerById(int id);
    bool HasConnectedPeers { get; }
    ReadOnlySpan<IAdapterNetPeer> GetPeers();
    void RegisterEvents();
    void DisconnectAll();
    void DisconnectAll(byte[] data, int start, int count);
    void DisconnectPeerForce(IAdapterNetPeer peer);
    void DisconnectPeer(IAdapterNetPeer peer);
    void DisconnectPeer(IAdapterNetPeer peer, byte[] data);
    void DisconnectPeer(IAdapterNetPeer peer, IAdapterDataWriter writer);
    void DisconnectPeer(IAdapterNetPeer peer, byte[] data, int start, int count);
    void DisconnectPeer(IAdapterNetPeer peer, string reason);
}