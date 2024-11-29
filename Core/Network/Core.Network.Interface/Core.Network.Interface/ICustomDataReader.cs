namespace Core.Network.Interface;

public interface ICustomDataReader
{
    int GetInt();
    float GetFloat();
    string GetString();
    bool GetBool();
    byte[] GetBytesWithLength();
}