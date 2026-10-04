using LearningTracker.Api.Controllers;
using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
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
    public async Task<LearningTopic> GetByIdAsync(int id)
    {
        var topic = await GetTopicOrThrowAsync(id);
        return topic;
    }

    public async Task<LearningTopic> CreateAsync(CreateLearningTopicRequest request)
    {
        var topic = new LearningTopic{Title = request.Title, CreatedAtUtc = DateTime.UtcNow, Description = request.Description, IsCompleted = false};
        _repository.Add(topic);
        await _repository.SaveChangesAsync();
        return topic;
    }
    public async Task<LearningTopic> UpdateAsync(int id, UpdateLearningTopicRequest request)
    {
        var topic = await GetTopicOrThrowAsync(id);

        topic.Title = request.Title;
        topic.Description = request.Description;

        await _repository.SaveChangesAsync();

        return topic;
    }
    public async Task<LearningTopic> CompleteAsync(int id)
    {
        var topic = await GetTopicOrThrowAsync(id);

        topic.IsCompleted = true;

        await _repository.SaveChangesAsync();

        return topic;
    }
    public async Task DeleteAsync(int id)
    {
        var topic = await GetTopicOrThrowAsync(id);

        _repository.Remove(topic);

        await _repository.SaveChangesAsync();
    }
    private async Task<LearningTopic> GetTopicOrThrowAsync(int id)
    {
        var topic = await _repository.GetByIdAsync(id);
        if(topic is null)
            throw new TopicNotFoundException(id);

        return topic;
    }

}
