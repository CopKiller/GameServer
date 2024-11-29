using Core.Network.Interface;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetworkService : INetworkService
{
    public NetworkService(ICustomEventBasedNetListener netListener, ILogger<LiteNetLibLoggerAdapter> logger)
    {
        
        if (netListener is not CustomEventBasedNetListener listener)
        {
            throw new ArgumentException("netListener must be an instance of EventBasedNetListener");
        }
        
        var loggerAdapter = new LiteNetLibLoggerAdapter(logger);
        
        NetDebug.Logger = loggerAdapter;

        _netManager = new NetManager(listener.GetListener());
    }

    private readonly NetManager _netManager;
    
    public void StartServer(int port)
    {
        _netManager.Start(port);
    }

    public void StartClient()
    {
        _netManager.Start();
    }

    public void ConnectToServer(string address, int port)
    {
       var netPeer = _netManager.Connect(address, port, "key");
       
         if (netPeer == null)
         {
              throw new Exception("Failed to connect to server");
         }
         
         
    }
    
    public void Register()
    {
        _netManager.AutoRecycle = true;
        _netManager.EnableStatistics = false;
        _netManager.UnconnectedMessagesEnabled = false;
    }

    public void Stop()
    {
        _netManager.Stop();
    }

    public void Update()
    {
        _netManager.PollEvents();
    }

    public void Dispose()
    {
        _netManager.Stop();
    }
}