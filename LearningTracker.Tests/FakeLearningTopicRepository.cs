using LearningTracker.Api.Data;
using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningTracker.Tests;

public class FakeLearningTopicRepository : ILearningTopicRepository
{
    private readonly List<LearningTopic> _topics = new List<LearningTopic>(); 
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
        throw new NotImplementedException();
    }

    public void Remove(LearningTopic topic)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
    public async Task<bool> ExistAsync(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<LearningTopicStatsDto?> GetTopicStatsAsync(int id)
    {
        throw new NotImplementedException();
    }
}
