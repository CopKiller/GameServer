using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class StartWindowScript : WindowBase
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnTextChanged(string newText, bool isUsername = false, bool isPassword = false)
	{
		if (isUsername)
		{
			GD.Print($"Username changed to: {newText}");
		}
		else if (isPassword)
		{
			GD.Print($"Password changed to: {newText}");
		}
		else
			GD.Print($"Text changed to: {newText}");
	}
}