
using Core.Logger.Interface;
using Godot;

namespace Game.Scripts.Singletons;

public class GodotLogOutput : ILogOutput
{
    public void Write(string message, params object[] args)
    {
        GD.Print(message, args);
    }

    public void WriteInfo(string message, params object[] args)
    {
        GD.Print(message, args);
    }

    public void WriteWarning(string message, params object[] args)
    {
        GD.Print(message, args);
    }

    public void WriteError(string message, params object[] args)
    {
        GD.PrintErr(message, args);
    }
}