using LearningTracker.Api.Models;

namespace LearningTracker.Api.Services.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
