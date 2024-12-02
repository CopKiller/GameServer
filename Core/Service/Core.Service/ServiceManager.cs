
using System.Collections.Immutable;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ServiceManager(IServiceCollection collection) : IServiceManager
{
    public IServiceConfiguration Configuration { get; } = new ServiceManagerConfiguration();
    public IServiceProvider? ServiceProvider { get; set; }
    private List<ISingleService> Services { get; set; } = [];
    private TimerPool? ServicesUpdate { get; set; }
    private CancellationTokenSource? _updateCancellationTokenSource;
    
    private ILogger<ServiceManager>? Log { get; set; }

    public void Register()
    {
        
        ServiceProvider = collection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });
        
        Services.Clear();
        
        // Obter serviço ILogger apartir do ServiceProvider.
        Log = ServiceProvider.GetRequiredService<ILogger<ServiceManager>>();
        
        ServicesUpdate = new TimerPool(Configuration, ServiceProvider.GetRequiredService<ILogger<TimerPool>>());
        
        Log?.LogDebug("Getting singleton services (ISingleService)...");
        foreach (var serviceDescriptor in collection.Where(sd => 
                     sd.Lifetime == ServiceLifetime.Singleton && 
                     sd.ServiceType.IsAssignableFrom(typeof(ISingleService))))
        {
            var singletonService = ServiceProvider.GetRequiredService(serviceDescriptor.ServiceType) as ISingleService;

            if (singletonService != null)
            {
                singletonService.Register();
                Services.Add(singletonService);
                ServicesUpdate?.AddService(singletonService);
                Log?.LogDebug($"Registered singleton service: {singletonService.GetType().Name}");
            }
        }
    }
    
    public void Start()
    {
        Log?.LogDebug("Starting services...");
        foreach (var singleService in Services.
                     Where(a => !a.Configuration.Enabled)
                     .ToImmutableList())
        {
            singleService.Start();
            Log?.LogDebug($"Started service: {singleService.Configuration.ServiceType.Name}");
        }
        
        _updateCancellationTokenSource ??= new CancellationTokenSource();
        ServicesUpdate?.Start(_updateCancellationTokenSource.Token);
    }
    
    public void ForceUpdate()
    {
        Log?.LogDebug("Forçando atualização de serviços...");
        
        foreach (var service in Services.OfType<ISingleService>())
        {
            ServicesUpdate?.Update(service, true);
        }
    }
    
    public void Stop()
    {
        Log?.LogDebug("Parando serviços...");
        _updateCancellationTokenSource?.Cancel();
    }
    
    public void Restart()
    {
        Log?.LogDebug("Reiniciando serviços...");
        Stop();
        Start();
    }
    
    public void Dispose()
    {
        Log?.LogDebug("Descartando serviços...");
        
        ServicesUpdate?.Dispose();
        
        // Dispose seguro para o ServiceProvider, caso seja IDisposable
        if (ServiceProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }
    
        _updateCancellationTokenSource?.Dispose();
    
        foreach (var service in Services.OfType<IDisposable>())
        {
            try
            {
                service.Dispose();
                Log?.LogDebug($"Disposed service: {service.GetType().Name}");
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Error while disposing service: {service.GetType().Name}");
            }
        }

    }

}