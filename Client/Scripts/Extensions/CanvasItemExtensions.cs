using System;
using System.Threading.Tasks;
using Godot;

namespace Game.Scripts.Extensions;

/// <summary>
/// Classe de extensão para realizar fade in e fade out em um nó do tipo CanvasItem.
/// </summary>
public static class CanvasItemExtensions
{
    public static async Task FadeIn(this CanvasItem node, float duration = 1f, Tween.EaseType ease = Tween.EaseType.InOut)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.GetNodeOrNull<Tween>("Tween") != null) return;
        
        var tween = node.CreateTween();
        node.Modulate = new Color(1, 1, 1, 0);

        tween.SetEase(ease);
        tween.TweenProperty(node, "modulate:a", 1f, duration);
        await node.ToSignal(tween, Tween.SignalName.Finished);
    }
    
    public static async Task FadeOut(this CanvasItem node, float duration = 1f, Tween.EaseType ease = Tween.EaseType.InOut)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (node.GetNodeOrNull<Tween>("Tween") != null) return;
        
        var tween = node.CreateTween();
        node.Modulate = new Color(1, 1, 1, 1);

        tween.SetEase(ease);
        tween.TweenProperty(node, "modulate:a", 0f, duration);
        await node.ToSignal(tween, Tween.SignalName.Finished);
    }
}