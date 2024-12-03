
using Core.Service.Interfaces.Types;
using Core.Service;

namespace Server.Services;

public sealed class ServerManager
{
    public IServiceManager Manager { get; } 
    
    public ServerManager()
    {
        var serverServices = new ServerServices();
        
        Manager = new ServiceManager(serverServices.GetServices());
    }
    
    public void Register()
    {
        Manager.Register();
    }
    
    public void Start()
    {
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