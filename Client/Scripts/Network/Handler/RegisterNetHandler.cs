using Core.Network.Interface;
using Core.Network.Packets.Handler;
using Core.Network.Packets.Request;
using Core.Network.Packets.Response;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class RegisterNetHandler(
    ILogger<RegisterHandler> logger) : RegisterHandler
{
    public override void HandleRequest(RegisterRequest request, IAdapterNetPeer peer)
    {
        // Not implemented here
    }

    public override void HandleSuccess(RegisterResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Register response received in client {response.Response?.Message} accountID: {response.Account?.Id} username: {response.Account?.Username}");
    }

    public override void HandleFailure(RegisterResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Register response received in client {response.Response?.Message}");
    }
}