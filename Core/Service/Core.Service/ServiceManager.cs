
using Core.Service.Interfaces;
using Core.Service.Interfaces.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Service;

public class ServiceManager(IServiceConfiguration configuration, IServiceCollection collection) : IServiceManager
{
    public IServiceConfiguration Configuration { get; } = configuration;
    public IServiceProvider? ServiceProvider { get; set; }
    private List<ISingleService> Services { get; set; } = [];
    private TimerPool? TickCounter { get; set; }
    private CancellationTokenSource? _updateCancellationTokenSource;
    
    private ILogger<ServiceManager>? Log { get; set; }
    private ILoggerFactory? LoggerFactory { get; set; }

    public void Register()
    {
        
        ServiceProvider = collection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateOnBuild = false,
            ValidateScopes = false
        });
        
        Services.Clear();
        
        // Obter serviço ILogger apartir do ServiceProvider.
        LoggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
        Log = LoggerFactory.CreateLogger<ServiceManager>();
        
        TickCounter = new TimerPool(Configuration, LoggerFactory.CreateLogger<TimerPool>());
        
        Log?.LogDebug("Getting singleton services (ISingleService)...");
        foreach (var serviceDescriptor in collection.Where(sd => 
                     sd.Lifetime == ServiceLifetime.Singleton && 
                     typeof(ISingleService).IsAssignableFrom(sd.ServiceType)))
        {
            var singletonService = ServiceProvider.GetRequiredService(serviceDescriptor.ServiceType) as ISingleService;

            if (singletonService != null)
            {
                singletonService.Register();
                Services.Add(singletonService);
                TickCounter?.AddService(singletonService);
                Log?.LogDebug($"Registered singleton service: {singletonService.GetType().Name}");
            }
        }
    }
    
    public void Start()
    {
        Log?.LogDebug("Starting services...");
        Services.ForEach(service =>
        {
            service.Start();
            Log?.LogDebug($"Service {service.GetType().Name} started.");
        });
        
        _updateCancellationTokenSource ??= new CancellationTokenSource();
        TickCounter?.Start(_updateCancellationTokenSource.Token);
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
        
        TickCounter?.Dispose();
        
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