using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class LiteNetSerializableAdapter<T> : INetSerializable where T : ICustomSerializable
{
    private readonly T _instance;

    public LiteNetSerializableAdapter(T instance)
    {
        _instance = instance;
    }
    
    public LiteNetSerializableAdapter()
    {
        _instance = Activator.CreateInstance<T>();
    }

    public void Serialize(NetDataWriter writer)
    {
        var customWriter = new CustomDataWriter(writer);
        _instance.Serialize(customWriter);
    }

    public void Deserialize(NetDataReader reader)
    {
        var customReader = new CustomDataReader(reader);
        _instance.Deserialize(customReader);
    }
}