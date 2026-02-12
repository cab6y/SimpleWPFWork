using MediatR;
using SimpleWPFWork.Application.Categories.Commands.CreateCategory;
using SimpleWPFWork.Application.Categories.Commands.DeleteCategory;
using SimpleWPFWork.Application.Categories.Commands.UpdateCategory;
using SimpleWPFWork.Application.Categories.Queries.GetCategory;
using SimpleWPFWork.Application.Categories.Queries.GetCategoryList;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Categories
{
    public class CategoryAppService : ICategoryAppService
    {
        private readonly IMediator _mediator;

        public CategoryAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryCommand input)
        {
            return await _mediator.Send(new CreateCategoryCommand
            {
                Name = input.Name,
                Color = input.Color
            });
        }

        public async Task<CategoryDto> UpdateAsync(UpdateCategoryCommand input)
        {
            return await _mediator.Send(new UpdateCategoryCommand
            {
                Id = input.Id,
                Name = input.Name,
                Color = input.Color
            });
        }

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            return await _mediator.Send(new GetCategoryQuery { Id = id });
        }

        public async Task<List<CategoryDto>> GetPaginationListAsync(GetCategoryListQuery input)
        {
            return await _mediator.Send(new GetCategoryListQuery
            {
                Name = input.Name,
                Color = input.Color,
                Limit = input.Limit,
                Page = input.Page
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });
        }
    }
}