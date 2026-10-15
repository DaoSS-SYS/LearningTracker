using LearningTracker.Api.Dto;

namespace LearningTracker.Api.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto> CreateAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}
