using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network;

public sealed class CustomDataWriter : ICustomDataWriter
{
    private readonly NetDataWriter _writer;

    public CustomDataWriter(NetDataWriter writer)
    {
        _writer = writer;
    }
    
    public CustomDataWriter()
    {
        _writer = new NetDataWriter();
    }

    public void Put(int value)
    {
        _writer.Put(value);
    }

    public void Put(float value)
    {
        _writer.Put(value);
    }

    public void Put(string value)
    {
        _writer.Put(value);
    }

    public void Put(bool value)
    {
        _writer.Put(value);
    }

    public void Put(byte[] data)
    {
        _writer.Put(data);
    }

    public NetDataWriter GetNetDataWriter()
    {
        return _writer;
    }
}
