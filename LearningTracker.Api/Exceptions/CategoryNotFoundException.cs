namespace LearningTracker.Api.Exceptions;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException(int id)
    : base($"Категория с ID = {id} не найдена") { }
}
