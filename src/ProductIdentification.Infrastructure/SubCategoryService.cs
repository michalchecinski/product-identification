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

        public async Task<SubCategory> AddSubcategory(string subCategoryName, string categoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);

            if (category == null)
            {
                throw new Exception($"Category with name: {categoryName} does not exist.");
            }

            var subCategory = new SubCategory
            {
                Name = subCategoryName,
                Category = category,
                CategoryId = category.Id
            };

            await _subCategoryRepository.AddSubCategoryAsync(subCategory);

            return subCategory;
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

        public async Task<SubCategory> UpdateSubcategory(string subCategoryName, string categoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);

            if (category == null)
            {
                throw new Exception($"Category with name: {categoryName} does not exist.");
            }

            var subCategory = new SubCategory
            {
                Name = subCategoryName,
                Category = category,
                CategoryId = category.Id
            };

            return await _subCategoryRepository.UpdateSubCategoryAsync(subCategory);
        }

        public async Task<IEnumerable<SubCategory>> GetSubcategories()
        {
            return await _subCategoryRepository.GetAll();
        }

        public async Task<string> GetSubcategoryNameById(int id)
        {
            var subCat = await _subCategoryRepository.GetName(id);
            if (subCat == null)
            {
                throw new Exception($"SubCategory with id: {id} does not exist.");
            }

            return subCat;
        }
    }
}