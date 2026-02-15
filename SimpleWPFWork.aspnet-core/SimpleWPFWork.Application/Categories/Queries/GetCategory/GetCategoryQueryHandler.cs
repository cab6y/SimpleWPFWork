using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(request.Id);

            if (category == null)
                throw new Exception($"Kategori bulunamadı: {request.Id}");

            return _mapper.Map<CategoryDto>(category);
        }
    }
}