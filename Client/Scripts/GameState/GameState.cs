using System;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Extensions.Attributes;
using Game.Scripts.GameState.Interface;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Godot;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.GameState;

public class GameState<T>(
    SceneManager sceneManager,
    LoadingManager loadingManager,
    ILogger<GameState<T>> logger) : IGameState<T> where T : CanvasItem
{
    public virtual async void EnterState()
    {
        try
        {
            logger.LogInformation($"{nameof(GameState<T>)}: {typeof(T).Name} entered!");

            loadingManager.AddTask(() =>
                {
                    var result = sceneManager.LoadSceneInBackground<T>();

                    if (!result)
                        logger.LogError($"Failed to load {typeof(T).Name}");

                    return Task.CompletedTask;
                }, $"Loading {typeof(T).Name}");

            await loadingManager.StartLoading(() => sceneManager.ChangeSceneLoadedInBackground<T>());
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to enter {typeof(T).Name}: {e.Message}");
        }
    }

    public virtual void ExitState()
    {
        logger.LogInformation($"{typeof(T).Name} exited!");
    }
}