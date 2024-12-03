using Core.Network.Interface;
using Core.Server.Network.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Services;

namespace Server;

public static class Program
{
    private static IServiceProvider? _serviceProvider;
    
    public static async Task Main(string[] args)
    {
        var server = new ServerManager();
        
        server.Register();

        server.Start();
        
        _serviceProvider = server.Manager.ServiceProvider;
   
        var logger = _serviceProvider?.GetRequiredService<ILogger<ServerManager>>();

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            logger?.LogInformation("Finalizando Servidor...");
            server?.Stop();

            server?.Dispose();
            
            eventArgs.Cancel = true; // Cancela a finalização automática
            
            Environment.Exit(0);
        };
        
        logger?.LogInformation("Servidor iniciado. Pressione Ctrl+C para encerrar.");

        await Task.Delay(-1);
    }
}