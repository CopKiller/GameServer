using System;
using System.Threading.Tasks;
using Core.Client.Extensions;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions.Services;
using Game.Scripts.Transitions;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Singletons;

public partial class ServiceManager : Node
{
    private IServiceManager? _serviceManager;
    private static IServiceProvider? _serviceProvider;
    
    public override void _Ready()
    {
        var loadingScript = GetTree().CurrentScene as LoadingScript;
        loadingScript?.AddTask(ConfigureServices, "Registrando serviços...");
        loadingScript?.AddTask(InitializeServices, "Inicializando serviços...");
    }
    
    public static T? GetService<T>()
    {
        if (_serviceProvider == null)
        {
            throw new NullReferenceException("ServiceProvider não foi inicializado.");
        }
        
        return _serviceProvider.GetService<T>();
    }
    
    public static T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new NullReferenceException("ServiceProvider não foi inicializado.");
        }
        
        return _serviceProvider.GetRequiredService<T>();
    }
    
    private async Task ConfigureServices()
    {
        await Task.Run(() =>
        {
            var services = new ServiceCollection();
            services.AddLogger();
            services.AddGodotLoggerOutput();
            services.AddNetwork();
            services.AddNetworkClient();
            services.AddServiceManager();
            
            _serviceProvider = services.BuildServiceProvider();
        });
    }
    
    private async Task InitializeServices()
    {
        await Task.Run(() =>
        {
            _serviceManager = _serviceProvider?.GetRequiredService<IServiceManager>();
            
            _serviceManager?.Register();
            //_serviceManager?.Start(); -- Não é necessário iniciar os serviços agora, mas nos provedores de cada serviço.
        });
    }
    
    private void StopServices()
    {
        _serviceManager?.Stop();
    }
    
    private void DisposeServices()
    {
        _serviceManager?.Dispose();
    }
}