using LearningTracker.Api.Dto;

namespace LearningTracker.Api.Services.Interfaces;

public interface IStudySessionService
{
    Task<List<StudySessionDto>> GetSessionsAsync(int id);
    Task<StudySessionDto> CreateAsync(CreateStudySessionRequest request, int id);
}
