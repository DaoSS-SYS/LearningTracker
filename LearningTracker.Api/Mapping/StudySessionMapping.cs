using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Mapping;

public static class StudySessionMapping
{
    public static StudySessionDto ToDto(this StudySession studySession)
    {
        return new StudySessionDto
        {
            Id = studySession.Id,
            LearningTopicId = studySession.LearningTopicId,
            StartedAtUtc = studySession.StartedAtUtc,
            Notes = studySession.Notes,
            DurationMinutes = studySession.DurationMinutes,
        };
    }
}
