using System.Reflection;
using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Database;
using Core.Database.Interface;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Database.Repositories;
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
using Core.Network.Packets.Handler.Interface;
using Core.Physics;
using Core.Physics.Builder;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Shared;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Core.Server.Network.Packet;
using Core.Server.Network.Packet.Handler;
using Core.Service;
using Core.Service.Interfaces.Types;
using Core.Utils.AutoMapper;
using Core.Utils.AutoMapper.Interface;
using Infrastructure.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Server.Extensions;

public static class ServerServiceExtensions
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
    /// Add database services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    /// <param name="connectionString">
    /// The connection string to the database
    /// </param>
    /// <param name="useInMemory">
    /// If true, the database will be in memory, for testing purposes
    /// </param>
    public static void AddDatabase(this IServiceCollection services, 
        string? connectionString = null,
        bool useInMemory = false)
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetAssembly(typeof(DatabaseContext))!)
            .AddEnvironmentVariables()
            .Build();

        var cnn = connectionString ?? configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(cnn) && !useInMemory)
            throw new InvalidOperationException("A valid connection string is required unless using InMemory database.");
        
        services.AddDbContext<IDbContext, DatabaseContext>(DbContextOptions);
        
        services.AddScoped<IRepository<AccountModel>, Repository<AccountModel>>();
        services.AddScoped<IRepository<PlayerModel>, Repository<PlayerModel>>();

        return;
        
        void DbContextOptions(DbContextOptionsBuilder options)
        {
            if (!useInMemory)
            {
                options.UseSqlServer(cnn);
                //options.EnableSensitiveDataLogging();
                //options.EnableDetailedErrors();
            }
            else
                options.UseInMemoryDatabase("InMemoryDatabase");
        }
    }
    
    /// <summary>
    /// Add server database services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddServerDatabase(this IServiceCollection services)
    {
        // Core.Server.Database abstractions
        services.AddScoped<IAccountRepository<AccountModel>, AccountRepository<AccountModel>>();
        services.AddScoped<IPlayerRepository<PlayerModel>, PlayerRepository<PlayerModel>>();
        services.AddScoped<IDatabaseService, DatabaseService>();
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
    /// Add network server services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddNetworkServer(this IServiceCollection services)
    {
        // Core.Server.Network abstractions
        services.AddSingleton<ISingleService, ServerNetworkService>();
        services.AddSingleton<IServerPacketSender, ServerPacketRequest>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
    }

    /// <summary>
    /// Add mapper services to the service collection
    /// Register the <see cref="IMapperService"/> and the <see cref="AutoMapper.MapperConfigurationExpression"/>
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile))); // Automatically scans profiles in the assembly
        services.AddScoped<IMapperService, MapperService>();
    }
    
    /// <summary>
    /// Add mapper services to the service collection
    /// Register the <see cref="IMapperService"/> and the <see cref="AutoMapper.MapperConfigurationExpression"/>
    /// This method also registers the profiles passed as parameter
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    /// <param name="profiles">
    /// The profiles to be registered in assembly
    /// </param>
    public static void AddMapper(this IServiceCollection services, params Type[] profiles)
    {
        AddMapper(services);
        services.AddAutoMapper(profiles);
    }
    
    /// <summary>
    /// Add physics services to the service collection
    /// This method registers all physics services and abstractions for the physics engine
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddPhysics(this IServiceCollection services)
    {
        services.AddTransient<IBodyBuilder, BodyBuilder>();
        services.AddTransient<IWorldBuilder, WorldBuilder>();
        services.AddSingleton<IWorldManager, WorldManager>();
        services.AddSingleton<IWorldService, WorldService>();
        
        services.AddSingleton<ISingleService, PhysicService>();
    }
    
    public static void AddNetworkRegisterHandlers(this IServiceCollection services)
    {
        services.AddScoped<ServerRegisterHandler>();
    }
}