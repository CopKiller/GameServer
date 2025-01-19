
namespace Core.Service.Interfaces;

/// <summary>
/// Implement this interface in a class that will be used to configure a service.
/// The service configuration will be used to enable/disable the service, set the update interval and other settings.
/// </summary>
public interface IServiceConfiguration
{
    Type ServiceType { get; }
    bool Enabled { get; set; }
    bool StartWithManager { get; set; }
    bool StopWithManager { get; set; }
    bool UpdateWithManager { get; set; }
    int UpdateIntervalMs { get; set; }
}