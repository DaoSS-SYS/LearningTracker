using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Repositories.Interfaces;

public interface ILearningTopicRepository
{
    Task<List<LearningTopic>> GetAllAsync();
    Task<LearningTopic?> GetByIdAsync(int id);
    void Add(LearningTopic topic);
    void Remove(LearningTopic topic);
    Task SaveChangesAsync();
    Task<bool> ExistAsync(int id);
    Task<LearningTopicStatsDto?> GetTopicStatsAsync(int id);
}
