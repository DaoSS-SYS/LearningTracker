namespace LearningTracker.Api.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string email)
    : base($"Пользователь с {email} уже существует") { }
}
