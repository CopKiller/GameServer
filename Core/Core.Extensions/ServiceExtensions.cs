using System.Reflection;
using System.Runtime.CompilerServices;
using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Physics;
using Core.Physics.Builder;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Shared;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;


namespace Core.Extensions;

using Cryptography;
using Cryptography.Interface;
using Database;
using Database.Interface;
using Database.Models.Account;
using Database.Models.Player;
using Database.Repositories;
using Network;
using Network.Connection;
using Network.Event;
using Network.Interface;
using Network.Interface.Connection;
using Network.Interface.Packet;
using Network.Packet;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Service.Interfaces.Types;
using Core.Utils.AutoMapper;
using Core.Utils.AutoMapper.Interface;

public static class ServiceExtensions
{
    /// <summary>
    /// Add cryptography services to the service collection
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add the services to
    /// </param>
    public static void AddCryptography(this IServiceCollection services)
    {
        services.AddScoped<ICrypto, Cryptography>();
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
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
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
                options.UseSqlServer(cnn);
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
        services.AddScoped<ICustomDataWriter, CustomDataWriter>();
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddSingleton<IPacketProcessor, PacketProcessor>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<INetworkConfiguration, NetworkConfiguration>();
        services.AddSingleton<INetworkEventsListener, NetworkEventsListener>();
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
        services.AddSingleton<IServerPacketProcessor, ServerPacketProcessor>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
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
        services.AddSingleton<ISingleService, ClientNetworkService>();
        services.AddSingleton<IClientPacketProcessor, ClientPacketProcessor>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
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
}