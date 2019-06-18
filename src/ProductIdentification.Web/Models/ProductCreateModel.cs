using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProductIdentification.Core.Models;

namespace ProductIdentification.Web.Models
{
    public class ProductCreateModel : ProductViewModel
    {
        public IEnumerable<SubCategory> SubCategoryNames { get; set; }
        public IEnumerable<string> CategoryNames { get; set; }
        public List<IFormFile> files { get; set; }

    }
}
