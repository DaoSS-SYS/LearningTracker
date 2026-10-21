using LearningTracker.Api.Data;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LearningTracker.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LearningTrackerDbContext _context;
    public CategoryRepository(LearningTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
    public void Add(Category category)
    {
        _context.Categories.Add(category);
    }
    public void Remove(Category category)
    {
        _context.Categories.Remove(category);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<bool> NameExistsAsync(string name, int id)
    {
        return await _context.Categories.AnyAsync(x => x.NormalizedName == name && x.Id != id);
    }
}
