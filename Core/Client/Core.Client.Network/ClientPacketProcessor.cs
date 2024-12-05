using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Core.Network.Packets.Client;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network;

public class ClientPacketProcessor(
    IPacketProcessor packetProcessor,
    INetworkEventsListener netListener,
    ILogger<ClientPacketProcessor> logger) : IClientPacketProcessor
{
    private readonly ClientNetworkPacket _clientNetworkPacket = new(logger);
    private ICustomNetPeer? ServerPeer { get; set; }

    public void Initialize(ICustomNetPeer peer)
    {
        ServerPeer = peer;

        packetProcessor.RegisterPacket<CPacketFirst>(_clientNetworkPacket.OnFirstPacket);
        packetProcessor.RegisterPacket<CPacketSecond>(_clientNetworkPacket.OnSecondPacket);

        netListener.OnNetworkReceive += ProcessPacket;
    }

    public void Stop()
    {
        packetProcessor.UnregisterPackets();

        netListener.OnNetworkReceive -= ProcessPacket;

        ServerPeer = null;
    }

    private void ProcessPacket(ICustomNetPeer peer, ICustomNetPacketReader reader, byte channel,
        CustomDeliveryMethod deliveryMethod)
    {
        ReadAllPackets(reader, peer);
    }

    public void SendPacket<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        packetProcessor.SendPacket(ServerPeer, packet, deliveryMethod);
    }

    public void SendPacket<T>(ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        packetProcessor.SendPacket(ServerPeer, ref packet, deliveryMethod);
    }

    public void SendPacket(byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        packetProcessor.SendPacket(ServerPeer, data, deliveryMethod);
    }

    public void RegisterNestedType<T>() where T : ICustomSerializable
    {
        packetProcessor.RegisterNestedType<T>();
    }

    public void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new()
    {
        packetProcessor.RegisterPacket(onReceive);
    }

    public void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer)
    {
        packetProcessor.ReadAllPackets(customNetPacketReader, customNetPeer);
    }
}