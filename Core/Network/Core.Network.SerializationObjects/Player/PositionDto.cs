using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Player;

public class PositionDto : ICustomSerializable
{
    public float X { get; set; }
    public float Y { get; set; }
    public int Z { get; set; }
    public double Rotation { get; set; }
    
    public void Deserialize(ICustomDataReader reader)
    {
        X = reader.GetFloat();
        Y = reader.GetFloat();
        Z = reader.GetInt();
        Rotation = reader.GetDouble();
    }
    
    public void Serialize(ICustomDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Z);
        writer.Put(Rotation);
    }
}