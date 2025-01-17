using Core.Network.Interface;
using Core.Network.Interface.Event;
using Core.Network.SerializationObjects;
using Microsoft.Extensions.Logging;

namespace Core.Server.Session;

public class SessionManager : ISessionManager
{
    private readonly Dictionary<int, AccountSession> _accountSessions = [];
    
    private readonly Dictionary<int, PlayerSession> _playerSessions = [];
    
    private readonly ILogger<SessionManager> _logger;
    
    public SessionManager(INetworkEventsListener listener, ILogger<SessionManager> logger)
    {
        _logger = logger;
        
        // listener.OnPeerConnected += (peer) =>
        // {
        //     logger.LogDebug($"Peer connected - id: {peer.Id} address: {peer.EndPoint}");
        //     
        //     AddAccountSession(peer, new AccountSession());
        //     AddPlayerSession(peer, new PlayerSession());
        // };
        
        listener.OnPeerDisconnected += RemoveAllSessions;
    }
    
    public void AddPlayerSession(IAdapterNetPeer peer, PlayerDto player)
    {
        var session = new PlayerSession
        {
            CurrentPlayer = player,
            CurrentPeer = peer
        };
        
        _playerSessions[peer.Id] = session;
    }
    
    public void AddAccountSession(IAdapterNetPeer peer, AccountDto account)
    {
        var session = new AccountSession
        {
            CurrentAccount = account,
            CurrentPeer = peer
        };
        
        _accountSessions[peer.Id] = session;
    }
    
    public AccountSession? GetAccountSession(int connectionId)
    {
        _accountSessions.TryGetValue(connectionId, out var session);
        return session;
    }

    public PlayerSession? GetPlayerSession(int connectionId)
    {
        _playerSessions.TryGetValue(connectionId, out var session);
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
    
    private void RemoveAllSessions(IAdapterNetPeer peer, IAdapterDisconnectInfo reason)
    {
        _logger.LogDebug($"Peer disconnected - id: {peer.Id} address: {peer.EndPoint} ({reason.Reason})");
        
        _accountSessions.Remove(peer.Id);
        _playerSessions.Remove(peer.Id);
    }
}