using Core.Network.SerializationObjects.Enum;
using Godot;

namespace Game.Scripts.Components;

public class AnimationController(AnimatedSprite2D animatedSprite)
{
    private const int StepDelay = 50;
    
    private ulong _stepTimer;
    
    public void SetStepTimer(bool state)
    {
        if (!state)
            _stepTimer = Time.GetTicksMsec() + StepDelay;
    }
    
    public void PlayAnimation(Direction direction, bool isMoving, bool isAttacking)
    {
        if (Time.GetTicksMsec() < _stepTimer) return;
        
        var animation = isAttacking ? "attack_" : (isMoving ? "move_" : "idle_");

        direction = direction switch
        {
            Direction.UpLeft => Direction.Left,
            Direction.UpRight => Direction.Right,
            Direction.DownLeft => Direction.Left,
            Direction.DownRight => Direction.Right,
            _ => direction
        };

        animation += direction switch
        {
            Direction.Up => "up",
            Direction.Down => "down",
            Direction.Left => "left",
            Direction.Right => "right",
            _ => direction.ToString().ToLower()
        };

        animatedSprite?.Play(animation);
    }
}