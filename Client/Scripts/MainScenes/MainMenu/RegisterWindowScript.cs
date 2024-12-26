using System;
using Core.Client.Validator;
using Game.Scripts.BaseControls;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class RegisterWindowScript : WindowBase
{
	[Export] public DatePicker? DatePickWindow;
	
	private LineEdit? _usernameLineEdit;
	private LineEdit? _passwordLineEdit;
	private LineEdit? _confirmPasswordLineEdit;
	private LineEdit? _birthdateLineEdit;
	private LineEdit? _emailLineEdit;
	public override void _Ready()
	{
		_usernameLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/UsernameLineEdit");
		_passwordLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/PasswordLineEdit");
		_confirmPasswordLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/ConfirmPasswordLineEdit");
		_birthdateLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/BirthdateLineEdit");
		_emailLineEdit = GetNode<LineEdit>("MarginContainer/VBoxContainer/EmailLineEdit");
		
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
	
}