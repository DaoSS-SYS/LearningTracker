using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Data;
using Microsoft.EntityFrameworkCore;
using LearningTracker.Api.Mapping;
using LearningTracker.Api.Dto;

namespace LearningTracker.Api.Repositories;

public class LearningTopicRepository : ILearningTopicRepository
{
    private readonly LearningTrackerDbContext _context;
    public LearningTopicRepository(LearningTrackerDbContext context)
    {
        _context = context;
    }
    public async Task<List<LearningTopic>> GetAllAsync()
    {
        return await _context.LearningTopics.
            AsNoTracking().
            OrderByDescending(x => x.CreatedAtUtc).
            ToListAsync();
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
