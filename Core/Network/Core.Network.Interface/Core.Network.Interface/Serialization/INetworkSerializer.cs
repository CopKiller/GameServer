namespace Core.Network.Interface.Serialization;

public interface INetworkSerializer
{
    void RegisterNestedType<T>() where T : IAdapterSerializable, new();
}