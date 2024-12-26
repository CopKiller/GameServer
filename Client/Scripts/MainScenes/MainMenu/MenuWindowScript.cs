using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class MenuWindowScript : WindowBase
{
	private Button? _startButton;
	private Button? _registerButton;
	private Button? _exitButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startButton = GetNode<Button>("%StartButton");
		_registerButton = GetNode<Button>("%RegisterButton");
		_exitButton = GetNode<Button>("%ExitButton");
	}
}