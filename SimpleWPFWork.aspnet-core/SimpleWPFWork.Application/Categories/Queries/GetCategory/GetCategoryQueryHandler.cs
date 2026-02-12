using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                throw new Exception($"Kategori bulunamadı: {request.Id}");

            return _mapper.Map<CategoryDto>(category);
        }
    }
}