using Core.Network.Interface;
using Core.Server.Network.Interface;

namespace Core.Server.Network;

public class ConnectionManager : IConnectionManager
{
    private List<ICustomNetPeer> _peers = new();
    
    internal void AddPeer(ICustomNetPeer peer)
    {
        _peers.Add(peer);
    }
    
    internal void RemovePeer(ICustomNetPeer peer, ICustomDisconnectInfo disconnectInfo)
    {
        _peers.Remove(peer);
    }
    
    public IEnumerable<ICustomNetPeer> GetAllPeers()
    {
        return _peers;
    }
}