using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;

namespace Core.Network;

public class NetworkConfiguration : INetworkConfiguration
{
    private readonly NetManager _netManager;

    public NetworkConfiguration(INetworkManager networkManager)
    {
        if (networkManager is not NetworkManager netManager)
            throw new ArgumentException("NetworkManager is not NetworkManager");

        _netManager = netManager.GetManager();
    }

    public string Address { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 9050;

    public string Key { get; set; } = "key";

    // Configurations from LiteNetLib NetManager

    /// <summary>
    /// Enable messages receiving without connection. (with SendUnconnectedMessage method)
    /// </summary>
    public bool UnconnectedMessagesEnabled
    {
        get => _netManager.UnconnectedMessagesEnabled;
        set => _netManager.UnconnectedMessagesEnabled = value;
    }

    /// <summary>
    /// Enable nat punch messages
    /// </summary>
    public bool NatPunchEnabled
    {
        get => _netManager.NatPunchEnabled;
        set => _netManager.NatPunchEnabled = value;
    }

    /// <summary>
    /// Library logic update and send period in milliseconds
    /// Lowest values in Windows doesn't change much because of Thread.Sleep precision
    /// To more frequent sends (or sends tied to your game logic) use <see cref="TriggerUpdate"/>
    /// </summary>
    public int UpdateTime
    {
        get => _netManager.UpdateTime;
        set => _netManager.UpdateTime = value;
    }

    /// <summary>
    /// Interval for latency detection and checking connection (in milliseconds)
    /// </summary>
    public int PingInterval
    {
        get => _netManager.PingInterval;
        set => _netManager.PingInterval = value;
    }

    /// <summary>
    /// If NetManager doesn't receive any packet from remote peer during this time (in milliseconds) then connection will be closed
    /// (including library internal keepalive packets)
    /// </summary>
    public int DisconnectTimeout
    {
        get => _netManager.DisconnectTimeout;
        set => _netManager.DisconnectTimeout = value;
    }

    /// <summary>
    /// Simulate packet loss by dropping random amount of packets. (Works only in DEBUG mode)
    /// </summary>
    public bool SimulatePacketLoss
    {
        get => _netManager.SimulatePacketLoss;
        set => _netManager.SimulatePacketLoss = value;
    }

    /// <summary>
    /// Simulate latency by holding packets for random time. (Works only in DEBUG mode)
    /// </summary>
    public bool SimulateLatency
    {
        get => _netManager.SimulateLatency;
        set => _netManager.SimulateLatency = value;
    }

    /// <summary>
    /// Chance of packet loss when simulation enabled. value in percents (1 - 100).
    /// </summary>
    public int SimulationPacketLossChance
    {
        get => _netManager.SimulationPacketLossChance;
        set => _netManager.SimulationPacketLossChance = value;
    }

    /// <summary>
    /// Minimum simulated latency (in milliseconds)
    /// </summary>
    public int SimulationMinLatency
    {
        get => _netManager.SimulationMinLatency;
        set => _netManager.SimulationMinLatency = value;
    }

    /// <summary>
    /// Maximum simulated latency (in milliseconds)
    /// </summary>
    public int SimulationMaxLatency
    {
        get => _netManager.SimulationMaxLatency;
        set => _netManager.SimulationMaxLatency = value;
    }

    /// <summary>
    /// Events automatically will be called without PollEvents method from another thread
    /// </summary>
    public bool UnsyncedEvents
    {
        get => _netManager.UnsyncedEvents;
        set => _netManager.UnsyncedEvents = value;
    }

    /// <summary>
    /// If true - receive event will be called from "receive" thread immediately otherwise on PollEvents call
    /// </summary>
    public bool UnsyncedReceiveEvent
    {
        get => _netManager.UnsyncedReceiveEvent;
        set => _netManager.UnsyncedReceiveEvent = value;
    }

    /// <summary>
    /// If true - delivery event will be called from "receive" thread immediately otherwise on PollEvents call
    /// </summary>
    public bool UnsyncedDeliveryEvent
    {
        get => _netManager.UnsyncedDeliveryEvent;
        set => _netManager.UnsyncedDeliveryEvent = value;
    }

    /// <summary>
    /// Allows receive broadcast packets
    /// </summary>
    public bool BroadcastReceiveEnabled
    {
        get => _netManager.BroadcastReceiveEnabled;
        set => _netManager.BroadcastReceiveEnabled = value;
    }

    /// <summary>
    /// Delay between initial connection attempts (in milliseconds)
    /// </summary>
    public int ReconnectDelay
    {
        get => _netManager.ReconnectDelay;
        set => _netManager.ReconnectDelay = value;
    }

    /// <summary>
    /// Maximum connection attempts before client stops and call disconnect event.
    /// </summary>
    public int MaxConnectAttempts
    {
        get => _netManager.MaxConnectAttempts;
        set => _netManager.MaxConnectAttempts = value;
    }

    /// <summary>
    /// Enables socket option "ReuseAddress" for specific purposes
    /// </summary>
    public bool ReuseAddress
    {
        get => _netManager.ReuseAddress;
        set => _netManager.ReuseAddress = value;
    }

    /// <summary>
    /// UDP Only Socket Option
    /// Normally IP sockets send packets of data through routers and gateways until they reach the final destination.
    /// If the DontRoute flag is set to True, then data will be delivered on the local subnet only.
    /// </summary>
    public bool DontRoute
    {
        get => _netManager.DontRoute;
        set => _netManager.DontRoute = value;
    }

    // TODO: Implement NetStatistics
    //public readonly NetStatistics Statistics = new NetStatistics();

    /// <summary>
    /// Toggles the collection of network statistics for the instance and all known peers
    /// </summary>
    public bool EnableStatistics
    {
        get => _netManager.EnableStatistics;
        set => _netManager.EnableStatistics = value;
    }

    // TODO: Implement NatPunchModule
    //public readonly NatPunchModule NatPunchModule;

    /// <summary>
    /// Returns true if socket listening and update thread is running
    /// </summary>
    public bool IsRunning => _netManager.IsRunning;

    /// <summary>
    /// Local EndPoint (host and port)
    /// </summary>
    public int LocalPort => _netManager.LocalPort;

    /// <summary>
    /// Automatically recycle NetPacketReader after OnReceive event
    /// </summary>
    public bool AutoRecycle
    {
        get => _netManager.AutoRecycle;
        set => _netManager.AutoRecycle = value;
    }

    /// <summary>
    /// IPv6 support
    /// </summary>
    public bool IPv6Enabled
    {
        get => _netManager.IPv6Enabled;
        set => _netManager.IPv6Enabled = value;
    }

    /// <summary>
    /// Override MTU for all new peers registered in this NetManager, will ignores MTU Discovery!
    /// </summary>
    public int MtuOverride
    {
        get => _netManager.MtuOverride;
        set => _netManager.MtuOverride = value;
    }

    /// <summary>
    /// Automatically discovery mtu starting from. Use at own risk because some routers can break MTU detection
    /// and connection in result
    /// </summary>
    public bool MtuDiscovery
    {
        get => _netManager.MtuDiscovery;
        set => _netManager.MtuDiscovery = value;
    }

    /// <summary>
    /// Experimental feature mostly for servers. Only for Windows/Linux
    /// use direct socket calls for send/receive to drastically increase speed and reduce GC pressure
    /// </summary>
    public bool UseNativeSockets
    {
        get => _netManager.UseNativeSockets;
        set => _netManager.UseNativeSockets = value;
    }

    /// <summary>
    /// Disconnect peers if HostUnreachable or NetworkUnreachable spawned (old behaviour 0.9.x was true)
    /// </summary>
    public bool DisconnectOnUnreachable
    {
        get => _netManager.DisconnectOnUnreachable;
        set => _netManager.DisconnectOnUnreachable = value;
    }

    /// <summary>
    /// Allows peer change it's ip (lte to wifi, wifi to lte, etc). Use only on server
    /// </summary>
    public bool AllowPeerAddressChange
    {
        get => _netManager.AllowPeerAddressChange;
        set => _netManager.AllowPeerAddressChange = value;
    }
}