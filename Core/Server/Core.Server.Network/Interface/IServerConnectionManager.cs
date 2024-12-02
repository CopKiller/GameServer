using Core.Network.Interface;

namespace Core.Server.Network.Interface;

public interface IServerConnectionManager
{
    /// <summary>
    /// Lista de peers customizados.
    /// </summary>
    IReadOnlyDictionary<int, ICustomNetPeer> CustomPeers { get; }

    /// <summary>
    /// Desconecta um peer específico.
    /// </summary>
    /// <param name="peer">Peer a ser desconectado.</param>
    /// <param name="reason">Razão da desconexão.</param>
    void DisconnectPeer(ICustomNetPeer peer, string reason = "Disconnected");

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
    public ICustomNetPeer? GetPeerById(int id);

    /// <summary>
    /// Obter todos os peers conectados.
    /// </summary>
    IEnumerable<ICustomNetPeer> GetPeers();
}