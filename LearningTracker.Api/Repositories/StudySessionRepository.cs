using LearningTracker.Api.Data;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Repositories;

public class StudySessionRepository : IStudySessionRepository
{
    private readonly LearningTrackerDbContext _context;
    public StudySessionRepository(LearningTrackerDbContext context)
    {
        _context = context;
    }
    public async Task<List<StudySession>> GetSessionsAsync(int id)
    {
        return await _context.StudySessions.
            Where(x => x.LearningTopicId == id).
            AsNoTracking().
            OrderByDescending(x => x.StartedAtUtc).
            ToListAsync();
    }
    public void Add(StudySession studySession)
    {
        _context.StudySessions.Add(studySession);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
