using System;
using Core.Client.Extensions;
using Core.Service.Interfaces.Types;
using Game.Scripts.Extensions;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Game.Scripts.Singletons;

public partial class ClientManager : Node
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly IServiceManager? _serviceManager;
    
    public ClientManager()
    {
        var services = new ServiceCollection();
        services.AddLogger();
        services.AddGodotLoggerOutput();
        services.AddServiceManager();
        
        _serviceProvider = services.BuildServiceProvider();
        
        _serviceManager = _serviceProvider.GetService<IServiceManager>();
        
        _serviceManager?.Register();
        _serviceManager?.Start();
        
    }
}