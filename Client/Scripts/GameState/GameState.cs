using System;
using System.Reflection;
using System.Threading.Tasks;
using Game.Scripts.Extensions;
using Game.Scripts.GameState.Interface;
using Game.Scripts.Loader;
using Game.Scripts.Singletons;
using Godot;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.GameState;

public class GameState<T>(
    SceneManager sceneManager,
    LoadingManager loadingManager,
    ILogger<GameState<T>> logger) : IGameState<T> where T : CanvasItem
{
    protected T? Scene { get; private set; }

    private bool FadeIn { get; set; } = true;
    private float FadeInDuration { get; set; } = 0.5f;
    private bool FadeOut { get; set; } = true;
    private float FadeOutDuration { get; set; } = 0.5f;

    private const LoaderPriority LoaderPriority = Loader.LoaderPriority.High;

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

    public virtual async Task EnterStateAsync()
    {
        logger.LogInformation($"{nameof(GameState<T>)}: {typeof(T).Name} entered!");

        if (sceneManager.IsConnected(SceneManager.SignalName.SceneChanged, Callable.From<Node>(OnSceneChanged)))
            sceneManager.Disconnect(SceneManager.SignalName.SceneChanged, Callable.From<Node>(OnSceneChanged));
        
        sceneManager.Connect(SceneManager.SignalName.SceneChanged, Callable.From<Node>(OnSceneChanged));
        
        loadingManager.AddTask(() => sceneManager.LoadSceneInBackground<T>(LoaderPriority), $"Loading {typeof(T).Name} scene...");
        
        await loadingManager.StartLoading();
    }
    
    private void OnSceneChanged(Node scene)
    {
        Scene = scene as T;

        if (Scene == null)
        {
            logger.LogError($"Failed to enter {typeof(T).Name}: Scene is null!");
            return;
        }

        if (FadeIn)
            Scene?.FadeIn(FadeInDuration);
    }

    public virtual async Task ExitStateAsync()
    {
        try
        {
            logger.LogInformation($"{typeof(T).Name} exited!");

            if (Scene == null)
            {
                logger.LogError($"Failed to exit {typeof(T).Name}: Scene is null!");
                return;
            }

            if (FadeOut)
                await Scene.FadeOut(FadeOutDuration);
        }
        catch (Exception e)
        {
            logger.LogError($"Failed to exit {typeof(T).Name}: {e.Message}");
        }
    }
}