using Core.Network.Interface;
using Core.Network.SerializationObjects;
using Core.Network.SerializationObjects.Enum;

namespace Core.Server.Session;

public class AccountSession
{
    public bool IsLoggedIn { get; set; }
    
    public ClientState ClientState { get; set; }
    
    public string Username => CurrentAccount?.Username ?? string.Empty;
    public int AccountId => CurrentAccount?.Id ?? 0;
    public AccountDto? CurrentAccount { get; set; }
    
    public IAdapterNetPeer? CurrentPeer { get; set; }
}