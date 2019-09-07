using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategoryController(ICategoryService categoryService, 
                                     IMapper mapper,
                                     ISubCategoryService subCategoryService,
                                     ISubCategoryRepository subCategoryRepository)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _subCategoryService = subCategoryService;
            _subCategoryRepository = subCategoryRepository;
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
            var categories = await _categoryService.GetAllCategoriesNames();
            model.CategoriesNames = categories;

            return View(model);
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubCategoryCreateModel model)
        {
            try
            {
                var subCategory = await _subCategoryService.AddSubcategory(model.Name, model.CategoryName);

                return RedirectToAction(nameof(ListFromCategory), new {id = subCategory.CategoryId});
            }
            catch
            {
                return View();
            }
        }

        // GET: SubCategory/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var subCategory = await _subCategoryService.GetSubcategoryById(id);

            var model = _mapper.Map<SubCategoryCreateModel>(subCategory);
            model = await AddCategories(model);

            return View(model);
        }

        private async Task<SubCategoryCreateModel> AddCategories(SubCategoryCreateModel model)
        {
            var categories = await _categoryService.GetAllCategoriesNames();
            model.CategoriesNames = categories;
            return model;
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SubCategoryCreateModel model)
        {
            try
            {
                var subCategory = await _subCategoryService.UpdateSubcategory(model.Name, model.CategoryName);

                return RedirectToAction(nameof(ListFromCategory), new { id = subCategory.CategoryId });
            }
            catch
            {
                return View();
            }
        }
    }
}