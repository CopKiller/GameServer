using Core.Network.Interface;
using LiteNetLib;

namespace Core.Network;

public sealed class CustomNetPacketReader(NetPacketReader reader) : CustomDataReader(reader), ICustomNetPacketReader
{
    void ICustomNetPacketReader.Recycle() => reader.Recycle();
    
    internal NetPacketReader GetReader => reader;
}