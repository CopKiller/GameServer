using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class LiteNetLibLoggerAdapter<T>(ILogger<T> logger) : INetLogger
{
    public void WriteNet(CustomNetLogLevel level, string str, params object[] args) 
        => logger.Log(Extensions.ConvertToLogLevel(level), str, args);
    
    public void WriteNet(NetLogLevel level, string str, params object[] args) =>
        WriteNet(Extensions.ConvertToCustomNetLogLevel(level), str, args);
    
}