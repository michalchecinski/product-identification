using System.Collections.Generic;
using ProductIdentification.Core.DomainModels;

namespace ProductIdentification.Core.Models
{
    public class ReviewProductPhotosDto
    {
        public Product Product { get; set; }
        public IEnumerable<PhotoFile> Photos { get; set; }
    }
}