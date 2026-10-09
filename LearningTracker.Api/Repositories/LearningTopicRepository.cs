using LearningTracker.Api.Data;
using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Repositories;

public class LearningTopicRepository : ILearningTopicRepository
{
    private readonly LearningTrackerDbContext _context;
    public LearningTopicRepository(LearningTrackerDbContext context)
    {
        _context = context;
    }
    public async Task<(List<LearningTopic> Items, int TotalCount)> GetAllAsync(LearningTopicsFilters filters)
    {
        IQueryable<LearningTopic> topics = _context.LearningTopics.
            AsNoTracking();

        if(filters.IsCompleted.HasValue)
        {
            topics = topics.
            Where(s => s.IsCompleted == filters.IsCompleted.Value);
        }

        if(!string.IsNullOrWhiteSpace(filters.Search))
        {
            topics = topics.Where(x => EF.Functions.ILike(x.Title, $"%{filters.Search}%"));
        }

        var total = await topics.CountAsync();

        var finalTopics = await topics.
            OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id).
            Skip((filters.Page - 1) * filters.PageSize).
            Take(filters.PageSize).
            ToListAsync();


        return (finalTopics, total);

    }

    public async Task<LearningTopic?> GetByIdAsync(int id)
    {
        return await _context.LearningTopics.FindAsync(id);
    } 

    public void Add(LearningTopic topic)
    {
        _context.LearningTopics.Add(topic);
    }

    public void Remove(LearningTopic topic)
    {
        _context.Remove(topic);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistAsync(int id)
    {
        return await _context.LearningTopics.AnyAsync(x => x.Id == id);
    }
    public async Task<LearningTopicStatsDto?> GetTopicStatsAsync(int id)
    {
        return await _context.LearningTopics.
            Where(x => x.Id == id).
            Select(s => new LearningTopicStatsDto { 
                Id = s.Id, 
                SessionsCount = s.StudySessions.Count,
                Title = s.Title,
                TotalMinutes = s.StudySessions.Sum(z => z.DurationMinutes)}).
            FirstOrDefaultAsync();
    }

}
