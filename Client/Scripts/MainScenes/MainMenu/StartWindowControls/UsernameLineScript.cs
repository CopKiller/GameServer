using Core.Client.Validator;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu.StartWindowControls;

public partial class UsernameLineScript : LineEdit
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void OnTextChanged(string newText)
	{
		GD.Print($"Username changed to: {newText}");

		var result = EntitiesValidator.ValidateAccountUsername(newText);
		
		if (result.IsValid)
			this.RemoveThemeColorOverride("font_color");
		else
			this.AddThemeColorOverride("font_color", new Color(1,0,0));
	}
}