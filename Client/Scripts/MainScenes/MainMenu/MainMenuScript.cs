using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Game.Scripts.Extensions.Attributes;
using Game.Scripts.Extensions.Services;
using Game.Scripts.Singletons;
using Godot;
using Godot.NativeInterop;

namespace Game.Scripts.MainScenes.MainMenu;

[ScenePath("res://Client/Scenes/MainScenes/MainMenuScene.tscn")]
public partial class MainMenuScript : Control
{
    [Export]
    private WindowBase? FirstWindowOpened { get; set; }
    
    
    private Label? _latencyLabel;
    private LoginWindowScript? _loginWindow;
    private CharacterWindowScript? _characterWindow;
    
    public override void _Ready()
    {
        GD.Print("MainMenuScene ready!");
        
        _latencyLabel = GetNode<Label>("LatencyLabel");
        _loginWindow = GetNode<LoginWindowScript>("%LoginWindow");
        _characterWindow = GetNode<CharacterWindowScript>("%CharacterWindow");

        var networkManager = ServiceManager.GetRequiredService<NetworkManager>();

        networkManager.Connect(NetworkManager.SignalName.NetworkLatencyUpdated, Callable.From<int>(ProcessLatency));
        
        FirstWindowOpened?.Show();
    }
    
    public void EnterCharacterSelection()
    {
        _loginWindow?.Hide();
        _characterWindow?.Show();
    }
    
    // private void SendTestMessage()
    // {
    //     var sender = ServiceManager.GetRequiredService<IClientPacketRequest>();
    //     
    //     LoginRequest request = new LoginRequest
    //     {
    //         Username = "Test",
    //         Password = "Test"
    //     };
    //     
    //     sender.SendPacket(request);
    // }
    
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