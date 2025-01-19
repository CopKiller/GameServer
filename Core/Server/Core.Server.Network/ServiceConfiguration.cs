using Core.Service.Interfaces;

namespace Core.Server.Network;

public class ServiceConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ServerNetworkService);
    public bool Enabled { get; set; } = false;
    public bool StartWithManager { get; set; } = true;
    public bool StopWithManager { get; set; } = true;
    public bool UpdateWithManager { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 1;
}