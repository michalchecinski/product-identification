using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductIdentification.Core.Models;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Controllers
{
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
            var product = await _productRepository.Get(id);
            var model = _mapper.Map<ProductViewModel>(product);
            return View(model);
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateModel model)
        {
            if (!ModelState.IsValid)
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
                
                return View(model);
            }

            try
            {
                var files = model.files;

                // full path to file in temp location
                var filePath = Path.GetTempFileName();

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                var product = _mapper.Map<Product>(model);
                var result = _productService.AddProduct(product, model.CategoryName, model.SubCategoryName);

                return RedirectToAction(nameof(Details), new { id = result.Id});
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _productRepository.Get(id);
            var model = _mapper.Map<ProductCreateModel>(product);

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductCreateModel model)
        {
            if (model.Id != id)
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var product = _mapper.Map<Product>(model);
                var result = _productService.UpdateProduct(product, model.CategoryName, model.SubCategoryName);

                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            catch
            {
                return View();
            }
        }
    }
}