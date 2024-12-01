using Core.Network.Interface;

namespace Core.Server.Network.Interface;

public interface IConnectionManager
{
    IEnumerable<ICustomNetPeer> GetAllPeers();
}