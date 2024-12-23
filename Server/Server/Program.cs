
using Core.Database.Models.Account;
using Core.Server.Database.Interface;
using Core.Server.Extensions;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server;

public static class Program
{
    private static IServiceManager? _serviceManager;

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddCryptography();
        services.AddLogger(LogLevel.Trace);
        services.AddDatabase();
        services.AddServerDatabase();
        services.AddNetwork();
        services.AddNetworkServer();
        services.AddMapper();
        services.AddServiceManager();
        services.AddPhysics();

        _serviceManager?.Start();
        

        var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });

        _serviceManager = serviceProvider.GetRequiredService<IServiceManager>();
        _serviceManager.Register();
        _serviceManager.Start();

        var logger = serviceProvider?.GetRequiredService<ILogger<IServiceManager>>();
        
        var accountRepository = serviceProvider?.GetService<IAccountRepository<AccountModel>>();
        
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true; // Cancela a finalização automática

            logger?.LogInformation("Finalizing Server...");

            _serviceManager?.Dispose();

            Environment.Exit(0);
        };

        logger?.LogInformation("Server started. Press Ctrl+C to quit.");

        await Task.Delay(-1);
    }
}