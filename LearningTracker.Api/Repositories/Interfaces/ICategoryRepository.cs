using LearningTracker.Api.Models;

namespace LearningTracker.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int id);
    void Add(Category category);
    void Remove(Category category);
    Task SaveChangesAsync();
    Task<bool> NameExistsAsync(string name, int id);
}
