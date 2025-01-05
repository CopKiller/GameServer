using Core.Network.Interface;

namespace Core.Network;

public class NetworkSettings(IAdapterNetManager netManager) : INetworkSettings
{
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public string Key { get; set; } = string.Empty;

    // Configurations from LiteNetLib NetManager
    /// <summary>
    /// Enable messages receiving without connection. (with SendUnconnectedMessage method)
    /// </summary>
    public bool UnconnectedMessagesEnabled
    {
        get => netManager.UnconnectedMessagesEnabled;
        set => netManager.UnconnectedMessagesEnabled = value;
    }

    /// <summary>
    /// Enable nat punch messages
    /// </summary>
    public bool NatPunchEnabled
    {
        get => netManager.NatPunchEnabled;
        set => netManager.NatPunchEnabled = value;
    }

    /// <summary>
    /// Library logic update and send period in milliseconds
    /// Lowest values in Windows doesn't change much because of Thread.Sleep precision
    /// To more frequent sends (or sends tied to your game logic) use <see cref="TriggerUpdate"/>
    /// </summary>
    public int UpdateTime
    {
        get => netManager.UpdateTime;
        set => netManager.UpdateTime = value;
    }

    /// <summary>
    /// Interval for latency detection and checking connection (in milliseconds)
    /// </summary>
    public int PingInterval
    {
        get => netManager.PingInterval;
        set => netManager.PingInterval = value;
    }

    /// <summary>
    /// If NetManager doesn't receive any packet from remote peer during this time (in milliseconds) then connection will be closed
    /// (including library internal keepalive packets)
    /// </summary>
    public int DisconnectTimeout
    {
        get => netManager.DisconnectTimeout;
        set => netManager.DisconnectTimeout = value;
    }

    /// <summary>
    /// Simulate packet loss by dropping random amount of packets. (Works only in DEBUG mode)
    /// </summary>
    public bool SimulatePacketLoss
    {
        get => netManager.SimulatePacketLoss;
        set => netManager.SimulatePacketLoss = value;
    }

    /// <summary>
    /// Simulate latency by holding packets for random time. (Works only in DEBUG mode)
    /// </summary>
    public bool SimulateLatency
    {
        get => netManager.SimulateLatency;
        set => netManager.SimulateLatency = value;
    }

    /// <summary>
    /// Chance of packet loss when simulation enabled. value in percents (1 - 100).
    /// </summary>
    public int SimulationPacketLossChance
    {
        get => netManager.SimulationPacketLossChance;
        set => netManager.SimulationPacketLossChance = value;
    }

    /// <summary>
    /// Minimum simulated latency (in milliseconds)
    /// </summary>
    public int SimulationMinLatency
    {
        get => netManager.SimulationMinLatency;
        set => netManager.SimulationMinLatency = value;
    }

    /// <summary>
    /// Maximum simulated latency (in milliseconds)
    /// </summary>
    public int SimulationMaxLatency
    {
        get => netManager.SimulationMaxLatency;
        set => netManager.SimulationMaxLatency = value;
    }

    /// <summary>
    /// Events automatically will be called without PollEvents method from another thread
    /// </summary>
    public bool UnsyncedEvents
    {
        get => netManager.UnsyncedEvents;
        set => netManager.UnsyncedEvents = value;
    }

    /// <summary>
    /// If true - receive event will be called from "receive" thread immediately otherwise on PollEvents call
    /// </summary>
    public bool UnsyncedReceiveEvent
    {
        get => netManager.UnsyncedReceiveEvent;
        set => netManager.UnsyncedReceiveEvent = value;
    }

    /// <summary>
    /// If true - delivery event will be called from "receive" thread immediately otherwise on PollEvents call
    /// </summary>
    public bool UnsyncedDeliveryEvent
    {
        get => netManager.UnsyncedDeliveryEvent;
        set => netManager.UnsyncedDeliveryEvent = value;
    }

    /// <summary>
    /// Allows receive broadcast packets
    /// </summary>
    public bool BroadcastReceiveEnabled
    {
        get => netManager.BroadcastReceiveEnabled;
        set => netManager.BroadcastReceiveEnabled = value;
    }

    /// <summary>
    /// Delay between initial connection attempts (in milliseconds)
    /// </summary>
    public int ReconnectDelay
    {
        get => netManager.ReconnectDelay;
        set => netManager.ReconnectDelay = value;
    }

    /// <summary>
    /// Maximum connection attempts before client stops and call disconnect event.
    /// </summary>
    public int MaxConnectAttempts
    {
        get => netManager.MaxConnectAttempts;
        set => netManager.MaxConnectAttempts = value;
    }

    /// <summary>
    /// Enables socket option "ReuseAddress" for specific purposes
    /// </summary>
    public bool ReuseAddress
    {
        get => netManager.ReuseAddress;
        set => netManager.ReuseAddress = value;
    }

    /// <summary>
    /// UDP Only Socket Option
    /// Normally IP sockets send packets of data through routers and gateways until they reach the final destination.
    /// If the DontRoute flag is set to True, then data will be delivered on the local subnet only.
    /// </summary>
    public bool DontRoute
    {
        get => netManager.DontRoute;
        set => netManager.DontRoute = value;
    }

    // TODO: Implement NetStatistics
    //public readonly NetStatistics Statistics = new NetStatistics();

    /// <summary>
    /// Toggles the collection of network statistics for the instance and all known peers
    /// </summary>
    public bool EnableStatistics
    {
        get => netManager.EnableStatistics;
        set => netManager.EnableStatistics = value;
    }

    // TODO: Implement NatPunchModule
    //public readonly NatPunchModule NatPunchModule;

    /// <summary>
    /// Returns true if socket listening and update thread is running
    /// </summary>
    public bool IsRunning => netManager.IsRunning;

    /// <summary>
    /// Local EndPoint (host and port)
    /// </summary>
    public int LocalPort => netManager.LocalPort;

    /// <summary>
    /// Automatically recycle NetPacketReader after OnReceive event
    /// </summary>
    public bool AutoRecycle
    {
        get => netManager.AutoRecycle;
        set => netManager.AutoRecycle = value;
    }

    /// <summary>
    /// IPv6 support
    /// </summary>
    public bool IPv6Enabled
    {
        get => netManager.IPv6Enabled;
        set => netManager.IPv6Enabled = value;
    }

    /// <summary>
    /// Override MTU for all new peers registered in this NetManager, will ignores MTU Discovery!
    /// </summary>
    public int MtuOverride
    {
        get => netManager.MtuOverride;
        set => netManager.MtuOverride = value;
    }

    /// <summary>
    /// Automatically discovery mtu starting from. Use at own risk because some routers can break MTU detection
    /// and connection in result
    /// </summary>
    public bool MtuDiscovery
    {
        get => netManager.MtuDiscovery;
        set => netManager.MtuDiscovery = value;
    }

    /// <summary>
    /// Experimental feature mostly for servers. Only for Windows/Linux
    /// use direct socket calls for send/receive to drastically increase speed and reduce GC pressure
    /// </summary>
    public bool UseNativeSockets
    {
        get => netManager.UseNativeSockets;
        set => netManager.UseNativeSockets = value;
    }

    /// <summary>
    /// Disconnect peers if HostUnreachable or NetworkUnreachable spawned (old behaviour 0.9.x was true)
    /// </summary>
    public bool DisconnectOnUnreachable
    {
        get => netManager.DisconnectOnUnreachable;
        set => netManager.DisconnectOnUnreachable = value;
    }

    /// <summary>
    /// Allows peer change it's ip (lte to wifi, wifi to lte, etc). Use only on server
    /// </summary>
    public bool AllowPeerAddressChange
    {
        get => netManager.AllowPeerAddressChange;
        set => netManager.AllowPeerAddressChange = value;
    }
}