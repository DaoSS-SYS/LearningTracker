using LearningTracker.Api.Dto;
using LearningTracker.Api.Filters;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LearningTracker.Tests;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>(); 
    public IReadOnlyList<User> Users { get { return _users; } }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return Users.FirstOrDefault(x => x.Email == email);
    }
    public void Add(User user)
    {
        _users.Add(user);
    }
    public void AddWithHash(string email, string password)
    {
        var hasher = new PasswordHasher<User>();

        var user = new User
        {
            CreatedAtUtc = DateTime.UtcNow,
            Email = email,
            Role = "User",
        };

        user.PasswordHash = hasher.HashPassword(user, password);

        _users.Add(user);
    }
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}
