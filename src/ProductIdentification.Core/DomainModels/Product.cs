using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductIdentification.Core.DomainModels
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid CustomVisionTagId { get; set; }
        [NotMapped]
        public string TagName => $"{Category.Name}_{SubCategory.Name}_{Name}".Replace(" ", "_");
    }
}