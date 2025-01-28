using Godot;

namespace Game.Scripts.Player;

public partial class Player : CharacterBody2D
{
    private float _gridSnapped = 32;

    private Vector2 _targetPosition = Vector2I.Zero;

    private float _speed = 2f;

    private bool _isMoving;

    public override void _Ready()
    {
        _targetPosition = Position;
    }

    public override void _PhysicsProcess(double delta)
    {
        CheckInputMovement();
        
        ProcessMovement(delta);
    }
    
    private void CheckInputMovement()
    {
        if (_isMoving) return;
        
        var direction = Input.GetVector("ui_left","ui_right","ui_up","ui_down");
        
        if (direction == Vector2.Zero) return;
        
        _targetPosition = direction * _gridSnapped;
        
        _isMoving = true;
    }
    
    private void ProcessMovement(double delta)
    {
        if (!_isMoving) return;

        var tween = CreateTween();
        tween.TweenProperty(this, "position", _targetPosition, 0.5);
        tween.TweenCallback(Callable.From(() =>
        {
            Position = _targetPosition;
            _isMoving = false;
        }));
        tween.Play();
    }
}