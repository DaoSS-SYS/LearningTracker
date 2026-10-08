using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Mapping;
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
    public async Task<List<LearningTopicDto>> GetAllAsync(LearningTopicsFilters filters)
    {
        var topics = await _repository.GetAllAsync(filters);
        return topics.Select(x => x.ToDto()).ToList();
    }
    public async Task<LearningTopicDto> GetByIdAsync(int id)
    {
        var topic = await GetTopicOrThrowAsync(id);
        return topic.ToDto();
    }

    public async Task<LearningTopicDto> CreateAsync(CreateLearningTopicRequest request)
    {
        var topic = new LearningTopic{Title = request.Title, CreatedAtUtc = DateTime.UtcNow, Description = request.Description, IsCompleted = false};
        _repository.Add(topic);
        await _repository.SaveChangesAsync();
        return topic.ToDto();
    }
    public async Task<LearningTopicDto> UpdateAsync(int id, UpdateLearningTopicRequest request)
    {
        var topic = await GetTopicOrThrowAsync(id);

        topic.Title = request.Title;
        topic.Description = request.Description;

        await _repository.SaveChangesAsync();

        return topic.ToDto();
    }
    public async Task<LearningTopicDto> CompleteAsync(int id)
    {
        var topic = await GetTopicOrThrowAsync(id);

        topic.IsCompleted = true;

        await _repository.SaveChangesAsync();

        return topic.ToDto();
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
    public async Task<LearningTopicStatsDto> GetStatsAsync(int id)
    {
        var topic = await _repository.GetTopicStatsAsync(id);

        if (topic is null)
            throw new TopicNotFoundException(id);

        return topic;
    }
}
