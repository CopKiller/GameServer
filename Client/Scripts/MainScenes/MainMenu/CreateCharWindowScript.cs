using Core.Client.Validator;
using Game.Scripts.BaseControls;
using Game.Scripts.Extensions;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class CreateCharWindowScript : WindowBase
{
    private LineEdit? _nameLineEdit;
    
    private int _characterSelectedSlot = -1;

    public override void _Ready()
    {
        _nameLineEdit ??= GetNode<LineEdit>("MarginContainer/VBoxContainer/HBoxContainer/NameLineEdit");
    }


    #region Signals

    private void UpdateCharacterSelectedSlotSignal(int index)
        => _characterSelectedSlot = index;

    private void CheckCharacterNameInputSignal()
    {
        var text = _nameLineEdit != null ? _nameLineEdit.Text : string.Empty;
        
        var result = EntitiesValidator.ValidatePlayerName(text);

        _nameLineEdit?.ChangeThemeColor(result.IsValid);
    }

    private void CheckCreateCharacterSignal()
    {
        var text = _nameLineEdit != null ? _nameLineEdit.Text : string.Empty;
        
        var result = EntitiesValidator.ValidatePlayerName(text);

        if (result.IsValid)
        {
            SendCreateChar();
        }
    }

    #endregion
}