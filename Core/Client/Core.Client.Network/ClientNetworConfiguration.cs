using Core.Service.Interfaces;

namespace Core.Client.Network;

public class ClientNetworConfiguration : IServiceConfiguration
{
    public Type ServiceType { get; } = typeof(ClientNetworkService);
    public bool Enabled { get; set; } = false;
    public bool StartWithManager { get; set; } = true;
    public bool StopWithManager { get; set; } = true;
    public bool UpdateWithManager { get; set; } = true;
    public int UpdateIntervalMs { get; set; } = 15;
}