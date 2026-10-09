using System.ComponentModel.DataAnnotations;

namespace LearningTracker.Api.Filters;

public class LearningTopicsFilters
{
    public bool? IsCompleted { get; set; }
    public string? Search { get; set; }

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}
