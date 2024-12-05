using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network.Serialization;

public class NetworkSerializer(NetPacketProcessor packetProcessor)
{
    public void RegisterNestedType<T>() where T : ICustomSerializable
    {
        packetProcessor.RegisterNestedType(() => new LiteNetSerializableAdapter<T>());
    }
}