using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class RegisterNetHandler(
    ILogger<RegisterNetHandler> logger) : IHandlerResponse<RegisterResponse>
{
    public void HandleResponse(RegisterResponse packet, IAdapterNetPeer peer)
    {
        if (packet.Response?.Success == true)
            HandleSuccess(packet, peer);
        else
            HandleFailure(packet, peer);
    }
    
    public void HandleSuccess(RegisterResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Register response received in client {response.Response?.Message} accountID: {response.Account?.Id} username: {response.Account?.Username}");
    }

    public void HandleFailure(RegisterResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Register response received in client {response.Response?.Message}");
    }
}