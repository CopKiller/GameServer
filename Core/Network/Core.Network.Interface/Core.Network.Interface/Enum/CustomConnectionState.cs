namespace Core.Network.Interface.Enum;

/// <summary>
/// Peer connection state
/// </summary>
[Flags]
public enum CustomConnectionState : byte
{
    Outgoing = 1 << 1,
    Connected = 1 << 2,
    ShutdownRequested = 1 << 3,
    Disconnected = 1 << 4,
    EndPointChange = 1 << 5,
    Any = Outgoing | Connected | ShutdownRequested | EndPointChange
}