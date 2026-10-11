using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;

namespace LearningTracker.Tests;

public class FakeLearningTopicRepository : ILearningTopicRepository
{
    private readonly List<LearningTopic> _topics = new List<LearningTopic>(); 
    public IReadOnlyList<LearningTopic> Topics { get { return _topics; } }
    public async Task<(List<LearningTopic> Items, int TotalCount)> GetAllAsync(LearningTopicsFilters filters)
    {
        throw new NotImplementedException();
    }

    public Task<LearningTopic?> GetByIdAsync(int id)
    {
        return Task.FromResult(_topics.FirstOrDefault(x => x.Id == id));
    }

    public void Add(LearningTopic topic)
    {
        _topics.Add(topic);
    }

    public void Remove(LearningTopic topic)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<LearningTopicStatsDto?> GetTopicStatsAsync(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<LearningTopic?> GetTopicDetailsAsync(int id)
    {
        throw new NotImplementedException();
    }
}
