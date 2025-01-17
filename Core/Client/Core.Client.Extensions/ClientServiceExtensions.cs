
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Client.Network.Packet;
using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Logger.Interface;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Event;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Event;
using Core.Network.Interface.Packet;
using Core.Network.Interface.Serialization;
using Core.Network.Packet;
using Core.Network.Packets;
using Core.Network.Packets.Interface.Handler;
using Core.Service;
using Core.Service.Interfaces.Types;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Client.Extensions;

public static class ClientServiceExtensions
{
    /// <summary>
    /// Add cryptography services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddCryptography(this IServiceCollection services)
    {
        services.AddScoped<ICrypto, CryptographyProvider>();
    }

    /// <summary>
    /// Add logger services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    /// <param name="logLevel">
    /// The minimum log level to be logged
    /// </param>
    public static void AddLogger(this IServiceCollection services, LogLevel logLevel = LogLevel.Trace)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);

            // Resolva o ILogOutput do contêiner de serviços
            loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                var logOutput = serviceProvider.GetRequiredService<ILogOutput>();
                return new CustomLoggerProvider(logOutput, logLevel);
            });
        });
    }

    
    /// <summary>
    /// Add service manager to the service collection
    /// This service is responsible for managing all singletons services of type <see cref="ISingleService"/>
    /// </summary>
    /// <param name="services"></param>
    public static void AddServiceManager(this IServiceCollection services)
    {
        services.AddSingleton<IServiceManager, ServiceManager>();
    }

    /// <summary>
    /// Add network services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddNetwork(this IServiceCollection services)
    {
        // Core.Network abstractions
        
        // LiteNetLib
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddSingleton<INetworkEventsListener>(p =>
            p.GetRequiredService<INetworkManager>().NetworkEventsListener);
        services.AddSingleton<IAdapterNetManager>(p =>
            p.GetRequiredService<INetworkManager>().AdapterNetManager);
        
        services.AddSingleton<INetService, NetService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<INetworkSettings, NetworkSettings>();
        
        // Packet
        services.AddSingleton<IHandlerRegistry, RegisterHandler>();
        services.AddSingleton<IPacketProcessor, PacketProcessor>();
        services.AddSingleton<IPacketRegister>(p => 
            p.GetRequiredService<IPacketProcessor>().PacketRegister);
        services.AddSingleton<IPacketHandler>(p => 
            p.GetRequiredService<IPacketProcessor>().PacketHandler);
        services.AddSingleton<IPacketSender>(p => 
            p.GetRequiredService<IPacketProcessor>().PacketSender);
        services.AddSingleton<INetworkSerializer>(p =>
            p.GetRequiredService<IPacketProcessor>().NetworkSerializer);
        
        // Serializer
        services.AddTransient<IAdapterDataWriter, AdapterDataWriter>();
    }

    /// <summary>
    /// Add network client services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddNetworkClient(this IServiceCollection services)
    {
        // Core.Server.Network abstractions
        services.AddSingleton<ISingleService ,ClientNetworkService>();
        services.AddSingleton<IClientPacketRequest, ClientPacketRequest>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
    }
}