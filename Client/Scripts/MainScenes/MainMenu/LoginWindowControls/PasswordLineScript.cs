using Core.Client.Validator;
using Game.Scripts.Extensions;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu.LoginWindowControls;

public partial class PasswordLineScript : LineEdit
{
	// TODO: Remover essa classe e centralizar tudo no LoginWindow invocando sinais.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void OnTextChanged(string newText)
	{
		GD.Print($"Password changed to: {newText}");

		var result = EntitiesValidator.ValidateAccountPassword(newText, false);
		
		this.ChangeThemeColor(result.IsValid);
	}
}