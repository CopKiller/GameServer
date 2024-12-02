using Core.Service.Interfaces;

namespace Core.Client.Network;

public class ClientNetworkConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ClientNetworkService);
    public bool Enabled { get; set; } = false;
    public bool NeedUpdate { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 15;
}