using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Scripts.BaseControls;
using Game.Scripts.Loader;
using Godot;

namespace Game.Scripts.Singletons;

public partial class AlertManager : Node
{
    private readonly List<AlertMessageScript> _alerts = [];
    
    private SceneManager? _sceneManager;

    private PackedScene? _alertMessageScene;

    public override void _Ready()
    {
        _sceneManager = ServiceManager.GetRequiredService<SceneManager>();
        
        _alertMessageScene = _sceneManager.LoadScenePacked<AlertMessageScript>();
    }

    public void AddGlobalAlert(string text)
    {
        CallDeferred(MethodName.InstantiateAlert, text);
    }
    
    // public void AddLocalAlert(string text)
    // {
    //     InstantiateAlert(text);
    // }
    
    private void InstantiateAlert(string text)
    {
        if (_alertMessageScene == null)
        {
            GD.PrintErr("Alert message scene not found!");
            return;
        }
        
        var alert = _alerts
            .FirstOrDefault(alertMsg => 
                alertMsg.GetText() == text) 
                    ?? _alertMessageScene.Instantiate<AlertMessageScript>();

        if (alert == null)
        {
            GD.PrintErr("Failed to instantiate alert message!");
            return;
        }
        
        alert.SetText(text);

        if (alert.IsInsideTree())
        {
            alert.Show();
            alert.PopupCentered();
            return;
        }

        ConnectAlertSignals(alert);
        
        AddChild(alert);
    }
    
    private void ConnectAlertSignals(AlertMessageScript alert)
    {
        alert.Connect(Node.SignalName.Ready, Callable.From(() =>
        {
            _alerts.Add(alert);
        }));
        
        alert.Connect(AlertMessageScript.SignalName.ClosePressed, Callable.From(() =>
        {
            _alerts.Remove(alert);
            alert.QueueFree();
        }));
    }
}