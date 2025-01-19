using Core.Client.Validator;
using Game.Scripts.Extensions;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu.LoginWindowControls;

public partial class UsernameLineScript : LineEdit
{
	// TODO: Remover essa classe e centralizar tudo no LoginWindow invocando sinais.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void OnTextChanged(string newText)
	{
		GD.Print($"Username changed to: {newText}");

		var result = EntitiesValidator.ValidateAccountUsername(newText);
		
		this.ChangeThemeColor(result.IsValid);
	}
}