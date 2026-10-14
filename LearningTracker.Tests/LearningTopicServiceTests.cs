using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace LearningTracker.Tests;

public class LearningTopicServiceTests
{
    [Fact]
    public async Task GetByIdAsync_TopicNotFound_ThrowsNotFoundException()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var service = new LearningTopicService(new FakeLearningTopicRepository(), new FakeStudySessionsRepository(), NullLogger<LearningTopicService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        await Assert.ThrowsAsync<TopicNotFoundException>(() => service.GetByIdAsync(10));
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsTopicNotCompleted()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var fakeSessionsRep = new FakeStudySessionsRepository();
        var service = new LearningTopicService(fakeTopicRep, fakeSessionsRep, NullLogger<LearningTopicService>.Instance);

        // Act + Assert
        var topic = await service.CreateAsync(new CreateLearningTopicRequest { Title = "EF" });

        Assert.Equal("EF", topic.Title);
        Assert.False(topic.IsCompleted);
        Assert.Single(fakeTopicRep.Topics);
    }

    [Fact]
    public async Task CompleteAsync_ValidRequest_ReturnsTopicCompleted()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var fakeSessionsRep = new FakeStudySessionsRepository();
        var service = new LearningTopicService(fakeTopicRep, fakeSessionsRep, NullLogger<LearningTopicService>.Instance);

        // Act + Assert
        fakeTopicRep.Add(new LearningTopic { Id = 2, Title = "TestComplete", IsCompleted = false});

        var topic = await service.CompleteAsync(2);

        Assert.True(fakeTopicRep.Topics.First(x => x.Id == 2).IsCompleted);
        Assert.True(topic.IsCompleted);
    }

    [Fact]
    public async Task DeleteAsync_TopicHasSessions_ThrowsTopicHasSessionsException()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var fakeSessionsRep = new FakeStudySessionsRepository();
        var service = new LearningTopicService(fakeTopicRep, fakeSessionsRep, NullLogger<LearningTopicService>.Instance);

        // Act + Assert
        fakeTopicRep.Add(new LearningTopic { Id = 2, Title = "Test" });
        fakeSessionsRep.Add(new StudySession { Id = 1, LearningTopicId = 2 });

        await Assert.ThrowsAsync<TopicHasSessionsException>(() => service.DeleteAsync(2));
        Assert.Contains(fakeTopicRep.Topics, x => x.Id == 2);

    }

    [Fact]
    public async Task DeleteAsync_TopicWithoutSessions_RemovesTopic()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var fakeSessionsRep = new FakeStudySessionsRepository();
        var service = new LearningTopicService(fakeTopicRep, fakeSessionsRep, NullLogger<LearningTopicService>.Instance);

        // Act + Assert
        fakeTopicRep.Add(new LearningTopic { Id = 3, Title = "Test" });

        await service.DeleteAsync(3);

        Assert.DoesNotContain(fakeTopicRep.Topics, x => x.Id == 3);
    }

    [Fact]
    public async Task UpdateAsync_TopicExists_UpdatesFields()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var service = new LearningTopicService(fakeTopicRep, new FakeStudySessionsRepository(), NullLogger<LearningTopicService>.Instance);

        fakeTopicRep.Add(new LearningTopic { Id = 1, Title = "Old" });

        // Act
        var newTopicCheck = await service.UpdateAsync(1, new UpdateLearningTopicRequest { Title = "New", Description = "newDesc"});

        var topicCheck = Assert.Single(fakeTopicRep.Topics);

        // Assert
        Assert.Equal("New", topicCheck.Title);
        Assert.Equal("newDesc", topicCheck.Description);

        Assert.Equal("New", newTopicCheck.Title);
        Assert.Equal("newDesc", newTopicCheck.Description);

        Assert.NotEqual("Old", topicCheck.Title);
    }

    [Fact]
    public async Task UpdateAsync_TopicNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var fakeTopicRep = new FakeLearningTopicRepository();
        var service = new LearningTopicService(fakeTopicRep, new FakeStudySessionsRepository(), NullLogger<LearningTopicService>.Instance);

        // Act + Assert
        await Assert.ThrowsAsync<TopicNotFoundException>(() => service.UpdateAsync(1, new UpdateLearningTopicRequest { Title = "test"}));
    }

}