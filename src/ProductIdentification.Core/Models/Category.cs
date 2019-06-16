namespace ProductIdentification.Core.Models
{
    using System.Collections.Generic;

    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
