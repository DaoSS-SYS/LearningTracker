namespace LearningTracker.Api.Exceptions;

public class CategoryHasTopicsException : Exception
{
    public CategoryHasTopicsException(int id)
    : base($"Категория с ID = {id} содержит темы и не может быть удалена") { }
}
