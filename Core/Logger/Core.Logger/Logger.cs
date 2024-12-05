using Core.Logger.Interface;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class Logger(string categoryName, LogLevel minLogLevel) : ILog
{
    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= minLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        Console.WriteLine($"[{logLevel}] {categoryName}: {message}");
    }
}