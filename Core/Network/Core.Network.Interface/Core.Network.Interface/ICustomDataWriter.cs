namespace Core.Network.Interface;

public interface ICustomDataWriter
{
    void Put(int value);
    void Put(float value);
    void Put(string value);
    void Put(bool value);
    void Put(byte[] data);
}