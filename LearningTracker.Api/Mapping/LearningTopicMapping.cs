using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Mapping;

public static class LearningTopicMapping
{
    public static LearningTopicDto ToDto(this LearningTopic topic)
    {
        return new LearningTopicDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Description = topic.Description,
            CreatedAtUtc = topic.CreatedAtUtc,
            IsCompleted = topic.IsCompleted
        };
    }
}
