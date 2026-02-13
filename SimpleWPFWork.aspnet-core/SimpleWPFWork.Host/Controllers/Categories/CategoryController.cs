using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.DeleteCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;

namespace SimpleWPFWork.Host.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CategoryDto> CreateAsync(CreateCategoryCommand input)
        {
            return await _mediator.Send(input);
        }

        [HttpPut]
        public async Task<CategoryDto> UpdateAsync(UpdateCategoryCommand input)
        {
            return await _mediator.Send(input);
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetAsync(Guid id)
        {
            return await _mediator.Send(new GetCategoryQuery { Id = id });
        }

        [HttpGet]
        public async Task<List<CategoryDto>> GetListAsync([FromQuery] GetCategoryListQuery input)
        {
            return await _mediator.Send(input);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteCategoryCommand { Id = id });
        }
    }
}