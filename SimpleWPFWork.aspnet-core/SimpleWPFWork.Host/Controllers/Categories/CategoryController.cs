using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWPFWork.ApplicationContracts.Categories;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.CreateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Commands.UpdateCategory;
using SimpleWPFWork.ApplicationContracts.Categories.Queries.GetCategoryList;

namespace SimpleWPFWork.Host.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryAppService _service;
        public CategoryController(ICategoryAppService service) { _service = service; }
        [HttpPost]
        public async Task<CategoryDto> Create(CreateCategoryCommand input) => await _service.CreateAsync(input);

        [HttpPut("{id}")]
        public async Task<CategoryDto> Update(UpdateCategoryCommand input) => await _service.UpdateAsync(input);

        [HttpGet("{id}")]
        public async Task<CategoryDto> Get(Guid id) => await _service.GetAsync(id);

        [HttpGet]
        public async Task<List<CategoryDto>> GetListPagination([FromQuery] GetCategoryListQuery input) => await _service.GetPaginationListAsync(input);

        [HttpDelete("{id}")]
        public async Task Delete(Guid id) => await _service.DeleteAsync(id);
    }
}
