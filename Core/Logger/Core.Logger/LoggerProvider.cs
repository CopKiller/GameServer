using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class CustomLoggerProvider(LogLevel minLogLevel) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName, minLogLevel);
    }

    public void Dispose()
    {
        // Libera recursos
    }
}