using Core.Network.Connection;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetworkService : INetworkService
{
    public NetworkMode Mode { get; private set; }
    
    internal readonly NetManager _netManager;
    
    public NetworkService(ICustomEventBasedNetListener listener, ILogger<LiteNetLibLoggerAdapter> logger)
    {
        if (listener is not CustomEventBasedNetListener customEventBasedNetListener)
        {
            throw new ArgumentException("Listener must implement IConnectionManager");
        }
        
        var loggerAdapter = new LiteNetLibLoggerAdapter(logger);
        NetDebug.Logger = loggerAdapter;
        
        _netManager = new NetManager(customEventBasedNetListener.GetListener());
    }
    
    public bool Initialize(NetworkMode mode, int port = 0, string? address = null)
    {
        Mode = mode;
        
        switch (mode)
        {
            case NetworkMode.Server:
                return StartServer(port);
            case NetworkMode.Client:
                ArgumentNullException.ThrowIfNull(address);
                StartClient();
                return ConnectToServer(address, port);
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
    
    private bool StartServer(int port)
    {
        return _netManager.Start(port);
    }

    private void StartClient()
    {
        _netManager.Start();
    }

    private bool ConnectToServer(string address, int port)
    {
        var netPeer = _netManager.Connect(address, port, "key");
        
        return netPeer != null;
    }
    
    public void Register()
    {
        _netManager.AutoRecycle = true;
        _netManager.EnableStatistics = false;
        _netManager.UnconnectedMessagesEnabled = false;
        
        if (Mode == NetworkMode.Server)
        {
            _netManager.UseNativeSockets = true;
        }
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