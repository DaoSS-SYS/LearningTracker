using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Api.Dto;

public class CreateStudySessionRequest
{
    [StringLength(100)]
    public string? Notes { get; set; }

    [Range(1, 1440)]
    public int DurationMinutes { get; set; }
}
