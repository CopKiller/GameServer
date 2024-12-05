using Core.Network.Interface;
using Core.Network.Interface.Enum;

namespace Core.Client.Network.Interface;

public interface IClientPacketProcessor
{
    public void Initialize(ICustomNetPeer peer);
    public void Stop();
    public void SendPacket<TPacket>(TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
        where TPacket : class, new();
    
    void SendPacket<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) 
        where T : ICustomSerializable;

    void SendPacket(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered);
    
    void RegisterNestedType<T>() where T : ICustomSerializable;

    void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new();
    
    void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer);
}