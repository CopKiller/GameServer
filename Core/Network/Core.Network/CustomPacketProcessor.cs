using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class CustomPacketProcessor : ICustomPacketProcessor
{
    // TODO: Implement ICustomPacketProcessor using the SOLID principles

    // Max string length for packet data
    private const byte MaxStringLength = 255;

    private readonly NetPacketProcessor _netPacketProcessor = new(MaxStringLength);
    
    public void RegisterNestedType<T>() where T : ICustomSerializable
    {
        _netPacketProcessor.RegisterNestedType(() => new LiteNetSerializableAdapter<T>());
    }
    
    public void SendPacket<TPacket>(ICustomNetPeer iCustomNetPeer, TPacket packet, CustomDeliveryMethod deliveryMethod) where TPacket : class, new()
    {
        if (iCustomNetPeer is not CustomNetPeer liteNetPeer)
            throw new InvalidOperationException("Invalid iCustomNetPeer type. Expected CustomNetPeer.");

        _netPacketProcessor.Send(liteNetPeer.Peer, packet, ConvertToLiteDeliveryMethod(deliveryMethod));
    }

    private DeliveryMethod ConvertToLiteDeliveryMethod(CustomDeliveryMethod deliveryMethod)
    {
        return deliveryMethod switch
        {
            CustomDeliveryMethod.ReliableOrdered => DeliveryMethod.ReliableOrdered,
            CustomDeliveryMethod.ReliableUnordered => DeliveryMethod.ReliableUnordered,
            CustomDeliveryMethod.ReliableSequenced => DeliveryMethod.ReliableSequenced,
            CustomDeliveryMethod.Unreliable => DeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };
    }

    // Exemplo

    // public void SendPacket(Packet packet)

    // public void ReceivePacket(Packet packet)

    // public void ProcessPacket(Packet packet)
}