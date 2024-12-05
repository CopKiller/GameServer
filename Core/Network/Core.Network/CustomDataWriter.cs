using System.Net;
using Core.Network.Interface;
using LiteNetLib.Utils;
using ArgumentNullException = System.ArgumentNullException;

namespace Core.Network;

public sealed class CustomDataWriter(NetDataWriter writer) : ICustomDataWriter
{
    public CustomDataWriter() : this(new NetDataWriter())
    {
    }

    internal NetDataWriter GetNetDataWriter()
    {
        return writer;
    }

    private void SerializeUsingAdapter<T>(T obj) where T : ICustomSerializable
    {
        var adapter = new LiteNetSerializableAdapter<T>(obj);
        adapter.Serialize(writer);
    }

    private LiteNetSerializableAdapter<T>[] AdaptArray<T>(T[] value) where T : ICustomSerializable, new()
    {
        return Array.ConvertAll(value, item => new LiteNetSerializableAdapter<T>(item));
    }

    public byte[] CopyData()
    {
        return writer.CopyData();
    }

    public void Put(float value)
    {
        writer.Put(value);
    }

    public void Put(double value)
    {
        writer.Put(value);
    }

    public void Put(long value)
    {
        writer.Put(value);
    }

    public void Put(ulong value)
    {
        writer.Put(value);
    }

    public void Put(int value)
    {
        writer.Put(value);
    }

    public void Put(uint value)
    {
        writer.Put(value);
    }

    public void Put(char value)
    {
        writer.Put(value);
    }

    public void Put(ushort value)
    {
        writer.Put(value);
    }

    public void Put(short value)
    {
        writer.Put(value);
    }

    public void Put(sbyte value)
    {
        writer.Put(value);
    }

    public void Put(byte value)
    {
        writer.Put(value);
    }

    public void Put(Guid value)
    {
        writer.Put(value);
    }

    public void Put(byte[] data, int offset, int length)
    {
        writer.Put(data, offset, length);
    }

    public void Put(params byte[] data)
    {
        writer.Put(data);
    }

    public void PutSBytesWithLength(sbyte[] data, int offset, ushort length)
    {
        writer.PutSBytesWithLength(data, offset, length);
    }

    public void PutSBytesWithLength(params sbyte[] data)
    {
        writer.PutSBytesWithLength(data);
    }

    public void PutBytesWithLength(byte[] data, int offset, ushort length)
    {
        writer.PutBytesWithLength(data, offset, length);
    }

    public void PutBytesWithLength(params byte[] data)
    {
        writer.PutBytesWithLength(data);
    }

    public void Put(bool value)
    {
        writer.Put(value);
    }

    public void PutArray(Array arr, int sz)
    {
        writer.PutArray(arr, sz);
    }

    public void PutArray(params float[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params double[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params long[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params ulong[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params int[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params uint[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params ushort[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params short[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params bool[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(params string[] value)
    {
        writer.PutArray(value);
    }

    public void PutArray(string[] value, int strMaxLength)
    {
        writer.PutArray(value, strMaxLength);
    }

    public void PutArray<T>(params T[] value) where T : ICustomSerializable, new()
    {
        var adapters = AdaptArray(value);
        writer.PutArray(adapters);
    }

    public void Put(IPEndPoint endPoint)
    {
        writer.Put(endPoint);
    }

    public void PutLargeString(string value)
    {
        writer.PutLargeString(value);
    }

    public void Put(string value)
    {
        writer.Put(value);
    }

    public void Put(string value, int maxLength)
    {
        writer.Put(value, maxLength);
    }

    public void Put<T>(T obj) where T : ICustomSerializable
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        SerializeUsingAdapter(obj);
    }
}