using System;
using System.Threading.Tasks;
using Core.Client.Extensions;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Services;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Game.Scripts.Singletons;

public partial class ServiceManager : Node
{
    private IServiceManager? _serviceManager;
    private static IServiceProvider? _serviceProvider;

    public override void _Ready()
    {
        GD.Print("ServiceManager ready!");

        ConfigureServices();

        var loadingManager = this.GetSingleton<LoadingManager>();
        loadingManager?.AddTask(RegisterServices, "Registrando serviços...");
        loadingManager?.AddTask(StartServices, "Iniciando serviços...");
    }

    public static T? GetService<T>()
    {
        if (_serviceProvider == null)
            throw new NullReferenceException("ServiceProvider não foi inicializado.");

        var service = _serviceProvider.GetService<T>();

        return service;
    }

    public static T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider == null)
            throw new NullReferenceException("ServiceProvider não foi inicializado.");

        var service = _serviceProvider.GetRequiredService<T>();

        return service;
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddLogger(logLevel: LogLevel.Debug);
        services.AddNetwork();
        services.AddNetworkClient();
        services.AddServiceManager();

        services.AddGodotLoggerOutput();
        services.AddGodotTree(this);
        services.AddGodotGameStateManager();
        services.AddGodotGameState();
        services.AddGodotNetworkManager();
        services.AddGodotSceneManager();
        services.AddGodotServiceManager();
        services.AddGodotLoadingManager();
        services.AddGodotAlertManager();
        services.AddGodotScenePathCache();
        services.AddGodotLoaderService();
        services.AddGodotCustomLoader();
        services.AddGodotRegisterNetworkHandlers();

        _serviceProvider = services.BuildServiceProvider();
    }

    private Task RegisterServices()
    {
        _serviceManager = _serviceProvider?.GetRequiredService<IServiceManager>();

        _serviceManager?.Register();
        
        return Task.CompletedTask;
    }
    
    private Task StartServices()
    {
        _serviceManager?.Start();
        
        return Task.CompletedTask;
    }

    public override void _ExitTree()
    {
        _serviceManager?.Stop();
        _serviceManager?.Dispose();
    }
}