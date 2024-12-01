using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface ICustomNetPeer
{
    int Id { get; }
    void Send(byte[] data, CustomDeliveryMethod deliveryMethod);
}