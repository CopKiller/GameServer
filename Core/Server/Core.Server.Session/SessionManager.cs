using Core.Network.Interface;
using Core.Network.Interface.Connection;
using Core.Network.Interface.Event;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Enum;
using Microsoft.Extensions.Logging;

namespace Core.Server.Session;

public class SessionManager : ISessionManager
{
    private readonly Dictionary<int, AccountSession> _accountSessions = [];
    
    private readonly IConnectionManager _connectionManager;
    private readonly ILogger<SessionManager> _logger;
    
    public SessionManager(INetworkEventsListener listener, IConnectionManager connectionManager, ILogger<SessionManager> logger)
    {
        _connectionManager = connectionManager;
        _logger = logger;
        
        listener.OnPeerDisconnected += RemoveAccountSession;
    }
    
    public void AddAccountSession(IAdapterNetPeer peer, AccountDto account, ClientState clientState = ClientState.CharacterSelection)
    {
        var session = new AccountSession
        {
            CurrentAccount = account,
            CurrentPeer = peer,
            ClientState = clientState,
            IsLoggedIn = true
        };
        
        _accountSessions[peer.Id] = session;
    }
    
    public void LogoutAccountSession(int connectionId)
    {
        if (_accountSessions.TryGetValue(connectionId, out var session))
        {
            _connectionManager.DisconnectPeer(connectionId);
            _logger.LogInformation($"Account session {connectionId} logged out.");
        }
        else
        {
            _logger.LogWarning($"Account session {connectionId} not found.");
        }
    }
    
    public AccountSession? GetAccountSession(int connectionId)
    {
        _accountSessions.TryGetValue(connectionId, out var session);
        return session;
    }
    
    public bool HasAccountSession(string username)
    {
        return _accountSessions.Values.Any(a => a.Username == username);
    }
    
    public bool HasAccountSession(int accountId)
    {
        return _accountSessions.Values.Any(a => a.AccountId == accountId);
    }
    
    private void RemoveAccountSession(IAdapterNetPeer peer, IAdapterDisconnectInfo reason)
    {
        _logger.LogDebug($"Peer disconnected - id: {peer.Id} address: {peer.EndPoint} ({reason.Reason})");
        
        _accountSessions.Remove(peer.Id);
    }
}