﻿namespace ProductIdentification.Core.Models
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
    }
}
