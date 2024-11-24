using Core.Service.Interfaces;

namespace Server.Dependency.Injection;

public class ServiceManagerConfiguration : IServiceConfiguration
{
    public bool Enabled { get; set; } = true;
    public bool NeedUpdate { get; set; } = false;
    public int UpdateIntervalMs { get; set; } = 1000;
}