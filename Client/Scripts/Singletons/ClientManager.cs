using System;
using Core.Client.Extensions;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Services;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Singletons;

public partial class ClientManager : Node
{
    private readonly IServiceManager? _serviceManager;
    
    private PackedScene? _loadingScene;
    private PackedScene? _splashScreen;
    
    public ClientManager()
    {
        var services = new ServiceCollection();
        services.AddLogger();
        services.AddGodotLoggerOutput();
        services.AddServiceManager();
        
        var provider = services.BuildServiceProvider();
        
        _serviceManager = provider.GetService<IServiceManager>();
        
        _serviceManager?.Register();
        _serviceManager?.Start();
    }
    
    public override void _Ready()
    {
        
    }
    
    private void ChangeScene(PackedScene scene)
    {
        var result = GetTree().ChangeSceneToPacked(scene);
        
        if (result != Error.Ok)
        {
            GD.PrintErr("Erro ao carregar cena!");
        }
    }
}