using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class CustomDataReader(NetDataReader reader) : ICustomDataReader
{
    private readonly NetDataReader _reader = reader ?? throw new ArgumentNullException(nameof(reader));

    public int GetInt()
    {
        return _reader.GetInt();
    }

    public float GetFloat()
    {
        return _reader.GetFloat();
    }

    public string GetString()
    {
        return _reader.GetString();
    }

    public bool GetBool()
    {
        return _reader.GetBool();
    }

    public byte[] GetBytesWithLength()
    {
        return _reader.GetBytesWithLength();
    }
}