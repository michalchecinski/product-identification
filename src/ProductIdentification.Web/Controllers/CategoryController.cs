using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.SystemFunctions;
using ProductIdentification.Core.DomainModels;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryService categoryService, ICategoryRepository categoryRepository,
                                  IMapper mapper)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: CategoriesNamesList
        public async Task<ActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            var viewModel = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return View(viewModel);
        }

        // GET: CategoriesNamesList/Create
        public ActionResult Create()
        {
            var model = new CategoryViewModel();
            return View(model);
        }

        // POST: CategoriesNamesList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _categoryRepository.GetCategoryByNameAsync(model.Name) != null)
            {
                ModelState.AddModelError(nameof(CategoryViewModel.Name), "Category with that name already exists");
                return View(model);
            }

            try
            {
                var category = _mapper.Map<Category>(model);

                await _categoryService.AddCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View(model);
            }
        }

        // GET: CategoriesNamesList/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategory(id);
            var viewModel = _mapper.Map<CategoryViewModel>(category);
            return View(viewModel);
        }

        // POST: CategoriesNamesList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var categoryFromDb = await _categoryRepository.GetCategoryByNameAsync(model.Name);
            if (categoryFromDb != null && (categoryFromDb.Id != model.Id || categoryFromDb.Id != id))
            {
                ModelState.AddModelError(nameof(CategoryViewModel.Name), "Category with that name already exists");
                return View(model);
            }

            try
            {
                var category = _mapper.Map<Category>(model);

                await _categoryService.UpdateCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View(model);
            }
        }
    }
}