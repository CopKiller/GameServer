using System;
using Core.Client.Network.Interface;
using Core.Network.Interface.Packet;
using Core.Network.Packets.Request;
using Core.Network.SerializationObjects.Enum;
using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Attributes;
using Game.Scripts.GameState;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

[ScenePath("res://Client/Scenes/MainScenes/MainMenuScene.tscn")]
public partial class MainMenuScript : Control
{
    private Label? _latencyLabel;
    
    [Export]
    public WindowBase? FirstWindowOpened { get; set; }
    
    public override void _Ready()
    {
        GD.Print("MainMenuScene ready!");
        
        _latencyLabel = GetNode<Label>("LatencyLabel");

        var networkManager = ServiceManager.GetRequiredService<NetworkManager>();

        networkManager.Connect(NetworkManager.SignalName.NetworkLatencyUpdated, Callable.From<int>(ProcessLatency));
        
        FirstWindowOpened?.Show();
        
        SendTestMessage();
    }
    
    private void SendTestMessage()
    {
        var sender = ServiceManager.GetRequiredService<IClientPacketRequest>();
        
        LoginRequest request = new LoginRequest
        {
            Username = "Test",
            Password = "Test"
        };
        
        sender.SendPacket(request);
    }
    
    private void ProcessLatency(int latency)
    {
        if (_latencyLabel == null)
        {
            return;
        }
        
        _latencyLabel.Text = "Ping " + latency + "ms";

        switch (latency)
        {
            case < 50:
                _latencyLabel.AddThemeColorOverride("font_color", new Color(0, 1, 0));
                break;
            case < 100:
                _latencyLabel.AddThemeColorOverride("font_color", new Color(1, 1, 0));
                break;
            default:
                _latencyLabel.AddThemeColorOverride("font_color", new Color(1, 0, 0));
                break;
        }
    }

    private void OnExitButtonPressed()
    {
        var clientManager = this.GetSingleton<GameStateManager>();
        clientManager.ExitGame();
    }
    
    public void CloseAllWindows()
    {
        GetTree().CallGroup("MainMenuWindows", Window.MethodName.Hide);
    }
}