using Core.Logger.Interface;

namespace Server.Extensions;

public class ConsoleLogOutput : ILogOutput
{
    public void Write(string message, params object[] args)
    {
        Console.WriteLine(message, args);
    }

    public void WriteInfo(string message, params object[] args)
    {
        Console.WriteLine(message, args);
    }

    public void WriteWarning(string message, params object[] args)
    {
        Console.WriteLine(message, args);
    }

    public void WriteError(string message, params object[] args)
    {
        Console.WriteLine(message, args);
    }
}