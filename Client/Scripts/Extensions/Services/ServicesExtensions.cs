
using Core.Logger.Interface;
using Game.Scripts.Cache;
using Game.Scripts.GameState;
using Game.Scripts.GameState.Interface;
using Game.Scripts.Loader;
using Game.Scripts.Logger;
using Game.Scripts.MainScenes.MainMenu;
using Game.Scripts.Singletons;
using Game.Scripts.Transitions;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Extensions.Services;

public static class ServicesExtensions
{
    public static void AddGodotTree(this IServiceCollection services, Node node)
    {
        services.AddSingleton<SceneTree>(p => node.GetTree());
    }
    
    public static void AddGodotLoggerOutput(this IServiceCollection services)
    {
        services.AddSingleton<ILogOutput, GodotLogOutput>();
    }
    
    public static void AddGodotNetworkManager(this IServiceCollection services)
    {
        services.AddSingleton<NetworkManager>(p => 
            p.GetRequiredService<SceneTree>().Root.GetSingleton<NetworkManager>());
    }
    
    public static void AddGodotSceneManager(this IServiceCollection services)
    {
        services.AddSingleton<SceneManager>(p => 
            p.GetRequiredService<SceneTree>().Root.GetSingleton<SceneManager>());
    }
    
    public static void AddGodotServiceManager(this IServiceCollection services)
    {
        services.AddSingleton<ServiceManager>(p => 
            p.GetRequiredService<SceneTree>().Root.GetSingleton<ServiceManager>());
    }
    
    public static void AddGodotLoadingManager(this IServiceCollection services)
    {
        services.AddSingleton<LoadingManager>(p => 
            p.GetRequiredService<SceneTree>().Root.GetSingleton<LoadingManager>());
    }
    
    public static void AddGodotCustomLoader(this IServiceCollection services)
    {
        services.AddSingleton<CustomLoader>();
    }
    
    public static void AddGodotGameStateManager(this IServiceCollection services)
    {
        services.AddSingleton<GameStateManager>(p => 
            p.GetRequiredService<SceneTree>().Root.GetSingleton<GameStateManager>());
    }
    
    public static void AddGodotGameState(this IServiceCollection services)
    {
        // Add all game states here
        services.AddScoped<IGameState<LoadingScript>, GameState<LoadingScript>>();
        services.AddScoped<IGameState<MainMenuScript>, MainMenuState>();
    }
    
    public static void AddGodotScenePathCache(this IServiceCollection services)
    {
        services.AddSingleton<ScenePathCache>();
    }
    
    public static void AddGodotLoaderService(this IServiceCollection services)
    {
        services.AddSingleton<ILoaderService, LoaderService>();
    }
}