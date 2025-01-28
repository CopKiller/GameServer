using System;
using Core.Client.Validator;
using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class CreateCharWindowScript : WindowBase
{
    private LineEdit? _nameLineEdit;

    private AlertManager? _alertManager;
    
    private int _characterSelectedSlot = -1;

    public override void _Ready()
    {
        _nameLineEdit ??= GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/NameLineEdit");

        _alertManager ??= ServiceManager.GetRequiredService<AlertManager>();
    }


    #region Signals

    private void UpdateCharacterSelectedSlotSignal(int index)
        => _characterSelectedSlot = index;

    private void CheckCharacterNameInputSignal(string text)
    {
        var result = EntitiesValidator.ValidatePlayerName(text);

        _nameLineEdit?.ChangeThemeColor(result.IsValid);
    }

    private void CheckCreateCharacterSignal()
    {
        var text = _nameLineEdit != null ? _nameLineEdit.Text : string.Empty;
        
        var result = EntitiesValidator.ValidatePlayerName(text);

        if (result.IsValid)
        {
            SendCreateChar(_characterSelectedSlot, text);
            return;
        }
        
        _alertManager?.AddGlobalAlert(string.Join(',' , result.Errors));
    }

    #endregion
}