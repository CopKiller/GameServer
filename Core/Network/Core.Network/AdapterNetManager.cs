using System.Net;
using Core.Network.Event;
using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class AdapterNetManager : IAdapterNetManager
{
    private readonly NetManager _netManager;

    public AdapterNetManager(
        NetManager netManager,
        ILogger<NetworkManager> logger)
    {
        _netManager = netManager;
        NetDebug.Logger = new AdapterLog<NetworkManager>(logger);
    }

    public bool StartListener(int port = 0)
    {
        return _netManager.Start(port);
    }

    public void Start()
    {
        _netManager.Start();
    }

    public IAdapterNetPeer ConnectToServer(string address, int port, string key)
    {
        return new AdapterNetPeer(_netManager.Connect(address, port, key));
    }

    public IAdapterNetPeer ConnectToServer(IPEndPoint target, string key)
    {
        return new AdapterNetPeer(_netManager.Connect(target, key));
    }

    public void Stop()
    {
        if (_netManager.IsRunning)
            _netManager.Stop();
    }

    public void PollEvents()
    {
        _netManager.PollEvents();
    }

    public void TriggerEvents()
    {
        _netManager.TriggerUpdate();
    }

    public void Stop(bool sendDisconnectMessages)
    {
        _netManager.Stop(sendDisconnectMessages);
    }

    public int GetPeersCount(CustomConnectionState peerState)
    {
        
        return _netManager.GetPeersCount(Extensions.ConvertToConnectionState(peerState));
    }
    
    public int HasConnectedPeers()
    {
        return _netManager.ConnectedPeersCount;
    }

    public IAdapterNetPeer GetPeerById(int id)
    {
        return new AdapterNetPeer(_netManager.GetPeerById(id));
    }

    public IEnumerable<IAdapterNetPeer> GetPeers(CustomConnectionState state)
    {
        foreach (var peer in _netManager.ConnectedPeerList)
            if (peer.ConnectionState == Extensions.ConvertToConnectionState(state))
                yield return new AdapterNetPeer(peer);
    }

    public IAdapterNetPeer GetFirstPeer()
    {
        return new AdapterNetPeer(_netManager.FirstPeer);
    }

    public void DisconnectAll()
    {
        _netManager.DisconnectAll();
    }

    public void DisconnectAll(byte[] data, int start, int count)
    {
        _netManager.DisconnectAll(data, start, count);
    }

    public void DisconnectPeerForce(IAdapterNetPeer peer)
    {
        if (peer is not AdapterNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _netManager.DisconnectPeerForce(customNetPeer.Peer);
    }

    public void DisconnectPeer(IAdapterNetPeer peer)
    {
        if (peer is not AdapterNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _netManager.DisconnectPeer(customNetPeer.Peer);
    }

    public void DisconnectPeer(IAdapterNetPeer peer, byte[] data)
    {
        if (peer is not AdapterNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _netManager.DisconnectPeer(customNetPeer.Peer, data);
    }

    public void DisconnectPeer(IAdapterNetPeer peer, IAdapterDataWriter writer)
    {
        if (peer is not AdapterNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        if (writer is not AdapterDataWriter customDataWriter)
            throw new ArgumentException("Writer is not CustomDataWriter");

        _netManager.DisconnectPeer(customNetPeer.Peer, customDataWriter.GetNetDataWriter());
    }

    public void DisconnectPeer(IAdapterNetPeer peer, byte[] data, int start, int count)
    {
        if (peer is not AdapterNetPeer customNetPeer) throw new ArgumentException("Peer is not CustomNetPeer");

        _netManager.DisconnectPeer(customNetPeer.Peer, data, start, count);
    }

    public void CreateNtpRequest(IPEndPoint endPoint)
    {
        _netManager.CreateNtpRequest(endPoint);
    }

    public void CreateNtpRequest(string ntpServerAddress, int port)
    {
        _netManager.CreateNtpRequest(ntpServerAddress, port);
    }

    public void CreateNtpRequest(string ntpServerAddress)
    {
        _netManager.CreateNtpRequest(ntpServerAddress);
    }
    
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