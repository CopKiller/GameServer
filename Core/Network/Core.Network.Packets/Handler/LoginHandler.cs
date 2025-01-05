using Core.Network.Packets.Handler.Interface;
using Core.Network.Packets.Request;
using Core.Network.Packets.Request.Interface;
using Core.Network.Packets.Response;
using Core.Network.Packets.Response.Interface;

namespace Core.Network.Packets.Handler;

public abstract class LoginHandler : IHandlerRequest<LoginRequest>, IHandlerResponse<LoginResponse>
{
    public abstract void HandleRequest(LoginRequest request);

    public void HandleResponse(LoginResponse response)
    {
        if (response.Success)
        {
            HandleSuccess(response);
        }
        else
        {
            HandleFailure(response);
        }
    }

    public abstract void HandleSuccess(LoginResponse response);
    public abstract void HandleFailure(LoginResponse response);
}