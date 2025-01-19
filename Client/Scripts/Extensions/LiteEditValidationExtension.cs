using Godot;

namespace Game.Scripts.Extensions;

public static class LiteEditValidationExtension
{
    public static void ChangeThemeColor(this LineEdit lineEdit, bool isValid)
    {
        if (isValid)
            lineEdit.RemoveThemeColorOverride("font_color");
        else
            lineEdit.AddThemeColorOverride("font_color", new Color(1, 0, 0));
    }
}