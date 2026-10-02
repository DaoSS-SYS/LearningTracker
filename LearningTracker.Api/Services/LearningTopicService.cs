using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services.Interfaces;

namespace LearningTracker.Api.Services;

public class LearningTopicService : ILearningTopicService
{
    private readonly ILearningTopicRepository _repository;
    public LearningTopicService(ILearningTopicRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<LearningTopic>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }
    public async Task<LearningTopic?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    
    public async Task<LearningTopic> CreateAsync(CreateLearningTopicRequest request)
    {
        var topic = new LearningTopic{Title = request.Title, CreatedAtUtc = DateTime.UtcNow, Description = request.Description, IsCompleted = false};
        _repository.Add(topic);
        await _repository.SaveChangesAsync();
        return topic;
    }
    public async Task<LearningTopic?> UpdateAsync(int id, UpdateLearningTopicRequest request)
    {
        var topic = await _repository.GetByIdAsync(id);
        if(topic is null)
            return null;

        topic.Title = request.Title;
        topic.Description = request.Description;

        await _repository.SaveChangesAsync();

        return topic;
    }
    public async Task<LearningTopic?> CompleteAsync(int id)
    {
        var topic = await _repository.GetByIdAsync(id);
        if (topic is null)
            return null;

        topic.IsCompleted = true;

        await _repository.SaveChangesAsync();

        return topic;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var topic = await _repository.GetByIdAsync(id);
        if (topic is null)
            return false;

        _repository.Remove(topic);

        await _repository.SaveChangesAsync();

        return true;
    }
}
