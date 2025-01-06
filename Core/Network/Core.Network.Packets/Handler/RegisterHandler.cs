using Core.Network.Interface;
using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Network.Packets.Request.Interface;
using Core.Network.Packets.Response;
using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets.Handler;

public abstract class RegisterHandler : IHandlerRequest<RegisterRequest>, IHandlerResponse<RegisterResponse>
{
    public abstract void HandleRequest(RegisterRequest request, IAdapterNetPeer peer);

    public void HandleResponse(RegisterResponse response, IAdapterNetPeer peer)
    {
        if (response.Response?.Success == true)
            HandleSuccess(response, peer);
        else
            HandleFailure(response, peer);
    }

    public abstract void HandleSuccess(RegisterResponse response, IAdapterNetPeer peer);
    public abstract void HandleFailure(RegisterResponse response, IAdapterNetPeer peer);
}