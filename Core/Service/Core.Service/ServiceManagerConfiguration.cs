using Core.Service.Interfaces;

namespace Core.Service;

public class ServiceManagerConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ServiceManager);
    public bool Enabled { get; set; } = false;
    public bool NeedUpdate { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 1;
}