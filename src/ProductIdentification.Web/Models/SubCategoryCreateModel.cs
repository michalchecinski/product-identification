using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductIdentification.Web.Models
{
    public class SubCategoryCreateModel : SubCategoryViewModel
    { 
        public List<string> CategoriesNames { get; set; }
    }
}
