using Core.Network.Interface;

namespace Core.Client.Network.Interface;

public interface IClientConnectionManager
{
    /// <summary>
    /// Desconecta o jogador local.
    /// </summary>
    void Disconnect();
    

    /// <summary>
    /// Obter o peer do servidor.
    /// </summary>
    ICustomNetPeer GetServerPeer();
}