using System;
using Game.Scripts.Extensions;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class MainMenuScript : Control
{
    
    
    public override void _Ready()
    {
        _ = this.FadeIn();
    }
}