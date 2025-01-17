using Core.Network.Interface;

namespace Core.Server.Network.Interface;

public interface IServerConnectionManager
{
    void ConfigureNetworkSettings();
    bool StartListener();

    /// <summary>
    /// Desconecta um peer específico.
    /// </summary>
    /// <param name="peer">Peer a ser desconectado.</param>
    /// <param name="reason">Razão da desconexão.</param>
    void DisconnectPeer(IAdapterNetPeer peer, string reason = "Disconnected");

    /// <summary>
    /// Desconecta todos os peers conectados.
    /// </summary>
    void DisconnectAll();

    /// <summary>
    /// Verifica se há peers conectados.
    /// </summary>
    bool HasConnectedPeers { get; }

    /// <summary>
    /// Obtém um peer pelo seu ID.
    /// </summary>
    /// <param name="id">ID do peer.</param>
    /// <returns>O peer correspondente, ou null se não encontrado.</returns>
    public IAdapterNetPeer? GetPeerById(int id);
}