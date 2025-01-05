using System.Net;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface IAdapterNetManager
{
    bool StartListener(int port = 0);
    void Start();
    IAdapterNetPeer ConnectToServer(string address, int port, string key);
    IAdapterNetPeer ConnectToServer(IPEndPoint target, string key);
    void Stop();
    void PollEvents();
    void TriggerEvents();
    void Stop(bool sendDisconnectMessages);
    int GetPeersCount(CustomConnectionState peerState);
    IEnumerable<IAdapterNetPeer> GetPeers(CustomConnectionState state);
    IAdapterNetPeer GetFirstPeer();
    void DisconnectAll();
    void DisconnectAll(byte[] data, int start, int count);
    void DisconnectPeerForce(IAdapterNetPeer peer);
    void DisconnectPeer(IAdapterNetPeer peer);
    void DisconnectPeer(IAdapterNetPeer peer, byte[] data);
    void DisconnectPeer(IAdapterNetPeer peer, IAdapterDataWriter writer);
    void DisconnectPeer(IAdapterNetPeer peer, byte[] data, int start, int count);
    void CreateNtpRequest(IPEndPoint endPoint);
    void CreateNtpRequest(string ntpServerAddress, int port);
    void CreateNtpRequest(string ntpServerAddress);
    
    string Address { get; set; }
    int Port { get; set; }
    string Key { get; set; }
    bool UnconnectedMessagesEnabled { get; set; }
    bool NatPunchEnabled { get; set; }
    int UpdateTime { get; set; }
    int PingInterval { get; set; }
    int DisconnectTimeout { get; set; }
    bool SimulatePacketLoss { get; set; }
    bool SimulateLatency { get; set; }
    int SimulationPacketLossChance { get; set; }
    int SimulationMinLatency { get; set; }
    int SimulationMaxLatency { get; set; }
    bool UnsyncedEvents { get; set; }
    bool UnsyncedReceiveEvent { get; set; }
    bool UnsyncedDeliveryEvent { get; set; }
    bool BroadcastReceiveEnabled { get; set; }
    int ReconnectDelay { get; set; }
    int MaxConnectAttempts { get; set; }
    bool ReuseAddress { get; set; }
    bool DontRoute { get; set; }
    bool EnableStatistics { get; set; }
    bool IsRunning { get; }
    int LocalPort { get; }
    bool AutoRecycle { get; set; }
    bool IPv6Enabled { get; set; }
    int MtuOverride { get; set; }
    bool MtuDiscovery { get; set; }
    bool UseNativeSockets { get; set; }
    bool DisconnectOnUnreachable { get; set; }
    bool AllowPeerAddressChange { get; set; }
}