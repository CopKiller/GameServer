using System.Threading.Tasks;
using Game.Scripts.Cache;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts;

public partial class FirstSceneScript : Node
{
    public override void _Ready()
    {
        var mainMenuState = ServiceManager.GetRequiredService<IGameState<MainMenuScript>>();
        
        var gameStateManager = ServiceManager.GetRequiredService<GameStateManager>();
        
        gameStateManager.ChangeState(mainMenuState);
    }
}