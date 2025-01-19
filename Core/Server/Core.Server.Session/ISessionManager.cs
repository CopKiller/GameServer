using Core.Network.Interface;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Enum;

namespace Core.Server.Session;

public interface ISessionManager
{
    AccountSession? GetAccountSession(int connectionId);
    void AddAccountSession(IAdapterNetPeer peer, AccountDto account, ClientState clientState = ClientState.CharacterSelection);
    void LogoutAccountSession(int connectionId);
    bool HasAccountSession(string username);
    bool HasAccountSession(int accountId);
}