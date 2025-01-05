using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface IAdapterNetLogger
{
    void WriteNet(CustomNetLogLevel level, string str, params object[] args);
}