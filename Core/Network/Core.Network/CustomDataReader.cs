using System;
using System.Net;
using Core.Network.Interface;
using LiteNetLib.Utils;

namespace Core.Network;

public class CustomDataReader : ICustomDataReader
{
    private readonly NetDataReader _reader;

    public CustomDataReader(NetDataReader reader)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    }

    public T Get<T>() where T : struct, ICustomSerializable
    {
        var obj = default(T);
        obj.Deserialize(this);
        return obj;
    }

    public T Get<T>(Func<T> constructor) where T : class, ICustomSerializable
    {
        var obj = constructor();
        obj.Deserialize(this);
        return obj;
    }

    public IPEndPoint GetIpEndPoint()
    {
        return _reader.GetNetEndPoint();
    }

    public byte GetByte()
    {
        return _reader.GetByte();
    }

    public sbyte GetSByte()
    {
        return _reader.GetSByte();
    }

    public bool GetBool()
    {
        return _reader.GetBool();
    }

    public char GetChar()
    {
        return (char)_reader.GetUShort();
    }

    public ushort GetUShort()
    {
        return _reader.GetUShort();
    }

    public short GetShort()
    {
        return _reader.GetShort();
    }

    public ulong GetULong()
    {
        return _reader.GetULong();
    }

    public long GetLong()
    {
        return _reader.GetLong();
    }

    public uint GetUInt()
    {
        return _reader.GetUInt();
    }

    public int GetInt()
    {
        return _reader.GetInt();
    }

    public double GetDouble()
    {
        return _reader.GetDouble();
    }

    public float GetFloat()
    {
        return _reader.GetFloat();
    }

    public string GetString()
    {
        return _reader.GetString();
    }

    public string GetString(int maxLength)
    {
        return _reader.GetString(maxLength);
    }

    public Guid GetGuid()
    {
        return _reader.GetGuid();
    }

    public T[] GetArray<T>(ushort size) where T : struct, ICustomSerializable
    {
        var array = new T[size];
        for (ushort i = 0; i < size; i++) array[i] = Get<T>();
        return array;
    }

    public T[] GetArray<T>() where T : ICustomSerializable, new()
    {
        var count = _reader.GetUShort();
        var array = new T[count];
        for (ushort i = 0; i < count; i++)
        {
            var obj = new T();
            obj.Deserialize(this);
            array[i] = obj;
        }

        return array;
    }

    public T[] GetArray<T>(Func<T> constructor) where T : class, ICustomSerializable
    {
        var count = _reader.GetUShort();
        var array = new T[count];
        for (ushort i = 0; i < count; i++)
        {
            var obj = constructor();
            obj.Deserialize(this);
            array[i] = obj;
        }

        return array;
    }

    public bool[] GetBoolArray()
    {
        return _reader.GetBoolArray();
    }

    public ushort[] GetUShortArray()
    {
        return _reader.GetUShortArray();
    }

    public short[] GetShortArray()
    {
        return _reader.GetShortArray();
    }

    public int[] GetIntArray()
    {
        return _reader.GetIntArray();
    }

    public uint[] GetUIntArray()
    {
        return _reader.GetUIntArray();
    }

    public float[] GetFloatArray()
    {
        return _reader.GetFloatArray();
    }

    public double[] GetDoubleArray()
    {
        return _reader.GetDoubleArray();
    }

    public long[] GetLongArray()
    {
        return _reader.GetLongArray();
    }

    public ulong[] GetULongArray()
    {
        return _reader.GetULongArray();
    }

    public string[] GetStringArray()
    {
        return _reader.GetStringArray();
    }

    public string[] GetStringArray(int maxStringLength)
    {
        return _reader.GetStringArray(maxStringLength);
    }

    public ArraySegment<byte> GetBytesSegment(int count)
    {
        return _reader.GetBytesSegment(count);
    }

    public ArraySegment<byte> GetRemainingBytesSegment()
    {
        return _reader.GetRemainingBytesSegment();
    }

    public byte[] GetRemainingBytes()
    {
        return _reader.GetRemainingBytes();
    }

    public byte[] GetBytes(int start, int count)
    {
        var buffer = new byte[count];
        _reader.GetBytes(buffer, start, count);
        return buffer;
    }

    public byte[] GetBytes(int count)
    {
        var buffer = new byte[count];
        _reader.GetBytes(buffer, count);
        return buffer;
    }
}