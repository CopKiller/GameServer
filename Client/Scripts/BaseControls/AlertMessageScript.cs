using Game.Scripts.Extensions.Attributes;
using Godot;

namespace Game.Scripts.BaseControls;

[ScenePath("res://Client/Scenes/BaseControls/AlertMessageWindow.tscn")]
public partial class AlertMessageScript : AcceptDialog
{
    [Signal]
    public delegate void OkPressedEventHandler();
    
    [Signal]
    public delegate void ClosePressedEventHandler();
    
    private Label? _label;
    private Button? _okButton;
    private string _textOnly = string.Empty;
    
    public override void _Ready()
    {
        _label = GetLabel();
        _okButton = GetOkButton();
    }
    
    private void OnClosePressed()
    {
        EmitSignal(SignalName.ClosePressed);
        this.Hide();
    }
    
    private void OnOkPressed()
    {
        EmitSignal(SignalName.OkPressed);
        EmitSignal(SignalName.ClosePressed);
        this.Hide();
    }
}