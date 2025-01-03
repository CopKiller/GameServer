using System;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Extensions;
using Game.Scripts.GameState.Interface;
using Game.Scripts.Singletons;
using Godot;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.GameState;

public class GameState<T>(
    SceneManager sceneManager,
    LoadingManager loadingManager,
    ILogger<GameState<T>> logger) : IGameState<T> where T : CanvasItem
{
    private T? _scene;

    private bool FadeIn { get; set; } = true;
    private float FadeInDuration { get; set; } = 0.5f;
    private bool FadeOut { get; set; } = true;
    private float FadeOutDuration { get; set; } = 0.5f;

    public void SetFadeIn(bool fadeIn, float duration = 0.5f)
    {
        FadeIn = fadeIn;
        FadeInDuration = duration;
    }

    public void SetFadeOut(bool fadeOut, float duration = 0.5f)
    {
        FadeOut = fadeOut;
        FadeOutDuration = duration;
    }


    public virtual async Task EnterState()
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

            await loadingManager.StartLoading(OnSceneLoaded);
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to enter {typeof(T).Name}: {e.Message}");
        }
    }

    private async Task OnSceneLoaded()
    {
        await sceneManager.ChangeSceneLoadedInBackground<T>();
        _scene = sceneManager.GetCurrentScene() as T;

        if (_scene == null)
        {
            logger.LogError($"Failed to enter {typeof(T).Name}: Scene is null!");
            return;
        }

        if (FadeIn)
            await _scene.FadeIn(FadeInDuration);
    }

    public virtual async Task ExitState()
    {
        try
        {
            logger.LogInformation($"{typeof(T).Name} exited!");

            if (_scene == null)
            {
                logger.LogError($"Failed to exit {typeof(T).Name}: Scene is null!");
                return;
            }
            
            if (FadeOut)
                await _scene.FadeOut(FadeOutDuration);
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to exit {typeof(T).Name}: {e.Message}");
        }
    }
}