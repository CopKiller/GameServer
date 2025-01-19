using Core.Network.Interface;
using Core.Network.Interface.Enum;

namespace Core.Client.Network.Interface;

public interface IClientPacketRequest
{
    public void SendPacket<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
        where TPacket : class, new();

    void SendPacket<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
        where T : IAdapterSerializable;

    void SendPacket(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered);
}