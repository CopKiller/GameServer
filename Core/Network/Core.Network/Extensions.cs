using Core.Network.Interface.Enum;
using LiteNetLib;

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
}