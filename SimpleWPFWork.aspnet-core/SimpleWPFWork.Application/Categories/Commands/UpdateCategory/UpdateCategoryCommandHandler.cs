using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            // ✅ Category nesnesini hazırla
            var category = new Category
            {
                Id = request.Id,
                Name = request.Name,
                Color = request.Color
            };

            // ✅ Dapper ile update
            var updatedCategory = await _categoryRepository.UpdateAsync(category);

            return _mapper.Map<CategoryDto>(updatedCategory);
        }
    }
}