using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Services.Interfaces;

public interface ILearningTopicService
{
    Task<List<LearningTopic>> GetAllAsync();
    Task<LearningTopic> GetByIdAsync(int id);
    Task<LearningTopic> CreateAsync(CreateLearningTopicRequest request);
    Task<LearningTopic> UpdateAsync(int id, UpdateLearningTopicRequest request);
    Task<LearningTopic> CompleteAsync(int id);
    Task DeleteAsync(int id);
}
