using Core.Client.Network.Interface;
using Core.Client.Validator;
using Core.Network.Packets.Request;
using Core.Network.SerializationObjects;
using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class LoginWindowScript : WindowBase
{
	[Export]
	public WindowBase? NextWindow { get; set; }
	
	private IClientPacketRequest? _packetRequest;
	private LineEdit? _usernameLineEdit;
	private LineEdit? _passwordLineEdit;
	private Button? _hidePasswordButton;
	private Button? _loginButton;
	
	private bool _isPasswordVisible = false;
	
	public override void _Ready()
	{
		_usernameLineEdit = GetNode<LineEdit>("%UsernameLine");
		_passwordLineEdit = GetNode<LineEdit>("%PasswordLine");
		_hidePasswordButton = GetNode<Button>("%HidePasswordButton");
		_loginButton = GetNode<Button>("%LoginButton");
		
		_packetRequest = ServiceManager.GetRequiredService<IClientPacketRequest>();
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
	
	private void OnLoginButtonPressed()
	{
		GD.Print("Login button pressed!");
		
		// var alertManager = ServiceManager.GetRequiredService<AlertManager>();
		// alertManager.AddGlobalAlert("Login button pressed!");
		//
		// alertManager.AddGlobalAlert("Register button pressed!");
		//
		// return;
		
		if (_usernameLineEdit == null) return;
		if (_passwordLineEdit == null) return;
		
		var username = _usernameLineEdit.Text;
		var password = _passwordLineEdit.Text;

		var userValid = EntitiesValidator.ValidateAccountUsername(username);
		var passwordValid = EntitiesValidator.ValidateAccountPassword(password);
		
		_usernameLineEdit.ChangeThemeColor(userValid.IsValid);
		_passwordLineEdit.ChangeThemeColor(passwordValid.IsValid);
		
		if (!userValid.IsValid || !passwordValid.IsValid)
		{
			return;
		}

		var loginRequest = new LoginRequest
		{
			Account =
			{
				Username = username,
				Password = password
			}
		};

		_packetRequest?.SendPacket(loginRequest);
	}
}