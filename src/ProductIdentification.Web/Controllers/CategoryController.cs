using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Core.Models;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
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
        public ActionResult Create(CategoryViewModel model)
        {
            //TODO Validate model here

            try
            {
                var category = _mapper.Map<Category>(model);

                _categoryService.AddCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
        public ActionResult Edit(int id, CategoryViewModel model)
        {
            //TODO Validate model here
            try
            {
                var category = _mapper.Map<Category>(model);

                _categoryService.UpdateCategory(category);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}