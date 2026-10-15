namespace LearningTracker.Api.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
    : base($"Неверные данные") { }
}
