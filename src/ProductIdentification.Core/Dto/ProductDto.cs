using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Core.Dto
{
    public class ProductDto
    {
        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            NetPrice = product.NetPrice;
            GrossPrice = product.GrossPrice;
            Category = product.Category.Name;
            SubCategory = product.SubCategory.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Photo { get; set; }
    }
}