using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.GameState;

public class MainMenuState(
    SceneManager sceneManager, 
    LoadingManager loadingManager, 
    ILogger<GameState<MainMenuScript>> logger) 
    : GameState<MainMenuScript>(sceneManager, loadingManager, logger)
{
    private readonly SceneManager _sceneManager = sceneManager;

    public override void ExitState()
    {
        base.ExitState();
        
        var currentScene = _sceneManager.GetCurrentScene();
        
        if (currentScene is MainMenuScript mainMenu)
        {
            mainMenu.CloseAllWindows();
        }
        
    }
}