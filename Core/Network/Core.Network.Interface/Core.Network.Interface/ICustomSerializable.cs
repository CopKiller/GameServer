namespace Core.Network.Interface;

public interface ICustomSerializable
{
    void Serialize(ICustomDataWriter writer);
    void Deserialize(ICustomDataReader reader);
}