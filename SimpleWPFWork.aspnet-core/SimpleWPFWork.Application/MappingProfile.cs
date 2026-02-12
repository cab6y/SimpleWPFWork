using AutoMapper;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.Domain.Entities.Categories;
namespace SimpleWPFWork.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category Mappings
            CreateMap<Category, CategoryDto>();

            CreateMap<CategoryCommand, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id command'dan set edilmez
                .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
                .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletionTime, opt => opt.Ignore())
                .ForMember(dest => dest.DeleterUserId, opt => opt.Ignore());
        }
    }
}
