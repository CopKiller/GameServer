using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class CreateCharNetHandler(
    ILogger<CreateCharNetHandler> logger) : IHandlerResponse<CreateCharResponse>
{
    public void HandleResponse(CreateCharResponse packet, IAdapterNetPeer peer)
    {
        if (packet.Response?.Success == true)
            HandleSuccess(packet, peer);
        else
            HandleFailure(packet, peer);
    }
    
    public void HandleSuccess(CreateCharResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Register response received in client {response.Response?.Message} accountID: {response.Player?.Id} username: {response.Player?.Name}");
        
    }

    public void HandleFailure(CreateCharResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Register response received in client {response.Response?.Message}");
    }
}