﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;

namespace ProductIdentification.Infrastructure
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public ProductService(IProductRepository productRepository,
                              ICategoryRepository categoryRepository,
                              ISubCategoryRepository subCategoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
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

            return subcategory.Products.ToList();
        }

        public async Task<List<Product>> GetAllByCategory(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category with id: {categoryId} does not exist");
            }

            return category.Products.ToList();
        }

        public async Task AddProduct(Product product)
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

            await _productRepository.AddProductAsync(product);
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
    }
}