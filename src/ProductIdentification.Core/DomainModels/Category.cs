using System.Collections.Generic;

namespace ProductIdentification.Core.DomainModels
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
