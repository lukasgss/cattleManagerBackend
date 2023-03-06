namespace CattleManager.Application.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }

    public UnauthorizedException() : base() { }

    public UnauthorizedException(string? message, Exception? innerException) : base(message, innerException) { }
}