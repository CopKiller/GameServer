using Core.Network.Interface;

namespace Core.Server.Network.Interface;

public interface IServerNetworkService
{
    int GetConnectedPlayersCount();
    
    void SendPacketToAllPlayers<T>(T packet) where T : ICustomSerializable;
}