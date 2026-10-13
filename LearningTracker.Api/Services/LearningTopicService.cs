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
    private readonly IStudySessionRepository _repositorySessions;
    private readonly ILogger<LearningTopicService> _logger;


    public LearningTopicService(ILearningTopicRepository repository, IStudySessionRepository repositorySessions, ILogger<LearningTopicService> logger)
    {
        _repository = repository;
        _repositorySessions = repositorySessions;
        _logger = logger;
    }
    public async Task<PagedResult<LearningTopicDto>> GetAllAsync(LearningTopicsFilters filters)
    {
        var result = await _repository.GetAllAsync(filters);
        var topics = result.Items.Select(x => x.ToDto());
        var pagedResult = new PagedResult<LearningTopicDto>
        {
            Page = filters.Page,
            PageSize = filters.PageSize,
            TotalCount = result.TotalCount,
            Items = topics.ToList()
        };

        return pagedResult;
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
        _logger.LogInformation("Topic {TopicId} created", topic.Id);
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

        if(await _repositorySessions.HasSessionsAsync(id))
        {
            _logger.LogWarning("Topic {TopicId} cannot be deleted: it has sessions", topic.Id);
            throw new TopicHasSessionsException(id);
        }
            
        _repository.Remove(topic);

        await _repository.SaveChangesAsync();

        _logger.LogInformation("Topic {TopicId} was deleted", topic.Id);
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
    public async Task<LearningTopicDetailsDto> GetDetailsAsync(int id)
    {
        var topic = await _repository.GetTopicDetailsAsync(id);

        if(topic is null)
            throw new TopicNotFoundException(id);

        return topic.ToDetailsDto();
    }

}
