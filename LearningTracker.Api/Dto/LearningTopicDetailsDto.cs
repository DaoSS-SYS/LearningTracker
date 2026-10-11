namespace LearningTracker.Api.Dto;

public class LearningTopicDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<StudySessionDto> Sessions { get; set; } = new();
}
