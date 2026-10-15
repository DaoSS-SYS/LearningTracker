using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Mapping;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearningTracker.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository repository, ILogger<AuthService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<UserDto> CreateAsync(RegisterRequest request)
    {
        await ThrowExceptionIfUserExists(request.Email.ToLower().Trim());

        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            CreatedAtUtc = DateTime.UtcNow,
            Email = request.Email.ToLower().Trim(),
            Role = "User",
        };

        user.PasswordHash = hasher.HashPassword(user, request.Password);        
        
        _repository.Add(user);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("User {Id} created", user.Id);

        return user.ToDto();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _repository.GetUserByEmailAsync(request.Email.ToLower().Trim());
        var hasher = new PasswordHasher<User>();

        if (user is null)
            throw new InvalidCredentialsException();

        if(hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            throw new InvalidCredentialsException();

        return new LoginResponse { Token = "testToken" };
    }

    private async Task ThrowExceptionIfUserExists(string email)
    {
        var user = await _repository.GetUserByEmailAsync(email);
        if (user is not null)
            throw new UserAlreadyExistsException(email);
    }
}
