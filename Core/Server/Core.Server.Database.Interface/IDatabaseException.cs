namespace Core.Server.Database.Interface;

public interface IDatabaseException
{
    bool IsError { get; }
    string Message { get; }
    string ToString();
}