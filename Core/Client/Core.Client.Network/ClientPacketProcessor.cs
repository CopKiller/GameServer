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
    IClientConnectionManager clientConnectionManager,
    ILogger<ClientPacketProcessor> logger) : IClientPacketProcessor
{
    private readonly ClientNetworkPacket _clientNetworkPacket = new(logger);

    public void Initialize()
    {
        packetProcessor.RegisterPacket<CPacketFirst>(_clientNetworkPacket.OnFirstPacket);
        packetProcessor.RegisterPacket<CPacketSecond>(_clientNetworkPacket.OnSecondPacket);

        netListener.OnNetworkReceive += ProcessPacket;
    }

    public void Stop()
    {
        packetProcessor.UnregisterPackets();

        netListener.OnNetworkReceive -= ProcessPacket;
    }

    private void ProcessPacket(ICustomNetPeer peer, ICustomNetPacketReader reader, byte channel,
        CustomDeliveryMethod deliveryMethod)
    {
        ReadAllPackets(reader, peer);
    }
    
    private bool IsServerPeerConnected => clientConnectionManager.CurrentPeer != null && clientConnectionManager.IsConnected;

    public void SendPacket<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketProcessor)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetProcessor.SendPacket(clientConnectionManager.CurrentPeer, packet, deliveryMethod);
    }

    public void SendPacket<T>(ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketProcessor)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetProcessor.SendPacket(clientConnectionManager.CurrentPeer, ref packet, deliveryMethod);
    }

    public void SendPacket(byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketProcessor)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetProcessor.SendPacket(clientConnectionManager.CurrentPeer, data, deliveryMethod);
    }

    public void RegisterNestedType<T>() where T : ICustomSerializable, new()
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