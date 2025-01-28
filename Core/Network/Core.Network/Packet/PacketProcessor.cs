using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Enum;
using Core.Network.Interface.Event;
using Core.Network.Interface.Packet;
using Core.Network.Interface.Serialization;
using Core.Network.Serialization;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Player;
using Core.Network.SerializationObjects.Response;
using LiteNetLib.Utils;

namespace Core.Network.Packet;

public sealed class PacketProcessor : IPacketProcessor
{
    // TODO: Implement IPacketProcessor using the SOLID principles

    // OCP: NetPacketProcessor is open for extension, but closed for modification
    private readonly NetPacketProcessor _netPacketProcessor;

    // SRP: PacketRegister is responsible for registering packets
    public IPacketRegister PacketRegister { get; }

    // SRP: PacketSender is responsible for sending packets
    public IPacketSender PacketSender { get; }

    // SRP: PacketReceiver is responsible for receiving packets
    public IPacketHandler PacketHandler { get; }
    
    // SRP: NetworkSerializer is responsible for serializing and deserializing packets
    public INetworkSerializer NetworkSerializer { get; }
    
    const int MaxStringLength = 255;

    public PacketProcessor(IConnectionManager connectionManager, INetworkEventsListener networkEventsListener)
    {
        _netPacketProcessor = new NetPacketProcessor(MaxStringLength);
        PacketRegister = new PacketRegister(_netPacketProcessor);
        PacketSender = new PacketRequest(_netPacketProcessor, connectionManager);
        PacketHandler = new PacketHandler(_netPacketProcessor, networkEventsListener);
        NetworkSerializer = new NetworkSerializer(_netPacketProcessor);

        NetworkSerializer.RegisterNestedType<AccountDto>();
        NetworkSerializer.RegisterNestedType<PlayerDto>();
        NetworkSerializer.RegisterNestedType<Vector2>();
        NetworkSerializer.RegisterNestedType<VitalsDto>();
        NetworkSerializer.RegisterNestedType<StatsDto>();
        
        NetworkSerializer.RegisterNestedType<ResponseDto>();
    }
}