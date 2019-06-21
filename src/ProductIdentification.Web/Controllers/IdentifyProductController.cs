using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductIdentification.Core.Models;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Web.Controllers
{
    public class IdentifyProductController : Controller
    {
        private readonly IProductIdentifyService _productIdentifyService;
        private readonly IMapper _mapper;

        public IdentifyProductController(IProductIdentifyService productIdentifyService, IMapper mapper)
        {
            _productIdentifyService = productIdentifyService;
            _mapper = mapper;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(IFormFile file)
        {
            if (file == null)
            {
                ViewData["FileError"] = "Must provide a valid image";
                return View();
            }

            Product product;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                product = await _productIdentifyService.IdentifyProduct(stream);
            }

            if (product == null)
            {
                ViewData["FileError"] = "Cannot find that product";
                return View();
            }

            return RedirectToAction("Details", "Product", new {id = product.Id});
        }
    }
}