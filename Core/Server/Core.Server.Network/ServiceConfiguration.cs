using Core.Service.Interfaces;

namespace Core.Server.Network;

public class ServiceConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ServerNetworkService);
    public bool Enabled { get; set; } = false;
    public bool NeedUpdate { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 1;
}