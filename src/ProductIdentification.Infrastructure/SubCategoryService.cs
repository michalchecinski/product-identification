using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<SubCategory>> GetAllByCategory(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist");
            }

            return await _subCategoryRepository.GetAll(categoryId);
        }

        public async Task<SubCategory> GetSubcategoryById(int id)
        {
            return await _subCategoryRepository.GetSubCategoryByIdAsync(id);
        }

        public async Task AddSubcategory(SubCategory subCategory)
        {
            var categoryId = subCategory.CategoryId;
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist. Cannot create subcategory.");
            }

            subCategory.Category = category;

            await _subCategoryRepository.AddSubCategoryAsync(subCategory);
        }

        public async Task<SubCategory> UpdateSubcategory(SubCategory subCategory)
        {
            var categoryId = subCategory.CategoryId;
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist. Cannot create subcategory.");
            }

            subCategory.Category = category;

            return await _subCategoryRepository.UpdateSubCategoryAsync(subCategory);
        }
    }
}
