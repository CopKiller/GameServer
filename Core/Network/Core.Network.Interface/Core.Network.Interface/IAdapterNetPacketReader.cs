namespace Core.Network.Interface;

public interface IAdapterNetPacketReader : IAdapterDataReader
{
    void Recycle();
}