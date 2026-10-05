using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Mapping;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services.Interfaces;

namespace LearningTracker.Api.Services;

public class StudySessionService : IStudySessionService
{
    private readonly IStudySessionRepository _studySessionRepository;
    private readonly ILearningTopicRepository _learningTopicRepository; 
    public StudySessionService(IStudySessionRepository studySessionRepository, ILearningTopicRepository learningTopicRepository)
    {
        _learningTopicRepository = learningTopicRepository;
        _studySessionRepository = studySessionRepository;
    }
    public async Task<List<StudySessionDto>> GetSessionsAsync(int id)
    {
        if (!await _learningTopicRepository.ExistAsync(id))
            throw new TopicNotFoundException(id);

        var sessions = await _studySessionRepository.GetSessionsAsync(id);
        return sessions.Select(x => x.ToDto()).ToList();
    }
    public async Task<StudySessionDto> CreateAsync(CreateStudySessionRequest request, int id)
    {
        if (! await _learningTopicRepository.ExistAsync(id))
            throw new TopicNotFoundException(id);
        
        var session = new StudySession
        {
            Notes = request.Notes,
            LearningTopicId = id,
            DurationMinutes = request.DurationMinutes
        };

        _studySessionRepository.Add(session);

        await _studySessionRepository.SaveChangesAsync();

        return session.ToDto();
    }
}
