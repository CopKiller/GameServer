using System;
using Core.Network.SerializationObjects.Enum;
using Godot;

namespace Game.Scripts.Components;

public class InputHandler(MovementController movement, AttackController attack)
{
    public void ProcessInput()
    {
        ProcessInputActions();
    }

    private void ProcessInputActions()
    {
        var header = "action_";

        if (Input.IsActionJustPressed(header + "attack"))
        {
            attack.Attack();
            return;
        }
        
        header += "move_";

        bool up = Input.IsActionPressed(header + "up");
        bool down = Input.IsActionPressed(header + "down");
        bool left = Input.IsActionPressed(header + "left");
        bool right = Input.IsActionPressed(header + "right");

        // Se pressionar direções opostas, ignora a entrada
        if ((up && down) || (left && right))
            return;

        if (up)
        {
            if (left)
                movement.MoveToDirection(Direction.UpLeft);
            else if (right)
                movement.MoveToDirection(Direction.UpRight);
            else
                movement.MoveToDirection(Direction.Up);
        }
        else if (down)
        {
            if (left)
                movement.MoveToDirection(Direction.DownLeft);
            else if (right)
                movement.MoveToDirection(Direction.DownRight);
            else
                movement.MoveToDirection(Direction.Down);
        }
        else if (left)
            movement.MoveToDirection(Direction.Left);
        else if (right)
            movement.MoveToDirection(Direction.Right);
    }
}