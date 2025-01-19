using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Game.Scripts.GameState;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Network.Handler;

public class LoginNetHandler(
    AlertManager alertManager,
    SceneManager sceneManager,
    NetworkManager networkManager,
    GameStateManager gameStateManager,
    ILogger<LoginNetHandler> logger) : IHandlerResponse<LoginResponse>
{
    public void HandleResponse(LoginResponse packet, IAdapterNetPeer peer)
    {
        if (packet.Response?.Success == true)
            HandleSuccess(packet, peer);
        else
            HandleFailure(packet, peer);
    }

    public void HandleSuccess(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogInformation($"Login response received in client {response.Response.Message} accountID: {response.Account.Id} username: {response.Account.Username}");
        
        alertManager.AddGlobalAlert(response.Response.Message);

        var currentState = gameStateManager.GetCurrentState();
        
        if (currentState is not MainMenuState mainMenuState)
        {
            networkManager.Reconnect("Client is not in MainMenu state, waiting for it to change...");
            return;
        }
        
        mainMenuState.ChangeStateToCharacterSelection();
        
        mainMenuState.AddCharacterToCharacterSelection(response.Account.Players);
    }

    public void HandleFailure(LoginResponse response, IAdapterNetPeer peer)
    {
        logger.LogError($"Login response received in client {response.Response.Message}");

        alertManager.AddGlobalAlert(response.Response.Message);
    }
}