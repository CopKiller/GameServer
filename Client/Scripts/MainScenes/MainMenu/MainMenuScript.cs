using System;
using Game.Scripts.Extensions;
using Game.Scripts.Singletons;
using Godot;

namespace Game.Scripts.MainScenes.MainMenu;

public partial class MainMenuScript : Control
{
    public override void _Ready()
    {
        _ = this.FadeIn();
    }
    
    public void OnExitButtonPressed()
    {
        var clientManager = this.GetSingleton<ClientManager>();
        clientManager.ExitGame();
    }
}