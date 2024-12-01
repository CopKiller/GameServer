using Core.Service.Interfaces.Types;

namespace Core.Service.Interfaces;

public interface IServiceConfiguration
{
    Type ServiceType { get; }
    bool Enabled { get; set; }
    bool NeedUpdate { get; set; }
    int UpdateIntervalMs { get; set; }
}