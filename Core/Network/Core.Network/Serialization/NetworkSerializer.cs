using Core.Network.Interface;
using Core.Network.Interface.Serialization;
using LiteNetLib.Utils;

namespace Core.Network.Serialization;

public class NetworkSerializer(NetPacketProcessor packetProcessor) : INetworkSerializer
{
    public void RegisterNestedType<T>() where T : IAdapterSerializable, new()
    {
        packetProcessor.RegisterNestedType<T>(
            (writer, obj) => obj.Serialize(new AdapterDataWriter(writer)),
            reader =>
            {
                var instance = new T();
                instance.Deserialize(new AdapterDataReader(reader));
                return instance;
            }
        );
    }
}