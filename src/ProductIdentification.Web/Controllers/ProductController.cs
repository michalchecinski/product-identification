﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductIdentification.Common;
using ProductIdentification.Core.DomainModels;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IProductRepository _productRepository;

        public ProductController(IProductService productService,
            IMapper mapper,
            ISubCategoryService subCategoryService,
            ICategoryService categoryService,
            IProductRepository productRepository)
        {
            _productService = productService;
            _mapper = mapper;
            _subCategoryService = subCategoryService;
            _categoryService = categoryService;
            _productRepository = productRepository;
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetAll();
            var model = _mapper.Map<List<ProductViewModel>>(products);

            return View("Index", model);
        }

        // GET: Product
        public async Task<ActionResult> ListFromSubCategory(int id)
        {
            var subCatProduct = await _productService.GetAllBySubCategory(id);
            var model = _mapper.Map<List<ProductViewModel>>(subCatProduct);

            var subCategory = await _subCategoryService.GetSubcategoryById(id);

            ViewData["Subcategory"] = subCategory.Name;
            ViewData["Category"] = subCategory.Category.Name;

            return View("Index", model);
        }

        public async Task<ActionResult> ListFromCategory(int id)
        {
            var catProduct = await _productService.GetAllByCategory(id);
            var model = _mapper.Map<IEnumerable<ProductViewModel>>(catProduct);

            ViewData["Category"] = await _categoryService.GetCategoryNameById(id);

            return View("Index", model);
        }

        // GET: Product/Create
        public async Task<ActionResult> Create(string category, string subcategory)
        {
            var model = new ProductCreateModel
            {
                CategoryNames = await _categoryService.GetAllCategoriesNames(),
                SubCategoryNames = new List<string>()
            };

            if (!string.IsNullOrWhiteSpace(category))
            {
                var subCats = await _subCategoryService.GetSubcategoriesByCategoryName(category);
                model.CategoryName = category;
                model.SubCategoryNames = subCats ?? new List<string>();
            }

            if (!string.IsNullOrWhiteSpace(subcategory))
            {
                model.SubCategoryName = subcategory;
            }

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var product = await _productService.Get(id);
            return View(product);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillSubcategoryList(model);

                return View(model);
            }

            try
            {
                var files = model.files;

                var product = _mapper.Map<Product>(model);
                var result = await _productService.AddProduct(product,
                    model.CategoryName,
                    model.SubCategoryName,
                    files);

                return RedirectToAction(nameof(Details), new {id = result.Id});
            }
            catch(Exception ex)
            {
                ViewData["Error"] = ex.Message;
                await FillSubcategoryList(model);
                return View(model);
            }
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productRepository.Get(id);
            var model = _mapper.Map<ProductCreateModel>(product);
            model.CategoryNames = await _categoryService.GetAllCategoriesNames();
            model.SubCategoryNames = await _subCategoryService.GetSubcategoriesByCategoryName(model.CategoryName);

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ProductCreateModel model)
        {
            if (model.Id != id)
            {
                await FillSubcategoryList(model);
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await FillSubcategoryList(model);
                return View(model);
            }

            try
            {
                var product = _mapper.Map<Product>(model);
                var result = _productService.UpdateProduct(product, model.CategoryName, model.SubCategoryName);

                return RedirectToAction(nameof(Details), new {id = result.Id});
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                await FillSubcategoryList(model);
                return View(model);
            }
        }

        private async Task FillSubcategoryList(ProductCreateModel model)
        {
            model.CategoryNames = await _categoryService.GetAllCategoriesNames();

            var category = model.CategoryName;
            if (!string.IsNullOrWhiteSpace(category))
            {
                var subCats = await _subCategoryService.GetSubcategoriesByCategoryName(category);
                model.SubCategoryNames = subCats ?? new List<string>();
            }
            else
            {
                model.SubCategoryNames = new List<string>();
            }
        }
    }
}