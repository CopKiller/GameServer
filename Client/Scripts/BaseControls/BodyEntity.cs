using Core.Network.SerializationObjects.Enum;
using Godot;

namespace Game.Scripts.BaseControls;

[Tool]
public partial class BodyEntity : CharacterBody2D
{
    [Export] private float _gridSnapped = 32;

    [Export] private float _speed = 2f;

    [Export] private Direction Direction
    {
        get => _direction;
        set => ProcessDirection(value);
    }
    
    [Export] private bool Attack
    {
        get => _isAttacking;
        set => ProcessAttack(value);
    }

    [Export] private AnimatedSprite2D? _animatedSprite;

    private Vector2 _targetPosition;
    private Direction _direction;
    private bool _isMoving;
    private bool _isAttacking;
    private ulong _stepTimer;

    public override void _Ready()
    {
        _targetPosition = Position;
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        if (Time.GetTicksMsec() < _stepTimer)
            return;

        var header = _isMoving ? "move_" : "idle_";
        
        if (_isAttacking)
            header = "attack_";

        header += _direction switch
        {
            Direction.DownRight or Direction.UpRight => "right",
            Direction.DownLeft or Direction.UpLeft => "left",
            _ => _direction.ToString().ToLower()
        };

        _animatedSprite?.Play(header);
    }

    # region Input

    public override void _Input(InputEvent @event)
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        ProcessInputAction();
    }

    private void ProcessInputAction()
    {
        const string actionHeader = "action_";

        ProcessInputMovement(actionHeader + "move_");

        ProcessInputAttack(actionHeader + "attack");
    }

    private void ProcessInputMovement(string header)
    {
        bool up = Input.IsActionPressed(header + "up");
        bool down = Input.IsActionPressed(header + "down");
        bool left = Input.IsActionPressed(header + "left");
        bool right = Input.IsActionPressed(header + "right");

        if (up && right)
            Direction = Direction.UpRight;
        else if (up && left)
            Direction = Direction.UpLeft;
        else if (down && right)
            Direction = Direction.DownRight;
        else if (down && left)
            Direction = Direction.DownLeft;
        else if (up)
            Direction = Direction.Up;
        else if (down)
            Direction = Direction.Down;
        else if (left)
            Direction = Direction.Left;
        else if (right)
            Direction = Direction.Right;
    }

    private void ProcessInputAttack(string header)
    {
        if (Input.IsActionPressed(header))
            Attack = true;
    }

    private void ProcessDirection(Direction direction)
    {
        if (_isMoving) return;

        _isMoving = true;

        _direction = direction;

        _targetPosition = direction switch
        {
            Direction.Up => new Vector2(Position.X, Position.Y - _gridSnapped),
            Direction.Down => new Vector2(Position.X, Position.Y + _gridSnapped),
            Direction.Left => new Vector2(Position.X - _gridSnapped, Position.Y),
            Direction.Right => new Vector2(Position.X + _gridSnapped, Position.Y),
            Direction.UpRight => new Vector2(Position.X + _gridSnapped, Position.Y - _gridSnapped),
            Direction.UpLeft => new Vector2(Position.X - _gridSnapped, Position.Y - _gridSnapped),
            Direction.DownRight => new Vector2(Position.X + _gridSnapped, Position.Y + _gridSnapped),
            Direction.DownLeft => new Vector2(Position.X - _gridSnapped, Position.Y + _gridSnapped),
            _ => _targetPosition
        };

        ProcessMovement();
    }

    private void ProcessMovement()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "position", _targetPosition, 0.5);
        tween.TweenCallback(Callable.From(() =>
        {
            Position = _targetPosition;
            _isMoving = false;
            _stepTimer = Time.GetTicksMsec() + 50;
        }));
        tween.Play();
    }
    
    private void ProcessAttack(bool isAttacking)
    {
        if (_isAttacking) return;

        _isAttacking = isAttacking;

        if (isAttacking)
        {
            var tween = CreateTween();
            tween.TweenProperty(this, "scale", new Vector2(1f, 1f), 0.5);
            tween.TweenCallback(Callable.From(() =>
            {
                _isAttacking = false;
                _stepTimer = Time.GetTicksMsec() + 50;
            }));
            tween.Play();
        }
    }

    # endregion
}