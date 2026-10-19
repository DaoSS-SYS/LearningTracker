using LearningTracker.Api.Models;
using LearningTracker.Api.Services.Interfaces;

namespace LearningTracker.Tests;

public class FakeJwtTokenService : IJwtTokenService
{
    public string CreateToken(User user)
    {
        return "test-token";
    }
}
