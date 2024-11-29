using Core.Network.Interface;
using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

public class LiteNetLibLoggerAdapter(ILogger<LiteNetLibLoggerAdapter> logger) : INetLogger
{
    public void WriteNet(CustomNetLogLevel level, string str, params object[] args) => logger.Log(level switch
    {
        CustomNetLogLevel.Trace => LogLevel.Trace,
        CustomNetLogLevel.Info => LogLevel.Information,
        CustomNetLogLevel.Warning => LogLevel.Warning,
        CustomNetLogLevel.Error => LogLevel.Error,
        _ => LogLevel.Information
    }, str, args);
    
    public void WriteNet(NetLogLevel level, string str, params object[] args) =>
        WriteNet(Extensions.ConvertToCustomNetLogLevel(level), str, args);
    
}