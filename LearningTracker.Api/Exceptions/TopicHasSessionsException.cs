namespace LearningTracker.Api.Exceptions;

public class TopicHasSessionsException : Exception
{
    public TopicHasSessionsException(int id)
    : base($"Тема с ID = {id} содержит сессии, удаление невозможно") { }
}
