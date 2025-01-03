using System;
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
    private Label? _LatencyLabel;
    
    public override void _Ready()
    {
        GD.Print("MainMenuScene ready!");
        
        _LatencyLabel = GetNode<Label>("LatencyLabel");

        var networkManager = ServiceManager.GetRequiredService<NetworkManager>();

        networkManager.Connect(NetworkManager.SignalName.NetworkLatencyUpdated, Callable.From<int>(ProcessLatency));
    }
    
    private void ProcessLatency(int latency)
    {
        if (_LatencyLabel == null)
        {
            return;
        }
        
        _LatencyLabel.Text = latency + "ms";

        switch (latency)
        {
            case < 50:
                _LatencyLabel.AddThemeColorOverride("font_color", new Color(0, 1, 0));
                break;
            case < 100:
                _LatencyLabel.AddThemeColorOverride("font_color", new Color(1, 1, 0));
                break;
            default:
                _LatencyLabel.AddThemeColorOverride("font_color", new Color(1, 0, 0));
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