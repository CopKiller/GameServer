using System;
using Core.Client.Network.Interface;
using Core.Client.Validator;
using Core.Network.Packets.Request;
using Core.Network.SerializationObjects;
using Game.Scripts.BaseControls;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class RegisterWindowScript : WindowBase
{
	private IClientPacketRequest? _packetRequest;
	
	[Export] public DatePicker? DatePickWindow;
	
	private LineEdit? _usernameLineEdit;
	private LineEdit? _passwordLineEdit;
	private LineEdit? _confirmPasswordLineEdit;
	private LineEdit? _birthdateLineEdit;
	private LineEdit? _emailLineEdit;
	private Button? _confirmRegisterButton;
	
	private Button? _hideRegisterPasswordButton;
	private Button? _hideRegisterConfirmPasswordButton;
	bool _isPasswordVisible = false;
	bool _isConfirmPasswordVisible = false;
	
	public override void _Ready()
	{
		_packetRequest = ServiceManager.GetRequiredService<IClientPacketRequest>();
		
		_usernameLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/UsernameLineEdit");
		_passwordLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/PasswordLineEdit");
		_confirmPasswordLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer2/ConfirmPasswordLineEdit");
		_birthdateLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/BirthdateLineEdit");
		_emailLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/EmailLineEdit");

		_confirmRegisterButton = GetNode<Button>("MarginContainer/VBoxContainer/ConfirmButton");
		
		_hideRegisterPasswordButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer/HideRegisterPasswordButton");
		_hideRegisterConfirmPasswordButton = GetNode<Button>("MarginContainer/VBoxContainer/HBoxContainer2/HideRegisterConfirmPasswordButton");
		
		DatePickWindow?.Connect(DatePicker.SignalName.DateSelected, Callable.From<string>((text) =>
		{
			this.Show();
			_birthdateLineEdit.Text = text;
		}));
		
		DatePickWindow?.Connect(Window.SignalName.CloseRequested, Callable.From(Show));

		_usernameLineEdit.MaxLength = Core.Database.Constants.CharactersLength.MaxUsernameLength;
		_passwordLineEdit.MaxLength = Core.Database.Constants.CharactersLength.MaxPasswordLength;
		_confirmPasswordLineEdit.MaxLength = Core.Database.Constants.CharactersLength.MaxPasswordLength;
		_emailLineEdit.MaxLength = Core.Database.Constants.CharactersLength.MaxEmailLength;
	}
	
	public void ShowDatePicker()
	{
		if (_birthdateLineEdit == null || DatePickWindow == null) return;
		
		this.Hide();
		DatePickWindow?.Show();	
	}
	
	public void OnUsernameTextChanged(string newText)
	{
		var result = EntitiesValidator.ValidateAccountUsername(newText);
		
		if (_usernameLineEdit == null) return;
		
		ChangeThemeColor(_usernameLineEdit, result.IsValid);
	}
	
	public void OnPasswordTextChanged(string newText)
	{
		var result = EntitiesValidator.ValidateAccountPassword(newText);
		
		if (_passwordLineEdit == null) return;
		
		ChangeThemeColor(_passwordLineEdit, result.IsValid);
	}
	
	public void OnConfirmPasswordTextChanged(string newText)
	{
		var result = EntitiesValidator.ValidateAccountPassword(newText);
		
		if (_passwordLineEdit == null) return;
		
		if (_passwordLineEdit.Text != newText)
		{
			result.IsValid = false;
			result.AddError("Passwords do not match");
		}

		if (_confirmPasswordLineEdit == null) return;
		
		ChangeThemeColor(_confirmPasswordLineEdit, result.IsValid);
	}
	
	public void OnBirthdateTextChanged(string newText)
	{
		var birthDate = DateOnly.Parse(newText);
		
		var result = EntitiesValidator.ValidateAccountBirthDate(birthDate);
		
		if (_birthdateLineEdit == null) return;
		
		ChangeThemeColor(_birthdateLineEdit, result.IsValid);
	}
	
	public void OnEmailTextChanged(string newText)
	{
		var result = EntitiesValidator.ValidateAccountEmail(newText);
		
		if (_emailLineEdit == null) return;
		
		ChangeThemeColor(_emailLineEdit, result.IsValid);
	}
	
	private void ChangeThemeColor(LineEdit lineEdit, bool isValid)
	{
		if (isValid)
			lineEdit.RemoveThemeColorOverride("font_color");
		else
			lineEdit.AddThemeColorOverride("font_color", new Color(1, 0, 0));
	}
	
	private void OnHidePasswordButtonPressed()
	{
		
		if (_hideRegisterPasswordButton == null)
		{
			GD.PrintErr("Hide password button not found!");
			return;
		}
		
		_isPasswordVisible = !_isPasswordVisible;
		
		var eyeImage = _hideRegisterPasswordButton.GetChild<TextureRect>(0);

		eyeImage.Texture = _isPasswordVisible
			? GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/ShowImage.png")
			: GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/HideImage.png");
		
		if (_passwordLineEdit == null) return;
		
		_passwordLineEdit.Secret = !_isPasswordVisible;
	}
	
	private void OnHideConfirmPasswordButtonPressed()
	{
		
		if (_hideRegisterConfirmPasswordButton == null)
		{
			GD.PrintErr("Hide password button not found!");
			return;
		}
		
		_isConfirmPasswordVisible = !_isConfirmPasswordVisible;
		
		var eyeImage = _hideRegisterConfirmPasswordButton.GetChild<TextureRect>(0);

		eyeImage.Texture = _isConfirmPasswordVisible
			? GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/ShowImage.png")
			: GD.Load<Texture2D>("res://Client/Resources/Texture/MainMenu/Secret/HideImage.png");
		
		if (_confirmPasswordLineEdit == null) return;
		
		_confirmPasswordLineEdit.Secret = !_isConfirmPasswordVisible;
	}

	private void OnConfirmRegisterPressed()
	{
		if (_usernameLineEdit == null || _passwordLineEdit == null || _confirmPasswordLineEdit == null || _birthdateLineEdit == null || _emailLineEdit == null) return;
		
		var username = _usernameLineEdit.Text;
		var password = _passwordLineEdit.Text;
		var confirmPassword = _confirmPasswordLineEdit.Text;
		var birthdate = _birthdateLineEdit.Text;
		var email = _emailLineEdit.Text;
		
		var usernameResult = EntitiesValidator.ValidateAccountUsername(username);
		var passwordResult = EntitiesValidator.ValidateAccountPassword(password);
		var confirmPasswordResult = EntitiesValidator.ValidateAccountPassword(confirmPassword);
		var emailResult = EntitiesValidator.ValidateAccountEmail(email);
		var birthdateResult = EntitiesValidator.ValidateAccountBirthDate(DateOnly.Parse(birthdate));
		
		if (!usernameResult.IsValid || !passwordResult.IsValid || !confirmPasswordResult.IsValid || !emailResult.IsValid || !birthdateResult.IsValid)
		{
			ChangeThemeColor(_usernameLineEdit, usernameResult.IsValid);
			ChangeThemeColor(_passwordLineEdit, passwordResult.IsValid);
			ChangeThemeColor(_confirmPasswordLineEdit, confirmPasswordResult.IsValid);
			ChangeThemeColor(_emailLineEdit, emailResult.IsValid);
			ChangeThemeColor(_birthdateLineEdit, birthdateResult.IsValid);
			
			if (password != confirmPassword)
			{
				_confirmPasswordLineEdit.GrabFocus();
				_confirmPasswordLineEdit.Text = "";
				_confirmPasswordLineEdit.PlaceholderText = "Passwords do not match";
				return;
			}
			
			if (!usernameResult.IsValid)
				_usernameLineEdit.GrabFocus();
			else if (!passwordResult.IsValid)
				_passwordLineEdit.GrabFocus();
			else if (!confirmPasswordResult.IsValid)
				_confirmPasswordLineEdit.GrabFocus();
			else if (!emailResult.IsValid)
				_emailLineEdit.GrabFocus();
			else if (!birthdateResult.IsValid)
				_birthdateLineEdit.GrabFocus();
			
			return;
		}

		var accountDto = new AccountDto()
		{
			Username = username,
			Password = password,
			Email = email,
			BirthDate = birthdate
		};

		var registerResponse = new RegisterRequest()
		{
			Account = accountDto
		};

		_packetRequest?.SendPacket(registerResponse);
	}
}