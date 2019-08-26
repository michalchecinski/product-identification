using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductIdentification.Core.Models;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class IdentifyProduct
    {
        private readonly IProductIdentifyService _identifyService;

        public IdentifyProduct(IProductIdentifyService identifyService)
        {
            _identifyService = identifyService;
        }


        [FunctionName("IdentifyProduct")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            Product product = await _identifyService.IdentifyProduct(req.Body);

            if (product == null)
            {
                return new NotFoundObjectResult("This product cannot be found.");
            }

            var result = new
            {
                product.Id,
                product.Name,
                product.GrossPrice,
                product.NetPrice,
                CategoryName = product.Category.Name,
                SubCategoryName = product.SubCategory.Name
            };

            //var result = new
            //{
            //    Name = "Name",
            //    GrossPrice = 12.00,
            //    NetPrice = 10.50,
            //    CategoryName = "CategoryName",
            //    SubCategoryName = "SubCategoryName"
            //};

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
    }
}