
using Core.Service.Interfaces.Types;
using Core.Service;

namespace Server.Dependency.Injection;

public sealed class ServerManager
{
    public IServiceManager Manager { get; } 
    
    public ServerManager()
    {
        var serverServices = new ServerServices();
        
        var serviceConfig = new ServiceManagerConfiguration() { Enabled = true, NeedUpdate = true, UpdateIntervalMs = 1 };
        
        Manager = new ServiceManager(serviceConfig, serverServices.GetServices());
    }
    
    public void Start()
    {
        Manager.Register();
        Manager.Start();
    }
    
    public void Stop()
    {
        Manager.Stop();
    }
    
    public void Dispose()
    {
        Manager.Dispose();
    }
}