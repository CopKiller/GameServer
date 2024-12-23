using Core.Logger.Interface;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class CustomLoggerProvider(ILogOutput logOutput, LogLevel minLogLevel) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new Logger(categoryName, minLogLevel, logOutput);
    }
    
    public void Dispose()
    {
        
    }
}