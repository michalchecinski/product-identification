using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProductIdentification.Core.Models;
using ProductIdentification.Web.Models;

namespace ProductIdentification.Web.Automapper
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name));

            CreateMap<SubCategoryCreateModel, SubCategory>();
            CreateMap<SubCategory, SubCategoryCreateModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name));

            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(x => x.SubCategoryName, opt => opt.MapFrom(x => x.SubCategory.Name));

            CreateMap<ProductCreateModel, Product>();
            CreateMap<Product, ProductCreateModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category.Name))
                .ForMember(x => x.SubCategoryName, opt => opt.MapFrom(x => x.SubCategory.Name));
        }
    }
}