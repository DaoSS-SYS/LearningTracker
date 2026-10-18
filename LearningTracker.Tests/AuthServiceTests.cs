using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Models;
using LearningTracker.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace LearningTracker.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterUser_EmailAlreadyExists_ThrowUserAlreadyExistsException()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var userRepository = new FakeUserRepository();
        var service = new AuthService(userRepository, NullLogger<AuthService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        userRepository.Add(new User {Email = "test@mail.ru"});
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.CreateAsync(new RegisterRequest {Email = "test@mail.ru"}));
    }

    [Fact]
    public async Task RegisterUser_EmailIsFree_CreateAsync()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var userRepository = new FakeUserRepository();
        var service = new AuthService(userRepository, NullLogger<AuthService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        await service.CreateAsync(new RegisterRequest { Email = "test@mail.ru", Password = "testtest" });
        var user = Assert.Single(userRepository.Users);
        Assert.Equal("test@mail.ru", user.Email);
        var hasher = new PasswordHasher<User>();
        Assert.Equal(PasswordVerificationResult.Success, hasher.VerifyHashedPassword(user, user.PasswordHash, "testtest"));
        Assert.Equal(PasswordVerificationResult.Failed, hasher.VerifyHashedPassword(user, user.PasswordHash, "tessssssss"));
        Assert.NotEqual("testtest", user.PasswordHash);
        Assert.Equal("User", user.Role);

        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.CreateAsync(new RegisterRequest { Email = "TEST@mail.ru" }));
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.CreateAsync(new RegisterRequest { Email = "    TEST@mail.ru" }));
    }

    [Fact]
    public async Task LoginAsync_IncorrectEmail_ThrowInvalidCredentialsException()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var userRepository = new FakeUserRepository();
        var service = new AuthService(userRepository, NullLogger<AuthService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        userRepository.AddWithHash("test@mail.ru", "testtest");

        await Assert.ThrowsAsync <InvalidCredentialsException>(() => service.LoginAsync(new LoginRequest { Email = "test2@mail.ru", Password = "testtest" }));
    }

    [Fact]
    public async Task LoginAsync_IncorrectPassword_ThrowInvalidCredentialsException()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var userRepository = new FakeUserRepository();
        var service = new AuthService(userRepository, NullLogger<AuthService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        userRepository.AddWithHash("test@mail.ru", "testtest");

        await Assert.ThrowsAsync<InvalidCredentialsException>(() => service.LoginAsync(new LoginRequest { Email = "test@mail.ru", Password = "testtest22" }));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        // Arrange: подготовить (фейковый репозиторий, сервис)
        var userRepository = new FakeUserRepository();
        var service = new AuthService(userRepository, NullLogger<AuthService>.Instance);

        // Act + Assert: выполнить и проверить, что бросилось нужное исключение
        userRepository.AddWithHash("test@mail.ru", "testtest");

        var token = await service.LoginAsync(new LoginRequest { Email = "test@mail.ru", Password = "testtest" });
        Assert.NotEmpty(token.Token);
    }
}