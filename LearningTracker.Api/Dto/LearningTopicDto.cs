using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Api.Dto;

public class LearningTopicDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
