using Core.Network.Interface;

namespace Core.Client.Network.Interface;

public interface IClientNetworkService
{
    bool IsConnected { get; }
    
    ICustomNetPeer? GetServerPeer();
}