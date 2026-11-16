using LearningTracker.Api.Models;
using LearningTracker.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningTracker.Tests
{
    public class FakeCategoryRepository : ICategoryRepository
    {
        public readonly List<Category> _categories = new List<Category>();

        public void Add(Category category)
        {
            _categories.Add(category);
        }

        public Task<List<Category>> GetCategoriesAsync()
        {
            return Task.FromResult(_categories);
        }

        public Task<Category?> GetCategoryByIdAsync(int id)
        {
            return Task.FromResult(_categories.FirstOrDefault(x => x.Id == id));
        }

        public Task<bool> NameExistsAsync(string name, int id)
        {
            return Task.FromResult(_categories.Any(x => x.NormalizedName == name && x.Id != id));
        }

        public void Remove(Category category)
        {
            _categories.Remove(category);
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
