using System.Collections.Immutable;
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ServiceManager(IServiceProvider serviceProvider) : IServiceManager
{
    public IServiceConfiguration ServiceConfiguration { get; } = new ServiceManagerConfiguration();
    public IServiceProvider ServiceProvider => serviceProvider;
    private List<ISingleService> Services { get; set; } = [];
    private TimerPool? ServicesUpdate { get; set; }
    private CancellationTokenSource? _updateCancellationTokenSource;

    private bool _disposed = false;

    private ILogger<ServiceManager>? Log { get; set; }

    public void Register()
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);

        Services.Clear();

        // Obter serviço ILogger apartir do ServiceProvider.
        Log = ServiceProvider.GetRequiredService<ILogger<ServiceManager>>();

        ServicesUpdate = new TimerPool(ServiceConfiguration, ServiceProvider.GetRequiredService<ILogger<TimerPool>>());

        Log?.LogDebug("Getting singleton services (ISingleService)...");
        foreach (var singleService in ServiceProvider.GetServices<ISingleService>())
            try
            {
                singleService.Register();
                Services.Add(singleService);
                ServicesUpdate?.AddService(singleService);
                Log?.LogDebug($"Registered singleton service: {singleService.ServiceConfiguration.ServiceType.Name}");
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Error registering service: {singleService.ServiceConfiguration.ServiceType.Name}");
            }
    }

    public void Update(long currentTick = 0)
    {
        if (!ServiceConfiguration.UpdateWithManager)
        {
            Log?.LogDebug("ServiceManager.Update disabled.");    
            return;
        }
        
        if (!ServiceConfiguration.Enabled)
        {
            Log?.LogDebug("ServiceManager is disabled.");
            return;
        }
        
        Log?.LogDebug("Forcing service updates...");

        foreach (var service in Services.OfType<ISingleService>())
            try
            {
                ServicesUpdate?.Update(service, true);
                Log?.LogDebug($"Force updated service: {service.GetType().Name}");
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Error forcing service update: {service.GetType().Name}");
            }
    }

    public void Start()
    {
        if (!ServiceConfiguration.StartWithManager)
        {
            Log?.LogDebug("ServiceManager.Start disabled.");
            return;
        }
        
        Log?.LogDebug("Starting services...");
        var servicesToStart = Services
            .Where(a => a.ServiceConfiguration is { Enabled: false, StartWithManager: true })
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

    public void Stop()
    {
        if (!ServiceConfiguration.StartWithManager)
        {
            Log?.LogDebug("ServiceManager.Stop disabled.");
            return;
        }
        
        Log?.LogDebug("Stopping services...");
        _updateCancellationTokenSource?.Cancel();
        _updateCancellationTokenSource?.Dispose();
        _updateCancellationTokenSource = null;
    }

    public void Restart()
    {
        Log?.LogDebug("Restarting services...");
        Stop();
        Start();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        
        Stop();

        Log?.LogDebug("Discarding services...");

        ServicesUpdate?.Dispose();

        if (ServiceProvider is IDisposable disposableProvider)
            try
            {
                disposableProvider.Dispose();
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, "Error disposing ServiceProvider");
            }

        foreach (var service in Services.OfType<IDisposable>())
            try
            {
                service.Dispose();
                Log?.LogDebug($"Disposed service: {service.GetType().Name}");
            }
            catch (Exception ex)
            {
                Log?.LogError(ex, $"Error when dismissing the service: {service.GetType().Name}");
            }

        Services.Clear();
    }
}