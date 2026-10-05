using LearningTracker.Api.Models;

namespace LearningTracker.Api.Dto;

public class StudySessionDto
{
    public int Id { get; set; }
    public int LearningTopicId { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public int DurationMinutes { get; set; }
    public string? Notes { get; set; }
}
