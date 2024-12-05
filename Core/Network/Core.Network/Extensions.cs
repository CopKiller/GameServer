using Core.Network.Interface.Enum;
using LiteNetLib;
using Microsoft.Extensions.Logging;

namespace Core.Network;

internal static class Extensions
{
    internal static CustomDeliveryMethod ConvertToCustomDeliveryMethod(DeliveryMethod deliveryMethod)
    {
        return deliveryMethod switch
        {
            DeliveryMethod.ReliableOrdered => CustomDeliveryMethod.ReliableOrdered,
            DeliveryMethod.ReliableUnordered => CustomDeliveryMethod.ReliableUnordered,
            DeliveryMethod.ReliableSequenced => CustomDeliveryMethod.ReliableSequenced,
            DeliveryMethod.Unreliable => CustomDeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };
    }
    
    internal static DeliveryMethod ConvertToLiteDeliveryMethod(CustomDeliveryMethod deliveryMethod)
    {
        return deliveryMethod switch
        {
            CustomDeliveryMethod.ReliableOrdered => DeliveryMethod.ReliableOrdered,
            CustomDeliveryMethod.ReliableUnordered => DeliveryMethod.ReliableUnordered,
            CustomDeliveryMethod.ReliableSequenced => DeliveryMethod.ReliableSequenced,
            CustomDeliveryMethod.Unreliable => DeliveryMethod.Unreliable,
            _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
        };
    }
    
    internal static CustomUnconnectedMessageType ConvertToCustomUnconnectedMessageType(UnconnectedMessageType messageType)
    {
        return messageType switch
        {
            UnconnectedMessageType.BasicMessage => CustomUnconnectedMessageType.BasicMessage,
            UnconnectedMessageType.Broadcast => CustomUnconnectedMessageType.Broadcast,
            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
    
    internal static NetLogLevel ConvertToLiteNetLogLevel(CustomNetLogLevel logLevel)
    {
        return logLevel switch
        {
            
            CustomNetLogLevel.Trace => NetLogLevel.Trace,
            CustomNetLogLevel.Info => NetLogLevel.Info,
            CustomNetLogLevel.Warning => NetLogLevel.Warning,
            CustomNetLogLevel.Error => NetLogLevel.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
    
    internal static LogLevel ConvertToLogLevel(CustomNetLogLevel logLevel)
    {
        return logLevel switch
        {
            CustomNetLogLevel.Trace => LogLevel.Trace,
            CustomNetLogLevel.Info => LogLevel.Information,
            CustomNetLogLevel.Warning => LogLevel.Warning,
            CustomNetLogLevel.Error => LogLevel.Error,
            _ => LogLevel.Information
        };
    }
    
    internal static CustomNetLogLevel ConvertToCustomNetLogLevel(NetLogLevel logLevel)
    {
        return logLevel switch
        {
            NetLogLevel.Trace => CustomNetLogLevel.Trace,
            NetLogLevel.Info => CustomNetLogLevel.Info,
            NetLogLevel.Warning => CustomNetLogLevel.Warning,
            NetLogLevel.Error => CustomNetLogLevel.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };
    }
    
    internal static ConnectionState ConvertToConnectionState(CustomConnectionState peerState)
    {
        return peerState switch
        {
            CustomConnectionState.Outgoing => ConnectionState.Outgoing,
            CustomConnectionState.Connected => ConnectionState.Connected,
            CustomConnectionState.ShutdownRequested => ConnectionState.ShutdownRequested,
            CustomConnectionState.Disconnected => ConnectionState.Disconnected,
            CustomConnectionState.EndPointChange => ConnectionState.EndPointChange,
            CustomConnectionState.Any => ConnectionState.Any,
            _ => throw new ArgumentOutOfRangeException(nameof(peerState), peerState, null)
        };
    }
}