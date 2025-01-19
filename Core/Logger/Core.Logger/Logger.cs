using Core.Logger.Interface;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class Logger(string categoryName, LogLevel minLogLevel, ILogOutput? output) : ILog
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
        var message = $" [{logLevel.ToString()}] {categoryName}: {formatter(state, exception)}";
        
        switch (logLevel)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                output?.WriteError(message);    
                return;
            case LogLevel.Warning:
                output?.WriteWarning(message);
                return;
            case LogLevel.Information:
                output?.WriteInfo(message);
                return;
            case LogLevel.Trace:
            case LogLevel.Debug:
            case LogLevel.None:
            default:
                output?.Write(message);
                break;
        }
    }
}