using Core.Network.Interface;
using Core.Network.Packets.Handler;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class LoginNetHandler(ILogger<LoginHandler> logger) : LoginHandler
{
    public override void HandleRequest(LoginRequest request, IAdapterNetPeer peer)
    {
        logger.LogInformation("Login request received in client");
    }

    public override void HandleSuccess(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Login response received in client {response.Message}");
    }

    public override void HandleFailure(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Login response received in client {response.Message}");
    }
}