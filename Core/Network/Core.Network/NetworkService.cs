using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetworkService : INetworkService
{
    private NetworkMode _mode;
    private readonly NetManager _netManager;
    
    public bool IsRunning => _netManager.IsRunning;
    
    public ICustomNetPeer? GetFirstPeer()
    {
        return new CustomNetPeer(_netManager.FirstPeer);
    }
    
    public NetworkService(ICustomEventBasedNetListener netListener, 
        ILogger<LiteNetLibLoggerAdapter> logger)
    {
        if (netListener is not CustomEventBasedNetListener listener)
        {
            throw new ArgumentException("netListener must be an instance of EventBasedNetListener");
        }
        
        _netManager = new NetManager(listener.GetListener());
        
        var loggerAdapter = new LiteNetLibLoggerAdapter(logger);
        
        NetDebug.Logger = loggerAdapter;
    }
    
    public bool Initialize(NetworkMode mode, int port = 0, string? address = null)
    {
        _mode = mode;
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
    
    public void Send(byte[] data, CustomDeliveryMethod deliveryMethod)
    {
        if (_mode == NetworkMode.Server)
        {
            _netManager.SendToAll(data, Extensions.ConvertToLiteDeliveryMethod(deliveryMethod));
        }
    }
}