
using Core.Client.Network.Interface;
using Game.Scripts.Singletons;

// ReSharper disable once CheckNamespace
namespace Game.Scripts.MainScenes.MainMenu;

public partial class CreateCharWindowScript
{
    private IClientPacketRequest? _packetRequest 
        = ServiceManager.GetRequiredService<IClientPacketRequest>();
    
    private void SendCreateChar()
    {
        // TODO: Implement new packet for CreateCharacter.
    }
}