using Godot;

namespace Game.Scripts.BaseControls;

public partial class ButtonBase : Button
{
    [Export] public Window? OpenWindow { get; set; }
    [Export] public Window? CloseWindow { get; set; }
    
    public void OnPressed()
    {

        if (OpenWindow?.Visible == false)
            OpenWindow?.Show();
        else
            OpenWindow?.Hide();

        CloseWindow?.Hide();
    }
}