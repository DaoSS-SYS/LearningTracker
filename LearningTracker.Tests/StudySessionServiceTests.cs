using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services;

namespace LearningTracker.Tests;

public class StudySessionServiceTests
{
    [Fact]
    public async Task CreateAsync_TopicNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var fakeTopicRepository = new FakeLearningTopicRepository();
        var fakeSessionRepository = new FakeStudySessionsRepository();
        var service = new StudySessionService(fakeSessionRepository, fakeTopicRepository);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        await Assert.ThrowsAsync<TopicNotFoundException>(() => service.CreateAsync(new CreateStudySessionRequest { DurationMinutes = 1 }, 1));

        Assert.Empty(fakeSessionRepository.Sessions);
    }

    [Fact]
    public async Task CreateAsync_TopicExists_CreateSession()
    {
        // Arrange
        var fakeTopicRepository = new FakeLearningTopicRepository();
        var fakeSessionRepository = new FakeStudySessionsRepository();
        var service = new StudySessionService(fakeSessionRepository, fakeTopicRepository);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        fakeTopicRepository.Add(new LearningTopic { Id = 1, Title = "test" });

        await service.CreateAsync(new CreateStudySessionRequest { DurationMinutes = 1 }, 1);

        var session = Assert.Single(fakeSessionRepository.Sessions);
        Assert.Equal(1, session.DurationMinutes);
        Assert.Equal(1, session.LearningTopicId);
    }

    [Fact]
    public async Task GetSessionsAsync_TopicNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var fakeTopicRepository = new FakeLearningTopicRepository();
        var fakeSessionRepository = new FakeStudySessionsRepository();
        var service = new StudySessionService(fakeSessionRepository, fakeTopicRepository);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        await Assert.ThrowsAsync<TopicNotFoundException>(() => service.GetSessionsAsync(1));
    }

    [Fact]
    public async Task GetSessionsAsync_TopicExists_GetSessions()
    {
        // Arrange
        var fakeTopicRepository = new FakeLearningTopicRepository();
        var fakeSessionRepository = new FakeStudySessionsRepository();
        var service = new StudySessionService(fakeSessionRepository, fakeTopicRepository);

        // Act + Assert
        fakeTopicRepository.Add(new LearningTopic { Id = 1, Title = "test1" });
        fakeTopicRepository.Add(new LearningTopic { Id = 2, Title = "test2" });

        fakeSessionRepository.Add(new StudySession {Id = 1, DurationMinutes = 1, LearningTopicId = 1 });
        fakeSessionRepository.Add(new StudySession {Id = 2, DurationMinutes = 2, LearningTopicId = 1 });
        fakeSessionRepository.Add(new StudySession {Id = 3, DurationMinutes = 3, LearningTopicId = 2 });

        var result = await service.GetSessionsAsync(1);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.DurationMinutes == 1);
        Assert.Contains(result, x => x.DurationMinutes == 2);
        Assert.DoesNotContain(result, x => x.DurationMinutes == 3);
    }
}
