using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Database;
using Core.Database.Interfaces;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Database.Repositorys;
using Core.Network;
using Core.Network.Interface;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;
using Core.Utils.AutoMapper;
using Core.Utils.AutoMapper.Interface;

namespace Server.Dependency.Injection;

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
        const LogLevel logLevel = LogLevel.Trace;
        
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
        services.AddScoped<ICustomNetPeer, CustomNetPeer>();
        services.AddSingleton<ICustomPacketProcessor, CustomPacketProcessor>();
        services.AddSingleton<INetworkManager, NetworkManager>();
        services.AddSingleton<INetworkService, NetworkService>();
        services.AddSingleton<ICustomEventBasedNetListener, CustomEventBasedNetListener>();
    }
    
    private void ConfigureMapperService(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ServerServices)); // Scaneia os Profiles no assembly
        services.AddScoped<IMapperService, MapperService>();
    }
}