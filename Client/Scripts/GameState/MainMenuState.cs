using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Network.SerializationObjects;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Godot;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.GameState;

public class MainMenuState(
    SceneManager sceneManager, 
    LoadingManager loadingManager, 
    ILogger<GameState<MainMenuScript>> logger) : GameState<MainMenuScript>(sceneManager, loadingManager, logger)
{
    private readonly SceneManager _sceneManager = sceneManager;

    public override async Task ExitStateAsync()
    {
        Scene?.CloseAllWindows();
        
        await base.ExitStateAsync();
    }
    
    public void ChangeStateToCharacterSelection()
    {
        Scene?.CloseAllWindows();        
        Scene?.CharacterWindow?.Show();
        Scene?.CharacterWindow?.PopulatePlayersList();
    }
    
    public void AddCharacterToCharacterSelection(List<PlayerDto> playersDto)
    {
        if (Scene?.CharacterWindow != null) 
            Scene.CharacterWindow.Players = playersDto;
    }
    public void AddCharacterToCharacterSelection(PlayerDto playerDto)
    {
        if (Scene?.CharacterWindow == null) return;
        
        var players = Scene.CharacterWindow.Players;
        players.Add(playerDto);
            
        Scene.CharacterWindow.Players = players;
    }
}