using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Api.Dto;

public class UpdateCategoryRequest
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
