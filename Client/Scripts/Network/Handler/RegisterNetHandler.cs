using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Game.Scripts.Singletons;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class RegisterNetHandler(
    AlertManager alertManager,
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
        logger.LogInformation($"Register response received in client {response.Response.Message} accountID: {response.Account.Id} username: {response.Account.Username}");
        alertManager.AddGlobalAlert(response.Response.Message);
    }   

    public void HandleFailure(RegisterResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Register response received in client {response.Response.Message}");
        alertManager.AddGlobalAlert(response.Response.Message);
    }
}