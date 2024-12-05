using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Server.Network.Interface;

namespace Core.Server.Network;

public class ServerConnectionManager(IConnectionManager connectionManager) : IServerConnectionManager
{
    public IReadOnlyDictionary<int, ICustomNetPeer> CustomPeers => connectionManager.CustomPeers;
    
    public void DisconnectPeer(ICustomNetPeer peer, string reason = "Disconnected") => connectionManager.DisconnectPeer(peer, reason);

    public void DisconnectAll() => connectionManager.DisconnectAll();

    public bool HasConnectedPeers => connectionManager.HasConnectedPeers;
    
    public ICustomNetPeer? GetPeerById(int id) => connectionManager.GetPeerById(id);

    public IEnumerable<ICustomNetPeer> GetPeers() => connectionManager.GetPeers();
}