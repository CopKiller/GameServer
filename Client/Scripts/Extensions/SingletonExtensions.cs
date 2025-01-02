using Godot;

namespace Game.Scripts.Extensions;

public static class SingletonExtensions
{
    public static T GetSingleton<T>(this Node node) where T : Node
    {
        return node.GetNode<T>("/root/" + typeof(T).Name);
    }
}