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

    public IPEndPoint GetIpEndPoint() => _reader.GetNetEndPoint();

    public byte GetByte() => _reader.GetByte();

    public sbyte GetSByte() => _reader.GetSByte();

    public bool GetBool() => _reader.GetBool();

    public char GetChar() => (char)_reader.GetUShort();

    public ushort GetUShort() => _reader.GetUShort();

    public short GetShort() => _reader.GetShort();

    public ulong GetULong() => _reader.GetULong();

    public long GetLong() => _reader.GetLong();

    public uint GetUInt() => _reader.GetUInt();

    public int GetInt() => _reader.GetInt();

    public double GetDouble() => _reader.GetDouble();

    public float GetFloat() => _reader.GetFloat();

    public string GetString() => _reader.GetString();

    public string GetString(int maxLength) => _reader.GetString(maxLength);

    public Guid GetGuid() => _reader.GetGuid();

    public T[] GetArray<T>(ushort size) where T : struct, ICustomSerializable
    {
        var array = new T[size];
        for (ushort i = 0; i < size; i++)
        {
            array[i] = Get<T>();
        }
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

    public bool[] GetBoolArray() => _reader.GetBoolArray();

    public ushort[] GetUShortArray() => _reader.GetUShortArray();

    public short[] GetShortArray() => _reader.GetShortArray();

    public int[] GetIntArray() => _reader.GetIntArray();

    public uint[] GetUIntArray() => _reader.GetUIntArray();

    public float[] GetFloatArray() => _reader.GetFloatArray();

    public double[] GetDoubleArray() => _reader.GetDoubleArray();

    public long[] GetLongArray() => _reader.GetLongArray();

    public ulong[] GetULongArray() => _reader.GetULongArray();

    public string[] GetStringArray() => _reader.GetStringArray();

    public string[] GetStringArray(int maxStringLength) => _reader.GetStringArray(maxStringLength);

    public ArraySegment<byte> GetBytesSegment(int count) => _reader.GetBytesSegment(count);

    public ArraySegment<byte> GetRemainingBytesSegment() => _reader.GetRemainingBytesSegment();

    public byte[] GetRemainingBytes() => _reader.GetRemainingBytes();

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