using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network.Serialization;

internal class SerializationService(NetPacketProcessor packetProcessor)
{
    private readonly SerializationRegistrar _serializationRegistrar = new(packetProcessor);
    
    internal void Initialize()
    {
        _serializationRegistrar.RegisterDtOs();
    }
    
    internal void RegisterNestedType<T>() where T : ICustomSerializable
    {
        _serializationRegistrar.RegisterNestedType<T>();
    }
}