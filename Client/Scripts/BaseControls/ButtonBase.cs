using Godot;

namespace Game.Scripts.BaseControls;

public partial class ButtonBase : Button
{
    [Export] public Window? OpenWindow { get; set; }
    [Export] public Window? CloseWindow { get; set; }
    
    public virtual void OnPressed()
    {
        if (OpenWindow != null)
        {
            OpenWindow.Visible = !OpenWindow.Visible;
            
            if (CloseWindow != null)
                CloseWindow.Hide();
        }
    }
}