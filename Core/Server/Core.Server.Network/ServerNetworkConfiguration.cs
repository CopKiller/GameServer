using Core.Service.Interfaces;

namespace Core.Server.Network;

public class ServerNetworkConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ServerNetworkService);
    public bool Enabled { get; set; } = true;
    public bool NeedUpdate { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 15;
}