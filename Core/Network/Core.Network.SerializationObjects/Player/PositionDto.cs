using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Player;

public class PositionDto : IAdapterSerializable
{
    public float X { get; set; }
    public float Y { get; set; }
    public int Z { get; set; }
    public double Rotation { get; set; }

    public void Deserialize(IAdapterDataReader reader)
    {
        X = reader.GetFloat();
        Y = reader.GetFloat();
        Z = reader.GetInt();
        Rotation = reader.GetDouble();
    }

    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Z);
        writer.Put(Rotation);
    }
}