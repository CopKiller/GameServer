using Core.Network.Interface.Enum;

namespace Core.Network.Interface.Packet;

public interface IPacketHandler
{
    void ReadAllPackets(IAdapterNetPeer adapterNetPeer, IAdapterNetPacketReader adapterNetPacketReader, byte channel,
        CustomDeliveryMethod deliveryMethod);
}