using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

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
}
