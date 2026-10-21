using LearningTracker.Api.Dto;
using LearningTracker.Api.Exceptions;
using LearningTracker.Api.Mapping;
using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using LearningTracker.Api.Services.Interfaces;

namespace LearningTracker.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILearningTopicRepository _learningTopicRepository;
    public CategoryService(ICategoryRepository categoryRepository, ILearningTopicRepository learningTopicRepository)
    {
        _categoryRepository = categoryRepository;
        _learningTopicRepository = learningTopicRepository;
    }
    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetCategoriesAsync();

        return categories.Select(x => x.ToDto()).ToList();
    }
    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var category = await GetCategoryOrThrowAsync(id);

        return category.ToDto();
    }
    public async Task DeleteAsync(int id)
    {
        var category = await GetCategoryOrThrowAsync(id);

        if (await _learningTopicRepository.AnyTopicsInCategoryAsync(id))
            throw new CategoryHasTopicsException(id);

        _categoryRepository.Remove(category);

        await _categoryRepository.SaveChangesAsync();
    }
    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        if (await _categoryRepository.NameExistsAsync(request.Name.ToLower().Trim(), 0))
            throw new CategoryNameIsBusyException(request.Name);

        var category = new Category
        {
            Name = request.Name.Trim(),
            CreatedAtUtc = DateTime.UtcNow,
            NormalizedName = request.Name.ToLower().Trim()
        };

        _categoryRepository.Add(category);

        await _categoryRepository.SaveChangesAsync();

        return category.ToDto();
    }
    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequest request)
    {
        var category = await GetCategoryOrThrowAsync(id);

        if (await _categoryRepository.NameExistsAsync(request.Name.ToLower().Trim(), id))
            throw new CategoryNameIsBusyException(request.Name);

        category.Name = request.Name.Trim();
        category.NormalizedName = request.Name.ToLower().Trim();

        await _categoryRepository.SaveChangesAsync();

        return category.ToDto();
    }
    private async Task<Category> GetCategoryOrThrowAsync(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category is null)
            throw new CategoryNotFoundException(id);

        return category;
    }
}
