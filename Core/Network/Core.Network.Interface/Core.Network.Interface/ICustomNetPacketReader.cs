namespace Core.Network.Interface;

public interface ICustomNetPacketReader : ICustomDataReader
{
    void Recycle();
}