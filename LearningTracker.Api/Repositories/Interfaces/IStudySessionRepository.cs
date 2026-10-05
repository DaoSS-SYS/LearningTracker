using LearningTracker.Api.Models;

namespace LearningTracker.Api.Repositories.Interfaces;

public interface IStudySessionRepository
{
    Task<List<StudySession>> GetSessionsAsync(int id);
    void Add(StudySession studySession);
    Task SaveChangesAsync();
}
