using Core.Network.Interface;

namespace Core.Network.Packets.Request.Interface;

public interface IHandlerRequest <in T> where T : class
{
    void HandleRequest(T packet, IAdapterNetPeer peer);
}