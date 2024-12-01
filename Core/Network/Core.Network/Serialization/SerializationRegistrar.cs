using Core.Network.Interface;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Player;
using LiteNetLib.Utils;

namespace Core.Network.Serialization;

internal class SerializationRegistrar(NetPacketProcessor packetProcessor)
{
    internal void RegisterDtOs()
    {
        RegisterNestedType<AccountDto>();
        RegisterNestedType<PlayerDto>();
        RegisterNestedType<StatsDto>();
        RegisterNestedType<VitalsDto>();
        RegisterNestedType<PositionDto>();
        // Adicione todos os DTOs aqui.
    }

    internal void RegisterNestedType<T>() where T : ICustomSerializable
    {
        packetProcessor.RegisterNestedType(() => new LiteNetSerializableAdapter<T>());
    }
}