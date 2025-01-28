using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Game.Scripts.GameState;
using Game.Scripts.Singletons;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class CreateCharNetHandler(
    AlertManager alertManager,
    GameStateManager gameStateManager,
    NetworkManager networkManager,
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
        logger.LogInformation($"CreateChar response received in client {response.Response?.Message} accountID: {response.Player?.Id} username: {response.Player?.Name}");

        if (response.Response != null)
            alertManager.AddGlobalAlert(response.Response.Message);

        var currentState = gameStateManager.GetCurrentState();
        
        if (currentState is not MainMenuState mainMenuState)
        {
            networkManager.Reconnect("Client is not in MainMenu state, waiting for it to change...");
            return;
        }
        
        mainMenuState.ChangeStateToCharacterSelection();

        if (response.Player != null) 
            mainMenuState.AddCharacterToCharacterSelection(response.Player);
    }

    public void HandleFailure(CreateCharResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Register response received in client {response.Response?.Message}");
    }
}