namespace Core.Network.Interface;

public interface IAdapterSerializable
{
    void Serialize(IAdapterDataWriter writer);
    void Deserialize(IAdapterDataReader reader);
}