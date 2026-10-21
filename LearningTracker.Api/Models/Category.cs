namespace LearningTracker.Api.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public List<LearningTopic> LearningTopics { get; set; } = new();
}
