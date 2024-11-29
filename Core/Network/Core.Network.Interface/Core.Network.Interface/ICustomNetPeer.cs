using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface ICustomNetPeer
{
    void Send(byte[] data, CustomDeliveryMethod deliveryMethod);
}