using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Repositories.Interfaces;

public interface ILearningTopicRepository
{
    Task<(List<LearningTopic> Items, int TotalCount)> GetAllAsync(LearningTopicsFilters filters); 
    Task<LearningTopic?> GetByIdAsync(int id);
    void Add(LearningTopic topic);
    void Remove(LearningTopic topic);
    Task SaveChangesAsync();
    Task<bool> ExistAsync(int id);
    Task<LearningTopicStatsDto?> GetTopicStatsAsync(int id);
}
