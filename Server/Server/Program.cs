using Core.Network.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Dependency.Injection;

namespace Server;

public static class Program
{
    private static IServiceProvider? _serviceProvider;
    
    public static async Task Main(string[] args)
    {
        var server = new ServerManager();

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
        
        logger?.LogInformation("Iniciando o network...");
        
        var eventListener = _serviceProvider?.GetRequiredService<ICustomEventBasedNetListener>();

        eventListener.OnConnectionRequest += request =>
        {
            request.AcceptIfKey("key");
        };
        
        eventListener.OnPeerConnected += peer =>
        {
            logger?.LogInformation($"Peer conectado: {peer}");
        };
        
        var networkManager = _serviceProvider?.GetRequiredService<INetworkService>();
        
        networkManager?.Register();

        networkManager?.StartServer(8224);
        
        networkManager?.StartClient();
        
        networkManager?.ConnectToServer("localhost", 8224);
        
        logger?.LogInformation("Servidor iniciado. Pressione Ctrl+C para encerrar.");
        
        while (true)
        {
            networkManager?.Update();
            await Task.Delay(15);
        }

        //await Task.Delay(-1);
    }
}