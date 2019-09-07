using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProductIdentification.Web.Models
{
    public class ProductCreateModel : ProductViewModel, IValidatableObject
    {
        public IEnumerable<string> SubCategoryNames { get; set; }
        public IEnumerable<string> CategoryNames { get; set; }
        public List<IFormFile> files { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (files.Count < 1)
            {
                yield return new ValidationResult(
                    "You should upload at least 1 product pictures", new[] {nameof(files)});
            }

            if (GrossPrice <= NetPrice)
            {
                yield return new ValidationResult(
                    "Gross price should be greater than Net price", new[] {nameof(GrossPrice), nameof(NetPrice)});
            }
        }
    }
}