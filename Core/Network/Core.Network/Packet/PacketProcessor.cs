using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Packet;
using Core.Network.Serialization;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Player;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public sealed class PacketProcessor : IPacketProcessor
{
    // TODO: Implement IPacketProcessor using the SOLID principles
    
    // DIP: IConnectionManager is an abstraction
    private readonly IConnectionManager _connectionManager;

    // OCP: NetPacketProcessor is open for extension, but closed for modification
    private readonly NetPacketProcessor _netPacketProcessor;
    
    // SRP: PacketRegister is responsible for registering packets
    private readonly PacketRegister _packetRegister;
    // SRP: PacketSender is responsible for sending packets
    private readonly PacketSender _packetSender;
    // SRP: PacketReceiver is responsible for receiving packets
    private readonly PacketReceiver _packetReceiver;
    // SRP: NetworkSerializer is responsible for serializing and deserializing packets
    private readonly NetworkSerializer _networkSerializer;
    
    public PacketProcessor(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
        
        var maxStringLength = 255;
        _netPacketProcessor = new NetPacketProcessor(maxStringLength);
        _packetRegister = new PacketRegister(_netPacketProcessor);
        _packetSender = new PacketSender(_netPacketProcessor, connectionManager);
        _packetReceiver = new PacketReceiver(_netPacketProcessor);
        _networkSerializer = new NetworkSerializer(_netPacketProcessor);
    }

    public void SendPacket<TPacket>(ICustomNetPeer peer, TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    => _packetSender.SendPacket(peer, packet, deliveryMethod);

    public void SendPacket<T>(ICustomNetPeer peer, ref T packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    => _packetSender.SendPacket(peer, ref packet, deliveryMethod);

    public void SendPacket(ICustomNetPeer peer, byte[] data,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    => _packetSender.SendPacket(peer, data, deliveryMethod);

    public void SendPacketToAll<TPacket>(TPacket packet,
        CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    => _packetSender.SendPacketToAll(packet, deliveryMethod);

    public void SendPacketToAll<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    => _packetSender.SendPacketToAll(ref packet, deliveryMethod);

    public void SendPacketToAll(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    => _packetSender.SendPacketToAll(data, deliveryMethod);

    public void RegisterNestedType<T>() where T : ICustomSerializable
    => _networkSerializer.RegisterNestedType<T>();

    public void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new()
    => _packetRegister.RegisterPacket(onReceive);

    public void UnregisterPackets()
    => _packetRegister.UnregisterPackets();

    public void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer)
    => _packetReceiver.ReadAllPackets(customNetPacketReader, customNetPeer);
}