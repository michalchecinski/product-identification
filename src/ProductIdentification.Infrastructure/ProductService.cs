using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IProductIdentifyService _productIdentifyService;

        public ProductService(IProductRepository productRepository,
                              ICategoryRepository categoryRepository,
                              ISubCategoryRepository subCategoryRepository,
                              IProductIdentifyService productIdentifyService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _productIdentifyService = productIdentifyService;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<List<Product>> GetAllBySubCategory(int subCategoryId)
        {
            var subcategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (subcategory == null)
            {
                throw new Exception($"Sub Category with id: {subCategoryId} does not exist");
            }

            var products = await _productRepository.GetAllBySubCategoryId(subCategoryId);

            return products;
        }

        public async Task<List<Product>> GetAllByCategory(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist");
            }

            var products = await _productRepository.GetAllByCategoryId(categoryId);

            return products;
        }

        public async Task<Product> AddProduct(Product product, List<IFormFile> images)
        {
            var categoryId = product.CategoryId;
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist");
            }

            var subCategoryId = product.SubCategoryId;
            var subcategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (subcategory == null)
            {
                throw new Exception($"Sub Category with id: {subCategoryId} does not exist");
            }

            if (subcategory.CategoryId != category.Id)
            {
                throw new Exception(
                    $"Sub Category with id: {subCategoryId} is not child of category with id: {categoryId}");
            }

            product.Category = category;
            product.SubCategory = subcategory;

            await _productIdentifyService.AddProduct(images, product);

            await _productRepository.AddProductAsync(product);
            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var productId = product.Id;
            var existingProduct = _productRepository.GetProductByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new Exception($"Product with id: {productId} does not exist.");
            }

            var categoryId = product.CategoryId;
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist");
            }

            var subCategoryId = product.SubCategoryId;
            var subcategory = await _subCategoryRepository.GetSubCategoryByIdAsync(subCategoryId);
            if (subcategory == null)
            {
                throw new Exception($"Sub Category with id: {subCategoryId} does not exist");
            }

            if (subcategory.CategoryId != category.Id)
            {
                throw new Exception(
                    $"Sub Category with id: {subCategoryId} is not child of category with id: {categoryId}");
            }

            product.Category = category;
            product.SubCategory = subcategory;

            return await _productRepository.UpdateProductAsync(product);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product> AddProduct(Product product, string categoryName, string subCategoryName,
            List<IFormFile> images)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
            if (category == null)
            {
                throw new Exception($"Category with name: {categoryName} does not exist");
            }

            var subCategory = category.SubCategories.SingleOrDefault(x => x.Name == subCategoryName);
            if (subCategory == null)
            {
                throw new Exception($"Subcategory with name: {subCategoryName} does not exist");
            }

            if (subCategory.CategoryId != category.Id)
            {
                throw new Exception(
                    $"Sub Category with name: {subCategoryName} is not child of category with name: {categoryName}");
            }

            product.Category = category;
            product.CategoryId = category.Id;

            product.SubCategory = subCategory;
            product.SubCategoryId = subCategory.Id;

            await _productIdentifyService.AddProduct(images, product);

            await _productRepository.AddProductAsync(product);
            return product;
        }

        public async Task<Product> UpdateProduct(Product product, string categoryName, string subCategoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
            var subCategory = category.SubCategories.SingleOrDefault(x => x.Name == subCategoryName);

            product.Category = category;
            product.CategoryId = category.Id;

            product.SubCategory = subCategory;
            product.SubCategoryId = subCategory.Id;

            return await UpdateProduct(product);
        }
    }
}