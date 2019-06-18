﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Web.Controllers
{
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
            var subCategory = new List<string>();
            try
            {
                subCategory = await _subCategoryService.GetSubcategoriesByCategoryName(category);
            }
            catch (Exception e)
            {

            }
            
            return new JsonResult(subCategory);
        }
    }
}
