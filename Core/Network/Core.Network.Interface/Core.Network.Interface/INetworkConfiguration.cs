using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface INetworkConfiguration
{
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