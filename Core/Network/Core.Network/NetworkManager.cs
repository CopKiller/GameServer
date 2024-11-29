using Core.Network.Interface;
using LiteNetLib;

namespace Core.Network;

public class NetworkManager(INetworkService networkService) : INetworkManager
{
    // TODO: Implement NetworkManager using the SOLID principles
    
    
    
    public void Register()
    {
        networkService.Register();
    }

    public void Start()
    {
        //networkService
    }
    
    public void Stop()
    {
        networkService.Stop();
    }
    
    public void Update()
    {
        networkService.Update();
    }
    
    public void Dispose()
    {
        networkService.Dispose();
    }
}