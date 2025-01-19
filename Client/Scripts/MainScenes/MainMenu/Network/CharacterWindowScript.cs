
using Core.Client.Network.Interface;
using Core.Network.Packets.Request;
using Core.Network.SerializationObjects;
using Game.Scripts.Singletons;

// ReSharper disable once CheckNamespace
namespace Game.Scripts.MainScenes.MainMenu;

public partial class CharacterWindowScript
{
    private readonly IClientPacketRequest? _packetRequest 
        = ServiceManager.GetRequiredService<IClientPacketRequest>();

    private void SendLogout()
    {
        var logoutRequest = new LogoutRequest();
        
        _packetRequest?.SendPacket(logoutRequest);
    }
}