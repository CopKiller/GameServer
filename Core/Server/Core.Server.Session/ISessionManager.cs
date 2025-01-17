using Core.Network.Interface;
using Core.Network.SerializationObjects;

namespace Core.Server.Session;

public interface ISessionManager
{
    AccountSession? GetAccountSession(int connectionId);
    PlayerSession? GetPlayerSession(int connectionId);
    void AddAccountSession(IAdapterNetPeer peer, AccountDto account);
    void AddPlayerSession(IAdapterNetPeer peer, PlayerDto player);
    bool HasAccountSession(string username);
    bool HasAccountSession(int accountId);
}