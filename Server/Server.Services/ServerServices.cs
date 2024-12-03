using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Database;
using Core.Database.Interfaces;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Database.Repositorys;
using Core.Network;
using Core.Network.Connection;
using Core.Network.Interface;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;
using Core.Server.Network;
using Core.Server.Network.Interface;
using Core.Service.Interfaces.Types;
using Core.Utils.AutoMapper;
using Core.Utils.AutoMapper.Interface;

namespace Server.Services;

internal class ServerServices
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();

        ConfigureCryptographyService(services);
        ConfigureLoggerService(services);
        ConfigureDatabaseService(services);
        ConfigureNetworkService(services);
        ConfigureMapperService(services);
        
        return services;
    }
    
    private void ConfigureCryptographyService(IServiceCollection services)
    {
        services.AddScoped<ICrypto, Cryptography>();
    }

    private void ConfigureLoggerService(IServiceCollection services)
    {
        const LogLevel logLevel = LogLevel.Debug;
        
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(logLevel);
            
            loggingBuilder.AddProvider(new CustomLoggerProvider(logLevel));
        });
    }
    
    private void ConfigureDatabaseService(IServiceCollection services)
    {
        services.AddDbContext<IDbContext, DatabaseContext>();
        //options.EnableSensitiveDataLogging();
        //options.EnableDetailedErrors();
        
        services.AddScoped<IRepository<AccountModel>, Repository<AccountModel>>();
        services.AddScoped<IRepository<PlayerModel>, Repository<PlayerModel>>();
        
        
        services.AddScoped<IAccountRepository<AccountModel>, AccountRepository<AccountModel>>();
        services.AddScoped<IPlayerRepository<PlayerModel>, PlayerRepository<PlayerModel>>();
        services.AddScoped<IDatabaseService, DatabaseService>();
    }
    
    private void ConfigureNetworkService(IServiceCollection services)
    {
        // Project Core.Network Abstract LiteNetLib
        services.AddScoped<ICustomDataWriter, CustomDataWriter>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
        
        // Project Core.Server.Network
        services.AddSingleton<ISingleService, ServerNetworkService>();
        services.AddSingleton<IServerNetworkProcessor, ServerNetworkProcessor>();
        services.AddSingleton<IServerConnectionManager, ServerConnectionManager>();
    }
    
    private void ConfigureMapperService(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperService)); // Scaneia os Profiles no assembly
        services.AddScoped<IMapperService, MapperService>();
    }
}