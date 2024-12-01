
using Core.Client.Network;
using Core.Network;
using Core.Network.Interface;
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
        
        await Task.Delay(-1);
    }
    
    private static void ConfigureLoggerService(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Trace;
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });
    }
    
    private static void ConfigureNetworkService(IServiceCollection services)
    {
        // Project Core.Network Abstract LiteNetLib
        //services.AddScoped<ICustomNetPeer, CustomNetPeer>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
        
        // ISingleService -> LoopService
        services.AddSingleton<ISingleService, ClientNetworkService>();
    }
}