using System.Numerics;
using Core.Extensions;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Interface.Enum;
using Core.Service;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server;

public static class Program
{
    private static IServiceManager? _serviceManager;

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddCryptography();
        services.AddLogger(LogLevel.Trace);
        services.AddDatabase();
        services.AddServerDatabase();
        services.AddNetwork();
        services.AddNetworkServer();
        services.AddMapper();
        services.AddServiceManager();
        services.AddPhysics();

        var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });

        var serviceManager = serviceProvider.GetRequiredService<IServiceManager>();
        serviceManager.Register();
        serviceManager.Start();

        var logger = serviceProvider?.GetRequiredService<ILogger<IServiceManager>>();
        
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Cancela a finalização automática

            logger?.LogInformation("Finalizing Server...");

            _serviceManager?.Dispose();

            Environment.Exit(0);
        };

        logger?.LogInformation("Server started. Press Ctrl+C to quit.");

        await Task.Delay(-1);
    }
}