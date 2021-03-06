using System;
using System.IO;
using System.Linq;
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
using ProductIdentification.Common;
using ProductIdentification.Core.Dto;
using ProductIdentification.Core.Repositories;
using ProductIdentification.Infrastructure;

namespace ProductIdentification.Functions
{
    public class IdentifyProductFunction
    {
        private readonly IProductIdentifyService _identifyService;
        private readonly IFileRepository _fileRepository;

        public IdentifyProductFunction(IProductIdentifyService identifyService,
                                       IFileRepository fileRepository)
        {
            _identifyService = identifyService;
            _fileRepository = fileRepository;
        }

        [FunctionName(nameof(IdentifyProduct))]
        public async Task<IActionResult> IdentifyProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Identify product function called via HTTP");
            
            var stream = new MemoryStream();
            await req.Body.CopyToAsync(stream);
            stream.Position = 0;

            var product = await _identifyService.IdentifyProduct(req.Body);

            if (product == null)
            {
                return new NotFoundObjectResult("This product cannot be found.");
            }
            
            log.LogInformation("Uploading blob");
            _fileRepository.SaveFileAsync(product.StoragePathToVerify(), Guid.NewGuid() + ".jpg", stream)
                           .RunAndForget();

            log.LogInformation("Returning result.");
            
            var productOriginalPhotos = await _fileRepository.FilesList(product.StoragePathOriginal());

            var productDto = new ProductDto(product)
            {
                Photo = productOriginalPhotos.FirstOrDefault()?.Path
            };

            return new JsonResult(JsonConvert.SerializeObject(productDto));
        }
    }
}