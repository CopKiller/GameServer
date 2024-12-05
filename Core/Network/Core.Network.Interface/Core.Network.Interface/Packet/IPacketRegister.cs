namespace Core.Network.Interface.Packet;

public interface IPacketRegister
{
    void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new();

    void UnregisterPackets();
}