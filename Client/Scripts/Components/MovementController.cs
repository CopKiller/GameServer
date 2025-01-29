using System;
using Core.Network.SerializationObjects.Enum;
using Godot;

namespace Game.Scripts.Components;

public class MovementController(CharacterBody2D character, float gridSnapSize)
{
    public event Action<bool>? MovementStateChanged;
    
    private const float MovementDuration = 0.5f;
    
    private bool _isMoving;
    private Vector2 _targetPosition;

    public bool IsMoving => _isMoving;
    
    public Direction Direction => GetDirection(_targetPosition);
    
    private Direction _lastDirection;

    public void MoveToDirection(Direction direction)
    {
        if (_isMoving) return;
        
        _lastDirection = direction;

        _targetPosition = direction switch
        {
            Direction.Up => new Vector2(character.Position.X, character.Position.Y - gridSnapSize),
            Direction.Down => new Vector2(character.Position.X, character.Position.Y + gridSnapSize),
            Direction.Left => new Vector2(character.Position.X - gridSnapSize, character.Position.Y),
            Direction.Right => new Vector2(character.Position.X + gridSnapSize, character.Position.Y),
            Direction.UpRight => new Vector2(character.Position.X + gridSnapSize, character.Position.Y - gridSnapSize),
            Direction.UpLeft => new Vector2(character.Position.X - gridSnapSize, character.Position.Y - gridSnapSize),
            Direction.DownRight => new Vector2(character.Position.X + gridSnapSize, character.Position.Y + gridSnapSize),
            Direction.DownLeft => new Vector2(character.Position.X - gridSnapSize, character.Position.Y + gridSnapSize),
            _ => _targetPosition
        };

        StartMovement();
    }
    
    public void MoveToPosition(Vector2 position)
    {
        if (_isMoving) return;
        
        position *= gridSnapSize;

        _targetPosition = position;
        
        _lastDirection = GetDirection(position);

        StartMovement();
    }
    
    private Direction GetDirection(Vector2 position)
    {
        return position switch
        {
            { X: var x, Y: var y} when x < character.Position.X && y < character.Position.Y => Direction.UpLeft,
            { X: var x, Y: var y} when x < character.Position.X && y > character.Position.Y => Direction.DownLeft,
            { X: var x, Y: var y} when x > character.Position.X && y < character.Position.Y => Direction.UpRight,
            { X: var x, Y: var y} when x > character.Position.X && y > character.Position.Y => Direction.DownRight,
            { X: var x, Y: var y } when x < character.Position.X => Direction.Left,
            { X: var x, Y: var y } when x > character.Position.X => Direction.Right,
            { X: var x, Y: var y } when y < character.Position.Y => Direction.Up,
            { X: var x, Y: var y } when y > character.Position.Y => Direction.Down,
            _ => _lastDirection
        };
    }

    private void StartMovement()
    {
        _isMoving = true;

        var tween = character.CreateTween();
        tween.TweenProperty(character, "position", _targetPosition, MovementDuration);
        tween.TweenCallback(Callable.From(() =>
        {
            MovementStateChanged?.Invoke(false);
            return _isMoving = false;
        }));
        tween.Play();
    }
}