using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using SimpleWPFWork.EntityFrameworkCore;

namespace SimpleWPFWork.Application.Categories.Queries.GetCategoryList
{
    public class GetCategoryListQueryHandler : IRequestHandler<GetCategoryListQuery, List<CategoryDto>>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryListQueryHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories.AsQueryable();

            // Filtreleme
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(c => c.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.Color))
                query = query.Where(c => c.Color == request.Color);

            // Sayfalama
            var categories = await query
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CategoryDto>>(categories);
        }
    }
}