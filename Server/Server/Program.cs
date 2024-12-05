
using Core.Extensions;
using Core.Service;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server;

public static class Program
{
    private static IServiceManager? _serviceManager;
    
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        _serviceManager = new ServiceManager(services);
        var serviceProvider = _serviceManager.ServiceProvider;
        
        services.AddCryptography();
        services.AddLogger(LogLevel.Trace);
        services.AddDatabase();
        services.AddNetwork();
        services.AddNetworkServer();
        services.AddMapper();
        
        _serviceManager.Register();
        _serviceManager.Start();
        
        var logger = serviceProvider?.GetRequiredService<ILogger<ServiceManager>>();

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Cancela a finalização automática
            
            logger?.LogInformation("Finalizando Servidor...");
 
            _serviceManager?.Dispose();
            
            Environment.Exit(0);
        };
        
        logger?.LogInformation("Servidor iniciado. Pressione Ctrl+C para encerrar.");

        await Task.Delay(-1);
    }
}