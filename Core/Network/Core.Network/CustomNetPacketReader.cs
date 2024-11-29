using Core.Network.Interface;
using LiteNetLib;

namespace Core.Network;

public sealed class CustomNetPacketReader(NetPacketReader reader) : ICustomNetPacketReader
{
    void ICustomNetPacketReader.Recycle() => reader.Recycle();
}