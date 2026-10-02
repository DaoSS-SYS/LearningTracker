namespace LearningTracker.Api.Dto;

public class UpdateLearningTopicRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description {  get; set; }
}
