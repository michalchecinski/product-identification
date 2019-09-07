using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductIdentification.Core.DomainModels;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task AddCategory(Category category)
        {
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            return await _categoryRepository.UpdateCategoryAsync(category);
        }

        public Task<List<Category>> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public async Task<List<string>> GetAllCategoriesNames()
        {
            return await _categoryRepository.GetAllNames();
        }

        public async Task<string> GetCategoryNameById(int id)
        {
            var category = await _categoryRepository.GetName(id);
            if (category == null)
            {
                throw new Exception($"Category with id: {id} does not exist.");
            }

            return category;
        }
    }
}
