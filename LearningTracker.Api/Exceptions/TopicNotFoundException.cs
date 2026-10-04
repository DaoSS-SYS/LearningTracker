namespace LearningTracker.Api.Exceptions;

public class TopicNotFoundException : Exception
{
    public TopicNotFoundException(int id)
    : base($"Тема с ID = {id} не найдена") { }
}
