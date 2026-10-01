namespace LearningTracker.Api.Dto;

public class CreateLearningTopicRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

}
