using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProductIdentification.Web.Models
{
    public class ProductViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [DisplayName("Net Price")]
        [Required(ErrorMessage = "Required")]
        public decimal NetPrice { get; set; }

        [DisplayName("Gross price")]
        [Required(ErrorMessage = "Required")]
        public decimal GrossPrice { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Required")]
        public string CategoryName { get; set; }

        [DisplayName("Subcategory Name")]
        [Required(ErrorMessage = "Required")]
        public string SubCategoryName { get; set; }
    }
}
