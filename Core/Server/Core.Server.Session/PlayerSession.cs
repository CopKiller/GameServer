using Core.Network.Interface;
using Core.Network.SerializationObjects;

namespace Core.Server.Session;

public class PlayerSession
{
    public PlayerDto? CurrentPlayer { get; set; }
    
    public void Logout()
    {
        CurrentPlayer = null;
    }
}