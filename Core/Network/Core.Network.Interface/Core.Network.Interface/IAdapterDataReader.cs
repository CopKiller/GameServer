using System.Net;
using Core.Network.Interface;

namespace Core.Network.Interface;

public interface IAdapterDataReader
{
    // Métodos genéricos para desserialização de tipos
    T Get<T>() where T : struct, IAdapterSerializable;
    T Get<T>(Func<T> constructor) where T : class, IAdapterSerializable;

    // Métodos para tipos primitivos e tipos específicos
    IPEndPoint GetIpEndPoint();
    byte GetByte();
    sbyte GetSByte();
    bool GetBool();
    char GetChar();
    ushort GetUShort();
    short GetShort();
    ulong GetULong();
    long GetLong();
    uint GetUInt();
    int GetInt();
    double GetDouble();
    float GetFloat();
    string GetString();
    string GetString(int maxLength);
    Guid GetGuid();

    // Métodos para arrays
    T[] GetArray<T>(ushort size) where T : struct, IAdapterSerializable;
    T[] GetArray<T>() where T : IAdapterSerializable, new();
    T[] GetArray<T>(Func<T> constructor) where T : class, IAdapterSerializable;

    bool[] GetBoolArray();
    ushort[] GetUShortArray();
    short[] GetShortArray();
    int[] GetIntArray();
    uint[] GetUIntArray();
    float[] GetFloatArray();
    double[] GetDoubleArray();
    long[] GetLongArray();
    ulong[] GetULongArray();
    string[] GetStringArray();
    string[] GetStringArray(int maxStringLength);

    // Métodos para bytes
    ArraySegment<byte> GetBytesSegment(int count);
    ArraySegment<byte> GetRemainingBytesSegment();
    byte[] GetRemainingBytes();
    byte[] GetBytes(int start, int count);
    byte[] GetBytes(int count);
}