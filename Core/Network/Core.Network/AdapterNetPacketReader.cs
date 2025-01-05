using Core.Network.Interface;
using LiteNetLib;

namespace Core.Network;

public sealed class AdapterNetPacketReader(NetPacketReader reader) : AdapterDataReader(reader), IAdapterNetPacketReader
{
    void IAdapterNetPacketReader.Recycle()
    {
        reader.Recycle();
    }

    internal NetPacketReader GetReader => reader;
}