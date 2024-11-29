using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface ICustomNetLogger
{
    void WriteNet(CustomNetLogLevel level, string str, params object[] args);
}