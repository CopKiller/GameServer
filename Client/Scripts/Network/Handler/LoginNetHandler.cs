using Core.Network.Interface;
using Core.Network.Packets.Handler;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class LoginNetHandler(
    ILogger<LoginHandler> logger) : LoginHandler
{
    public override void HandleRequest(LoginRequest request, IAdapterNetPeer peer)
    {
        // Not implemented here
    }

    public override void HandleSuccess(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Login response received in client {response.Response?.Message} accountID: {response.Account?.Id} username: {response.Account?.Username}");
    }

    public override void HandleFailure(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Login response received in client {response.Response?.Message}");
    }
}