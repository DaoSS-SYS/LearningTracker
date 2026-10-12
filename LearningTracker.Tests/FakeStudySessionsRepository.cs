using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;

namespace LearningTracker.Tests;

public class FakeStudySessionsRepository : IStudySessionRepository
{
    private readonly List<StudySession> _sessions = new List<StudySession>();
    public IReadOnlyList<StudySession> Sessions { get { return _sessions; } }
    
    public async Task<List<StudySession>> GetSessionsAsync(int id)
    {
        throw new NotImplementedException();
    }
    public void Add(StudySession studySession)
    {
        _sessions.Add(studySession);
    }
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
    public Task<bool> HasSessionsAsync(int id)
    {
        return Task.FromResult(_sessions.Any(x => x.LearningTopicId == id));
    }
}
