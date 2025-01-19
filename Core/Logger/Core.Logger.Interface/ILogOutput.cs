namespace Core.Logger.Interface;

public interface ILogOutput
{
    void Write(string message, params object[] args);
    void WriteInfo(string message, params object[] args);
    void WriteWarning(string message, params object[] args);
    void WriteError(string message, params object[] args);
}