using Core.Service.Interfaces;

namespace Core.Service;

public class ServiceManagerConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ServiceManager);
    public bool Enabled { get; set; } = false;
    public bool StartWithManager { get; set; } = true;
    public bool StopWithManager { get; set; } = true;
    public bool UpdateWithManager { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 1;
}