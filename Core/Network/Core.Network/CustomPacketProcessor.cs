using Core.Network.Interface;
using Core.Network.Interface.Enum;
using Core.Network.Serialization;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class CustomPacketProcessor : ICustomPacketProcessor
{
    // TODO: Implement ICustomPacketProcessor using the SOLID principles

    // Max string length for packet data
    private const byte MaxStringLength = 255;

    private readonly NetPacketProcessor _netPacketProcessor;
    
    private readonly SerializationService _serializationService;
    
    // Use IConnectionManager to get all peers and send packets to all peers
    private readonly IConnectionManager _manager;
    
    public CustomPacketProcessor(IConnectionManager manager)
    {
        _netPacketProcessor = new NetPacketProcessor(MaxStringLength);
        _serializationService = new SerializationService(_netPacketProcessor);
        _manager = manager;
        
        _serializationService.Initialize();
    }
    
    public void RegisterNestedType<T>() where T : ICustomSerializable
    {
        _serializationService.RegisterNestedType<T>();
    }
    
    public void RegisterPacket<TPacket>(Action<TPacket, ICustomNetPeer> onReceive) where TPacket : class, new()
    {
        _netPacketProcessor.SubscribeReusable(onReceive);
    }
    
    public void UnregisterPackets()
    {
        _netPacketProcessor.ClearSubscriptions();
    }
    
    public void ReadAllPackets(ICustomNetPacketReader customNetPacketReader, ICustomNetPeer customNetPeer)
    {
        if (customNetPacketReader is not CustomNetPacketReader netPacketReader)
            throw new InvalidOperationException("Invalid customNetPacketReader type. Expected CustomNetPacketReader.");
        
        _netPacketProcessor.ReadAllPackets(netPacketReader.GetReader, customNetPeer);
    }
    
    public void SendPacket<TPacket>(ICustomNetPeer peer, TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        if (peer is not CustomNetPeer liteNetPeer)
            throw new InvalidOperationException("Invalid peer type. Expected CustomNetPeer.");

        _netPacketProcessor.Send(liteNetPeer.Peer, packet, ConvertToLiteDeliveryMethod(deliveryMethod));
    }
    
    public void SendPacket<T>(ICustomNetPeer peer, ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        if (peer is not CustomNetPeer liteNetPeer)
            throw new InvalidOperationException("Invalid peer type. Expected CustomNetPeer.");

        var adapter = new LiteNetSerializableAdapter<T>(packet);
        _netPacketProcessor.SendNetSerializable(liteNetPeer.Peer, ref adapter, ConvertToLiteDeliveryMethod(deliveryMethod));
    }
    
    public void SendPacket(ICustomNetPeer peer, byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        peer.Send(data, deliveryMethod);
    }
    
    public void SendPacketToAll<TPacket>(TPacket packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where TPacket : class, new()
    {
        var allPeers = _manager.CustomPeers.Values;
        
        foreach (var peer in allPeers)
        {
            SendPacket(peer, packet, deliveryMethod);
        }
    }
    
    public void SendPacketToAll<T>(ref T packet, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered) where T : ICustomSerializable
    {
        
        var allPeers = _manager.CustomPeers.Values;
        
        var adapter = new LiteNetSerializableAdapter<T>(packet);
        
        foreach (var peer in allPeers)
        {
            SendPacket(peer, adapter, deliveryMethod);
        }
    }
    
    public void SendPacketToAll(byte[] data, CustomDeliveryMethod deliveryMethod = CustomDeliveryMethod.ReliableOrdered)
    {
        var allPeers = _manager.CustomPeers.Values;
        
        foreach (var peer in allPeers)
        {
            peer.Send(data, deliveryMethod);
        }
    }

    private DeliveryMethod ConvertToLiteDeliveryMethod(CustomDeliveryMethod deliveryMethod)
    {
        return deliveryMethod switch
        {
            CustomDeliveryMethod.ReliableOrdered => DeliveryMethod.ReliableOrdered,
            CustomDeliveryMethod.ReliableUnordered => DeliveryMethod.ReliableUnordered,
            CustomDeliveryMethod.ReliableSequenced => DeliveryMethod.ReliableSequenced,
            CustomDeliveryMethod.Unreliable => DeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };
    }

    // Exemplo

    // public void SendPacket(Packet packet)

    // public void ReceivePacket(Packet packet)

    // public void ProcessPacket(Packet packet)
}