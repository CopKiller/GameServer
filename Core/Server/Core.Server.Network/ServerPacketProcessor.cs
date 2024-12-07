using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Core.Network.Packets.Server;
using Core.Server.Network.Interface;
using Microsoft.Extensions.Logging;

namespace Core.Server.Network;

public class ServerPacketProcessor(
    IPacketProcessor packetProcessor,
    INetworkEventsListener netListener,
    ILogger<ServerPacketProcessor> logger) : IServerPacketProcessor
{
    private readonly ServerNetworkPacket _serverNetworkPacket = new(packetProcessor, logger);

    public void Initialize()
    {
        RegisterPacket<SPacketFirst>(_serverNetworkPacket.OnFirstPacket);
        RegisterPacket<SPacketSecond>(_serverNetworkPacket.OnSecondPacket);

        netListener.OnNetworkReceive += ProcessPacket;
    }

    public void SendPacket<TPacket>(ICustomNetPeer peer, TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        packetProcessor.SendPacket(peer, packet, deliveryMethod);
    }

    public void SendPacket<T>(ICustomNetPeer peer, ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        packetProcessor.SendPacket(peer, ref packet, deliveryMethod);
    }

    public void SendPacket(ICustomNetPeer peer, byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        packetProcessor.SendPacket(peer, data, deliveryMethod);
    }

    public void SendPacketToAll<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        packetProcessor.SendPacketToAll(packet, deliveryMethod);
    }

    public void SendPacketToAll<T>(ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        packetProcessor.SendPacketToAll(ref packet, deliveryMethod);
    }

    public void SendPacketToAll(byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        packetProcessor.SendPacketToAll(data, deliveryMethod);
    }

    public void RegisterNestedType<T>() where T : ICustomSerializable, new()
    {
        packetProcessor.RegisterNestedType<T>();
    }

    public void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new()
    {
        packetProcessor.RegisterPacket(onReceive);
    }

    private void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer)
    {
        packetProcessor.ReadAllPackets(customNetPacketReader, customNetPeer);
    }

    private void ProcessPacket(ICustomNetPeer peer, ICustomNetPacketReader reader, byte channel,
        CustomDeliveryMethod deliveryMethod)
    {
        ReadAllPackets(reader, peer);
    }
}