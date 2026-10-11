using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Services;

namespace LearningTracker.Tests;

public class LearningTopicServiceTests
{
    [Fact]
    public async Task GetByIdAsync_TopicNotFound_ThrowsNotFoundException()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var service = new LearningTopicService(new FakeLearningTopicRepository());

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        await Assert.ThrowsAsync<TopicNotFoundException>(() => service.GetByIdAsync(10));


    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsTopicNotCompleted()
    {
        // Arrange
        var fake = new FakeLearningTopicRepository();
        var service = new LearningTopicService(fake);

        // Act + Assert
        var topic = await service.CreateAsync(new CreateLearningTopicRequest { Title = "EF" });

        Assert.Equal("EF", topic.Title);
        Assert.False(topic.IsCompleted);
        Assert.Single(fake.Topics);





    }

}