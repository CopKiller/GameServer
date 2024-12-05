
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
    
    private bool _disposed = false;
    
    private ILogger<ServiceManager>? Log { get; set; }

    public void Register()
    {
        ArgumentNullException.ThrowIfNull(collection);

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
            try
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
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Erro ao registrar serviço: {serviceDescriptor.ServiceType.Name}");
            }
        }
    }
    
    public void Start()
    {
        Log?.LogDebug("Starting services...");
        var servicesToStart = Services
            .Where(a => !a.ServiceConfiguration.Enabled)
            .ToList();

        foreach (var singleService in servicesToStart)
        {
            singleService.Start();
            Log?.LogDebug($"Started service: {singleService.ServiceConfiguration.ServiceType.Name}");
        }
        
        if (_updateCancellationTokenSource == null)
        {
            _updateCancellationTokenSource = new CancellationTokenSource();
            ServicesUpdate?.Start(_updateCancellationTokenSource.Token);
        }
    }
    
    public void ForceUpdate()
    {
        Log?.LogDebug("Forçando atualização de serviços...");
        
        foreach (var service in Services.OfType<ISingleService>())
        {
            try
            {
                ServicesUpdate?.Update(service, true);
                Log?.LogDebug($"Force updated service: {service.GetType().Name}");
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Erro ao forçar atualização do serviço: {service.GetType().Name}");
            }
        }
    }
    
    public void Stop()
    {
        Log?.LogDebug("Parando serviços...");
        _updateCancellationTokenSource?.Cancel();
        _updateCancellationTokenSource?.Dispose();
        _updateCancellationTokenSource = null;
    }
    
    public void Restart()
    {
        Log?.LogDebug("Reiniciando serviços...");
        Stop();
        Start();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        Log?.LogDebug("Descartando serviços...");

        ServicesUpdate?.Dispose();

        if (ServiceProvider is IDisposable disposableProvider)
        {
            try
            {
                disposableProvider.Dispose();
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, "Erro ao descartar o ServiceProvider.");
            }
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
                Log?.LogError(ex, $"Erro ao descartar serviço: {service.GetType().Name}");
            }
        }

        Services.Clear();
    }

}