using System.Net;

namespace Core.Network.Interface;

public interface ICustomDataWriter
{
    byte[] CopyData();
    void Put(float value);
    void Put(double value);
    void Put(long value);
    void Put(ulong value);
    void Put(int value);
    void Put(uint value);
    void Put(char value);
    void Put(ushort value);
    void Put(short value);
    void Put(sbyte value);
    void Put(byte value);
    void Put(Guid value);
    void Put(byte[] data, int offset, int length);
    void Put(params byte[] data);
    void PutSBytesWithLength(sbyte[] data, int offset, ushort length);
    void PutSBytesWithLength(params sbyte[] data);
    void PutBytesWithLength(byte[] data, int offset, ushort length);
    void PutBytesWithLength(params byte[] data);
    void Put(bool value);
    void PutArray(Array arr, int sz);
    void PutArray(params float[] value);
    void PutArray(params double[] value);
    void PutArray(params long[] value);
    void PutArray(params ulong[] value);
    void PutArray(params int[] value);
    void PutArray(params uint[] value);
    void PutArray(params ushort[] value);
    void PutArray(params short[] value);
    void PutArray(params bool[] value);
    void PutArray(params string[] value);
    void PutArray(string[] value, int strMaxLength);
    void PutArray<T>(params T[] value) where T : ICustomSerializable, new();
    void Put(IPEndPoint endPoint);
    void PutLargeString(string value);
    void Put(string value);
    void Put(string value, int maxLength);
    void Put<T>(T obj) where T : ICustomSerializable;
}