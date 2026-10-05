namespace LearningTracker.Api.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int LearningTopicId { get; set; }
        public LearningTopic LearningTopic { get; set; } = null!;
        public DateTime StartedAtUtc { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
}
