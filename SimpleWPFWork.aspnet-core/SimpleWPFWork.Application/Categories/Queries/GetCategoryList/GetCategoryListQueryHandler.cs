using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using SimpleWPFWork.Domain.Entities.Categories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryListQueryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetFilteredAsync(
                name: request.Name,
                color: request.Color,
                page: (int)request.Page,
                limit: (int)request.Limit);

            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}