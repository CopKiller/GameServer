using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public class PacketSender(NetPacketProcessor packetProcessor, IConnectionManager manager) : IPacketSender
{
    [ThreadStatic] // Evita problemas de concorrência
    private static CustomDataWriter? _writer;

    private CustomDataWriter Writer => _writer ??= new CustomDataWriter();

    public void SendPacket<TPacket>(
        ICustomNetPeer peer,
        TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    ) where TPacket : class, new()
    {
        var writer = Writer;
        writer.Reset();
        packetProcessor.Write(writer.GetNetDataWriter(), packet); // Usa o NetPacketProcessor diretamente
        peer.Send(writer, deliveryMethod);
    }

    public void SendPacket<T>(
        ICustomNetPeer peer,
        ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    ) where T : ICustomSerializable
    {
        var writer = Writer;
        writer.Reset();
        SerializePacket(ref packet, writer); // Centraliza a lógica de serialização
        peer.Send(writer, deliveryMethod);
    }

    public void SendPacket(
        ICustomNetPeer peer,
        byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    )
    {
        peer.Send(data, deliveryMethod); // Pacotes simples já estão prontos
    }

    public void SendPacketToAll<TPacket>(
        TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    ) where TPacket : class, new()
    {
        var writer = Writer;
        writer.Reset();
        packetProcessor.Write(writer.GetNetDataWriter(), packet); // Serializa uma vez

        foreach (var peer in manager.CustomPeers.Values)
        {
            peer.Send(writer, deliveryMethod);
        }
    }

    public void SendPacketToAll<T>(
        ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    ) where T : ICustomSerializable
    {
        var writer = Writer;
        writer.Reset();
        SerializePacket(ref packet, writer); // Serializa uma vez

        foreach (var peer in manager.CustomPeers.Values)
        {
            peer.Send(writer, deliveryMethod);
        }
    }

    public void SendPacketToAll(
        byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered
    )
    {
        foreach (var peer in manager.CustomPeers.Values)
        {
            peer.Send(data, deliveryMethod);
        }
    }

    private void SerializePacket<T>(ref T packet, CustomDataWriter writer) where T : ICustomSerializable
    {
        packetProcessor.WriteHash<T>(writer.GetNetDataWriter());
        packet.Serialize(writer);
    }
}