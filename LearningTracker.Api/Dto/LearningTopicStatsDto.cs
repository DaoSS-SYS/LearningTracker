namespace LearningTracker.Api.Dto;

public class LearningTopicStatsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int SessionsCount { get; set; }
    public int TotalMinutes { get; set; }
}
