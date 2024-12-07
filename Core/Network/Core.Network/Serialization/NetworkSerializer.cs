using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network.Serialization;

public class NetworkSerializer(NetPacketProcessor packetProcessor)
{
    public void RegisterNestedType<T>() where T : ICustomSerializable, new()
    {
        packetProcessor.RegisterNestedType<T>(
            (writer, obj) => obj.Serialize(new CustomDataWriter(writer)),
            reader =>
            {
                var instance = new T();
                instance.Deserialize(new CustomDataReader(reader));
                return instance;
            }
        );
    }
}