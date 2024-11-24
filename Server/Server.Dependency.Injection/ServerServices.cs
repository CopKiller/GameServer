﻿using Core.Cryptography;
using Core.Cryptography.Interface;
using Core.Database;
using Core.Database.Interfaces;
using Core.Database.Interfaces.Account;
using Core.Database.Interfaces.Player;
using Core.Database.Models.Account;
using Core.Database.Models.Player;
using Core.Database.Repositorys;
using Core.Logger.Interface;
using Infrastructure.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core.Server.Database;
using Core.Server.Database.Interface;
using Core.Server.Database.Repositories;

namespace Server.Dependency.Injection;

internal class ServerServices
{
    public IServiceCollection GetServices()
    {
        IServiceCollection services = new ServiceCollection();

        ConfigureCryptographyService(services);
        ConfigureLoggerService(services);
        ConfigureDatabaseService(services);
        
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
}