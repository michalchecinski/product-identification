using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class IdentifyProductFunction
    {
        private readonly IProductIdentifyService _identifyService;

        public IdentifyProductFunction(IProductIdentifyService identifyService)
        {
            _identifyService = identifyService;
        }

        [FunctionName(nameof(IdentifyProduct))]
        public async Task<IActionResult> IdentifyProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            
            log.LogInformation("Identify product function called via HTTP");
            log.LogInformation($"Request body isNull: {req.Body.IsNull()}");
            
            var product = await _identifyService.IdentifyProduct(req.Body);

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

            return new JsonResult(JsonConvert.SerializeObject(result));
        }
    }
}