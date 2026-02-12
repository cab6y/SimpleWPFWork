using AutoMapper;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.Domain.Entities.Categories;
namespace SimpleWPFWork.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
