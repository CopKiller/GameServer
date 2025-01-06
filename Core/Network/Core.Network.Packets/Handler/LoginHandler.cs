using Core.Network.Interface;
using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Network.Packets.Request.Interface;
using Core.Network.Packets.Response;
using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets.Handler;

public abstract class LoginHandler : IHandlerRequest<LoginRequest>, IHandlerResponse<LoginResponse>
{
    public abstract void HandleRequest(LoginRequest request, IAdapterNetPeer peer);

    public void HandleResponse(LoginResponse response, IAdapterNetPeer peer)
    {
        if (response.Response?.Success == true)
            HandleSuccess(response, peer);
        else
            HandleFailure(response, peer);
    }

    public abstract void HandleSuccess(LoginResponse response, IAdapterNetPeer peer);
    public abstract void HandleFailure(LoginResponse response, IAdapterNetPeer peer);
}