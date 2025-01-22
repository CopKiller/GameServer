using Game.Scripts.BaseControls;
using Game.Scripts.Extensions.Attributes;
using Game.Scripts.Extensions.Services;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

[ScenePath("res://Client/Scenes/MainScenes/MainMenuScene.tscn")]
public partial class MainMenuScript : Control
{
	[Export]
	private WindowBase? FirstWindowOpened { get; set; }
	
	private Label? _latencyLabel;
	private LoginWindowScript? _loginWindow;
	
	public CharacterWindowScript? CharacterWindow { get; private set; }
	
	public override void _Ready()
	{
		GD.Print("MainMenuScene ready!");
		
		_latencyLabel = GetNode<Label>("LatencyLabel");
		_loginWindow = GetNode<LoginWindowScript>("%LoginWindow");
		CharacterWindow = GetNode<CharacterWindowScript>("%CharacterWindow");

		var networkManager = ServiceManager.GetRequiredService<NetworkManager>();
		
		FirstWindowOpened?.Show();
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
