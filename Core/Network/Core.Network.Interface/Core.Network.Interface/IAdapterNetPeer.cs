using System.Net;
using Core.Network.Interface.Enum;

namespace Core.Network.Interface;

public interface IAdapterNetPeer
{
    int Id { get; }

    bool IsConnected { get; }

    IPAddress EndPoint { get; }

    // TODO: Deixar os métodos de envio gerenciados a partir do ConnectionManager ou algo do tipo...
    void Send(byte[] data, CustomDeliveryMethod deliveryMethod);
    
    void Send(IAdapterDataWriter writer, CustomDeliveryMethod deliveryMethod);
}