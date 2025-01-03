using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class LoginWindowScript : WindowBase
{
	private Button? _hidePasswordButton;
	
	private bool _isPasswordVisible = false;
	
	public override void _Ready()
	{
		_hidePasswordButton = GetNode<Button>("%HidePasswordButton");
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

	private void OnHidePasswordButtonPressed()
	{
		if (_hidePasswordButton == null)
		{
			return;
		}
		
		_isPasswordVisible = !_isPasswordVisible;
		
		var eyeImage = _hidePasswordButton.GetChild<TextureRect>(0);

		eyeImage.Texture = _isPasswordVisible
			? GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/ShowImage.png")
			: GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/HideImage.png");
		
		var passwordField = GetNode<LineEdit>("%PasswordLine");
		
		passwordField.Secret = !_isPasswordVisible;
	}
}