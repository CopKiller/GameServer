
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Interface;
using Core.Network.Packets.Server;
using Core.Service;
using Core.Service.Interfaces.Types;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        
        var service = new ServiceCollection();
        
        ConfigureLoggerService(service);
        ConfigureNetworkService(service);

        var servicesManager = new ServiceManager(service);
        
        servicesManager.Register();
        servicesManager.Start();
        
        var clientPacketProcessor = servicesManager.ServiceProvider?.GetRequiredService<IClientNetworkProcessor>();
        
        await Task.Delay(2000);
        
        var packet = new SPacketFirst();
        clientPacketProcessor?.SendPacket(packet);
        
        var packet2 = new SPacketSecond();
        clientPacketProcessor?.SendPacket(packet2);
        
        await Task.Delay(-1);
    }
    
    private static void ConfigureLoggerService(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Debug;
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });
    }
    
    private static void ConfigureNetworkService(IServiceCollection services)
    {
        // Project Core.Network Abstract LiteNetLib
        services.AddScoped<ICustomDataWriter, CustomDataWriter>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
        
        // ISingleService -> LoopService
        services.AddSingleton<ISingleService, ClientNetworkService>();
        services.AddSingleton<IClientNetworkProcessor, ClientNetworkProcessor>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
    }
}