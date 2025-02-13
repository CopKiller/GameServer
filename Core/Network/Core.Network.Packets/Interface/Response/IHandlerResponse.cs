using Core.Network.Interface;

namespace Core.Network.Packets.Interface.Response;

public interface IHandlerResponse <in T> where T : class
{
    void HandleResponse(T packet, IAdapterNetPeer peer);
    void HandleSuccess(T packet, IAdapterNetPeer peer);
    void HandleFailure(T packet, IAdapterNetPeer peer);
}