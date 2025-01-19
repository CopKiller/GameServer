namespace Core.Network.Interface.Packet;

public interface IPacketRegister
{
    void RegisterPacket<TPacket>(Action<TPacket, IAdapterNetPeer> onReceive) where TPacket : class, new();

    void UnregisterPacket<TPacket>();
    
    void UnregisterPackets();
}