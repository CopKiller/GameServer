using System.Net;
using Core.Network.Event;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class NetworkManager : INetworkManager
{
    public IAdapterNetManager AdapterNetManager { get; }
    public INetworkEventsListener NetworkEventsListener { get; }

    public NetworkManager(ILoggerFactory loggerFactory)
    {
        var eventBasedNetListener = new EventBasedNetListener();
        
        NetworkEventsListener = new NetworkEventsListener(eventBasedNetListener);
        
        var liteNetManager = new NetManager(eventBasedNetListener);
        
        AdapterNetManager = new AdapterNetManager(liteNetManager, loggerFactory.CreateLogger<NetworkManager>());
    }
}