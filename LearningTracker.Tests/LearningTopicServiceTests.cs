using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
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
}

