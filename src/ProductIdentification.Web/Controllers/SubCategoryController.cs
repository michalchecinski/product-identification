using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private ICategoryRepository _categoryRepository;

        public SubCategoryController(ICategoryService categoryService, 
                                     IMapper mapper,
                                     ISubCategoryService subCategoryService,
                                     ISubCategoryRepository subCategoryRepository,
                                     ICategoryRepository categoryRepository)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _subCategoryService = subCategoryService;
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: SubCategory
        public async Task<ActionResult> Index()
        {
            var subCategory = await _subCategoryRepository.GetAll();
            var model = _mapper.Map<IEnumerable<SubCategoryViewModel>>(subCategory);
            return View("Index", model);
        }

        // GET: SubCategory
        public async Task<ActionResult> ListFromCategory(int id)
        {
            ViewData["Category"] = await _categoryService.GetCategoryNameById(id);
            var subCategory = await _subCategoryService.GetAllByCategory(id);
            var model = _mapper.Map<IEnumerable<SubCategoryViewModel>>(subCategory);
            return View("Index", model);
        }

        // GET: SubCategory/Create
        public async Task<ActionResult> Create()
        {
            var model = new SubCategoryCreateModel();
            await AddCategories(model);

            return View(model);
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubCategoryCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
            
            var category = await _categoryRepository.GetCategoryByNameAsync(model.CategoryName);
            if (category.SubCategories.FirstOrDefault(x => x.Name == model.Name) != null)
            {
                ModelState.AddModelError(nameof(SubCategoryCreateModel.Name),
                    "SubCategory with this name already exists in that category");
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
            try
            {
                var subCategory = await _subCategoryService.AddSubcategory(model.Name, model.CategoryName);

                return RedirectToAction(nameof(ListFromCategory), new {id = subCategory.CategoryId});
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
        }

        // GET: SubCategory/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var subCategory = await _subCategoryService.GetSubcategoryById(id);

            var model = _mapper.Map<SubCategoryCreateModel>(subCategory);
            await AddCategories(model);

            return View(model);
        }

        private async Task AddCategories(SubCategoryCreateModel model)
        {
            var categories = await _categoryService.GetAllCategoriesNames();
            model.CategoriesNames = categories;
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SubCategoryCreateModel model)
        {
            if (model.Id != id)
            {
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
            var category = await _categoryRepository.GetCategoryByNameAsync(model.CategoryName);
            if (category.SubCategories.FirstOrDefault(x => x.Name == model.Name) != null)
            {
                ModelState.AddModelError(nameof(SubCategoryCreateModel.Name),
                    "SubCategory with this name already exists in that category");
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
            
            try
            {
                var subCategory = await _subCategoryService.UpdateSubcategory(model.Name, model.CategoryName);

                return RedirectToAction(nameof(ListFromCategory), new { id = subCategory.CategoryId });
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                await FillCategoryAndCategoryList(model);
                return View(model);
            }
        }

        private async Task FillCategoryAndCategoryList(SubCategoryCreateModel model)
        {
            if (model.CategoryName == null)
            {
                model.CategoryName = string.Empty;
            }

            if (model.CategoriesNames == null)
            {
                await AddCategories(model);
            }
        }
    }
}