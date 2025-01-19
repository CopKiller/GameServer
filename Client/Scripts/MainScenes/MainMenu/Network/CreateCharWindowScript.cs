
using Core.Client.Network.Interface;
using Core.Network.Packets.Request;
using Core.Network.SerializationObjects;
using Game.Scripts.Singletons;

// ReSharper disable once CheckNamespace
namespace Game.Scripts.MainScenes.MainMenu;

public partial class CreateCharWindowScript
{
    private readonly IClientPacketRequest? _packetRequest 
        = ServiceManager.GetRequiredService<IClientPacketRequest>();

    private void SendCreateChar(string name)
    {
        // TODO: Implement new packet for CreateCharacter.

        var createCharPacket = new CreateCharRequest
        {
            Player = new PlayerDto
            {
                Name = name
            }
        };
        
        _packetRequest?.SendPacket(createCharPacket);
    }
}