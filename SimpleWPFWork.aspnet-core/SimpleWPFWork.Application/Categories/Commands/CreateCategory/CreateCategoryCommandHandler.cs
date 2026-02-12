using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Color = request.Color
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
