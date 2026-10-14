using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;

namespace LearningTracker.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    void Add(User user);
    Task SaveChangesAsync();
}
