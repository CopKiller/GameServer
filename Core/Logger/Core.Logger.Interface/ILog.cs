using Microsoft.Extensions.Logging;

namespace Core.Logger.Interface;

public interface ILog : ILogger
{
    /*void Print(string message, params object[] args);
    void PrintInfo(string message, params object[] args);
    void PrintWarning(string message, params object[] args);
    void PrintError(string message, params object[] args);*/
}

public interface ILog<out T> : ILogger<T>
{
    /*void Print(string message, params object[] args);
    void PrintInfo(string message, params object[] args);
    void PrintWarning(string message, params object[] args);
    void PrintError(string message, params object[] args);*/
}