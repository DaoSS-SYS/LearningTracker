using LearningTracker.Api.Data;
using LearningTracker.Api.Dto;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LearningTrackerDbContext _context;
    public UserRepository(LearningTrackerDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
    public void Add(User user)
    {
        _context.Add(user);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
