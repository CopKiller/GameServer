using System.Numerics;
using Core.Network.Interface;

namespace Core.Network.SerializationObjects.Player;

public struct Vector2 : IEquatable<Vector2>, IAdapterSerializable
{
    public float X { get; set; }
    public float Y { get; set; }

    public void Deserialize(IAdapterDataReader reader)
    {
        X = reader.GetFloat();
        Y = reader.GetFloat();
    }

    public void Serialize(IAdapterDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
    }

    public bool Equals(Vector2 other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }
}