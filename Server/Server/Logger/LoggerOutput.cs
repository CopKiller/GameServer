using Core.Logger.Interface;

namespace Server.Logger;

public class LoggerOutput : ILogOutput
{
    public void Write(string message, params object[] args)
    {
        try
        {
            if (args.Length > 0)
            {
                Console.WriteLine(message, args);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Erro de formatação no logger: " + ex.Message);
        }
    }

    public void WriteInfo(string message, params object[] args)
    {
        Write(message, args);
    }

    public void WriteWarning(string message, params object[] args)
    {
        Write(message, args);
    }

    public void WriteError(string message, params object[] args)
    {
        Write(message, args);
    }
}