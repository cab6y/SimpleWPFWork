using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories.FindAsync(new object[] { request.Id }, cancellationToken);

            if (category == null)
                throw new Exception($"Kategori bulunamadı: {request.Id}");

            category.Name = request.Name;
            category.Color = request.Color;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}