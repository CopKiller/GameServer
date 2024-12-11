using Core.Client.Network;
using Core.Client.Network.Interface;
using Core.Physics;
using Core.Physics.Builder;
using Core.Physics.Interface;
using Core.Physics.Interface.Builder;
using Core.Physics.Shared;
using Core.Service;


namespace Core.Extensions;

using Cryptography;
using Cryptography.Interface;
using Database;
using Database.Interfaces;
using Database.Models.Account;
using Database.Models.Player;
using Database.Repositorys;
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
    public static void AddCryptography(this IServiceCollection services)
    {
        services.AddScoped<ICrypto, Cryptography>();
    }

    public static void AddLogger(this IServiceCollection services, LogLevel logLevel = LogLevel.Trace)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });
    }
    
    public static void AddServiceManager(this IServiceCollection services)
    {
        services.AddSingleton<IServiceManager, ServiceManager>();
    }

    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<IDbContext, DatabaseContext>();
        services.AddScoped<IRepository<AccountModel>, Repository<AccountModel>>();
        services.AddScoped<IRepository<PlayerModel>, Repository<PlayerModel>>();
        services.AddScoped<IAccountRepository<AccountModel>, AccountRepository<AccountModel>>();
        services.AddScoped<IPlayerRepository<PlayerModel>, PlayerRepository<PlayerModel>>();
        services.AddScoped<IDatabaseService, DatabaseService>();
    }

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

    public static void AddNetworkServer(this IServiceCollection services)
    {
        // Core.Server.Network abstractions
        services.AddSingleton<ISingleService, ServerNetworkService>();
        services.AddSingleton<IServerPacketProcessor, ServerPacketProcessor>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
    }

    public static void AddNetworkClient(this IServiceCollection services)
    {
        // Core.Server.Network abstractions
        services.AddSingleton<ISingleService, ClientNetworkService>();
        services.AddSingleton<IClientPacketProcessor, ClientPacketProcessor>();
        services.AddSingleton<IClientConnectionManager, ClientConnectionManager>();
    }

    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperService)); // Automatically scans profiles in the assembly
        services.AddScoped<IMapperService, MapperService>();
    }
    
    public static void AddMapper(this IServiceCollection services, params Type[] profiles)
    {
        AddMapper(services);
        services.AddAutoMapper(profiles);
    }
    
    public static void AddPhysics(this IServiceCollection services)
    {
        services.AddTransient<IBodyBuilder, BodyBuilder>();
        services.AddTransient<IWorldBuilder, WorldBuilder>();
        services.AddSingleton<IWorldManager, WorldManager>();
        services.AddSingleton<IWorldService, WorldService>();
        
        services.AddSingleton<ISingleService, PhysicService>();
    }
}