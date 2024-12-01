using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface ICustomPacketProcessor
{
    public void SendPacket<TPacket>(ICustomNetPeer iCustomNetPeer, TPacket packet, CustomDeliveryMethod deliveryMethod)
        where TPacket : class, new();

    void RegisterNestedType<T>() where T : ICustomSerializable;

    void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new();
    
    void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer);
}