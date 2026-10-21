namespace LearningTracker.Api.Exceptions;

public class CategoryNameIsBusyException : Exception
{
    public CategoryNameIsBusyException(string name)
    : base($"Категория с названием = {name} уже есть") { }
}
