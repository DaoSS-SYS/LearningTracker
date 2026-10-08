using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;

namespace LearningTracker.Api.Services.Interfaces;

public interface ILearningTopicService
{
    Task<List<LearningTopicDto>> GetAllAsync(LearningTopicsFilters filters);
    Task<LearningTopicDto> GetByIdAsync(int id);
    Task<LearningTopicDto> CreateAsync(CreateLearningTopicRequest request);
    Task<LearningTopicDto> UpdateAsync(int id, UpdateLearningTopicRequest request);
    Task<LearningTopicDto> CompleteAsync(int id);
    Task DeleteAsync(int id);
    Task<LearningTopicStatsDto> GetStatsAsync(int id);
}
