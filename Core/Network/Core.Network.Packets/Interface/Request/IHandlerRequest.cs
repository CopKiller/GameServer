using Core.Network.Interface;

namespace Core.Network.Packets.Interface.Request;

public interface IHandlerRequest <in T> where T : class
{
    void HandleRequest(T packet, IAdapterNetPeer peer);
}