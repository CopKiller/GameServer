using System;
using Core.Network.Interface;
using Core.Network.Packets.Interface.Response;
using Core.Network.Packets.Response;
using Core.Network.SerializationObjects.Enum;
using Game.Scripts.Singletons;

namespace Game.Scripts.Network.Handler;

public class LogoutNetHandler(
    GameStateManager gameStateManager,
    AlertManager alertManager
    ) : IHandlerResponse<LogoutResponse>
{
    public void HandleResponse(LogoutResponse packet, IAdapterNetPeer peer)
    {
        if (packet.Response.Success)
            HandleSuccess(packet, peer);
        else
            HandleFailure(packet, peer);
    }

    public void HandleSuccess(LogoutResponse packet, IAdapterNetPeer peer)
    {
        gameStateManager.ReceiveChangeState(packet.ClientState);
    }

    public void HandleFailure(LogoutResponse packet, IAdapterNetPeer peer)
    {
        alertManager.AddGlobalAlert(packet.Response.Message);
    }
}