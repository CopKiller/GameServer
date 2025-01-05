using Core.Network.Interface.Enum;
using Core.Network.Interface.Serialization;

namespace Core.Network.Interface.Packet;

public interface IPacketProcessor
{
    IPacketRegister PacketRegister { get; }
    IPacketSender PacketSender { get; }
    IPacketHandler PacketHandler { get; }
    INetworkSerializer NetworkSerializer { get; }
}