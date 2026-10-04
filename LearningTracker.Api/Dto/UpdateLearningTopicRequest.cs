using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Api.Dto;

public class UpdateLearningTopicRequest
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Description {  get; set; }
}
