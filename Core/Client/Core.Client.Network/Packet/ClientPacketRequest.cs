using Core.Client.Network.Interface;
using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Microsoft.Extensions.Logging;

namespace Core.Client.Network.Packet;

public class ClientPacketRequest(
    IPacketSender packetSender,
    IClientConnectionManager clientConnectionManager,
    ILogger<ClientPacketRequest> logger) : IClientPacketRequest
{
    private bool IsServerPeerConnected => clientConnectionManager.CurrentPeer != null && clientConnectionManager.IsConnected;

    public void SendPacket<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketRequest)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetSender.SendPacket(clientConnectionManager.CurrentPeer, packet, deliveryMethod);
    }

    public void SendPacket<T>(ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : IAdapterSerializable
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketRequest)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetSender.SendPacket(clientConnectionManager.CurrentPeer, ref packet, deliveryMethod);
    }

    public void SendPacket(byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        if (!IsServerPeerConnected)
        {
            logger.LogError($"ServerPeer not connected in method: {nameof(SendPacket)} in class: {nameof(ClientPacketRequest)}");
            return;
        }

        if (clientConnectionManager.CurrentPeer != null)
            packetSender.SendPacket(clientConnectionManager.CurrentPeer, data, deliveryMethod);
    }
}