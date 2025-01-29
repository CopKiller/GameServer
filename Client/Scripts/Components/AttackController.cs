using System;
using Godot;

namespace Game.Scripts.Components;

public class AttackController(CharacterBody2D body)
{
    public event Action<bool>? AttackStateChanged;
    
    private const float AttackDuration = 0.5f;
    
    public  bool IsAttacking => _isAttacking;
    private bool _isAttacking;
    
    public void Attack()
    {
        if (_isAttacking) return;

        _isAttacking = true;

        var tween = body.CreateTween();
        tween.TweenProperty(body, "scale", new Vector2(1f, 1f), AttackDuration);
        tween.TweenCallback(Callable.From(() =>
        {
            _isAttacking = false;
            AttackStateChanged?.Invoke(false);
        }));
        tween.Play();
    }
}