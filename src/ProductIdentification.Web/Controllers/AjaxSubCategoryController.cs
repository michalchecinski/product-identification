using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Web.Controllers
{
    [Authorize]
    public class AjaxSubCategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public AjaxSubCategoryController(ICategoryService categoryService,
                                     IMapper mapper,
                                     ISubCategoryService subCategoryService,
                                     ISubCategoryRepository subCategoryRepository)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _subCategoryService = subCategoryService;
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<ActionResult> GetSubcategories(string category)
        {
            List<string> subCategory;
            try
            {
                subCategory = await _subCategoryService.GetSubcategoriesByCategoryName(category);
            }
            catch (ArgumentException e)
            {
                return new BadRequestObjectResult(e.Message);
            }

            return new JsonResult(subCategory);
        }
    }
}
