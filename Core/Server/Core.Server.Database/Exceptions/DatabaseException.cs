using Core.Database.Interfaces;
using Core.Server.Database.Interface;

namespace Core.Server.Database.Exceptions;

public class DatabaseException(bool isError, string message) : IDatabaseException
{
    public bool IsError => isError;
    public string Message => message;

    public override string ToString() => message;
}