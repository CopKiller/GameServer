using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Event;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Packet;
using Core.Network.Packet;
using Core.Network.Packets.Server;
using Core.Service;
using Core.Service.Interfaces.Types;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Extensions;

namespace Client;

public static class Program
{
    public static IServiceManager? ServiceManager { get; private set; }

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        ServiceManager = new ServiceManager(services);

        services.AddCryptography();
        services.AddLogger(LogLevel.Debug);
        services.AddDatabase();
        services.AddNetwork();
        services.AddNetworkClient();
        services.AddMapper();

        ServiceManager.Register();
        ServiceManager.Start();

        var logger = ServiceManager.ServiceProvider?.GetRequiredService<ILogger<ServiceManager>>();

        var clientPacketProcessor = ServiceManager.ServiceProvider?.GetRequiredService<IClientPacketProcessor>();

        await Task.Delay(1000);

        var packet = new SPacketFirst();
        clientPacketProcessor?.SendPacket(packet);

        var packet2 = new SPacketSecond();
        clientPacketProcessor?.SendPacket(packet2);

        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Cancela a finalização automática

            logger?.LogInformation("Finalizando Client...");

            ServiceManager?.Dispose();

            Environment.Exit(0);
        };

        logger?.LogInformation("Client iniciado. Pressione Ctrl+C para encerrar.");

        await Task.Delay(-1);
    }
}