using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Mapping;

public static class LearningTopicDetailsMapping
{
    public static LearningTopicDetailsDto ToDetailsDto(this LearningTopic learningTopic)
    {
        return new LearningTopicDetailsDto
        {
            Id = learningTopic.Id,
            Title = learningTopic.Title,
            Sessions = learningTopic.StudySessions.Select(x => x.ToDto()).ToList()
        };
    }
}
