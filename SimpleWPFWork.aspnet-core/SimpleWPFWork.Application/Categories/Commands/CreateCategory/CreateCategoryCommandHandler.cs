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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Color = request.Color
            };

            await _categoryRepository.CreateAsync(category);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
