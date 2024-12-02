using Core.Network.Interface;
using Core.Network.Interface.Enum;

namespace Core.Server.Network.Interface;

public interface IServerNetworkProcessor
{
    public void Initialize();
    
    public void SendPacket<TPacket>(ICustomNetPeer peer, TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
        where TPacket : class, new();
    
    void SendPacket<T>(ICustomNetPeer peer, ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) 
        where T : ICustomSerializable;

    void SendPacket(ICustomNetPeer peer, byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered);
    
    void SendPacketToAll<TPacket>(TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
        where TPacket : class, new();
    
    void SendPacketToAll<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) 
        where T : ICustomSerializable;
    
    void SendPacketToAll(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered);
    
    void RegisterNestedType<T>() where T : ICustomSerializable;

    void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new();
}