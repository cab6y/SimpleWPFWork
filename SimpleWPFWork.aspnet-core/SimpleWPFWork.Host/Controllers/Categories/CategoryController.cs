using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWPFWork.ApplicationContracts.Categories;

namespace SimpleWPFWork.Host.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryAppService _service;
        public CategoryController(ICategoryAppService service) { _service = service; }
        [HttpPost]
        public async Task<CategoryDto> Create(CategoryCommand input) => await _service.CreateAsync(input);

        [HttpPut("{id}")]
        public async Task<CategoryDto> Update(Guid id, CategoryCommand input) => await _service.UpdateAsync(input);

        [HttpGet("{id}")]
        public async Task<CategoryDto> Get(Guid id) => await _service.GetAsync(id);

        [HttpGet]
        public async Task<List<CategoryDto>> GetList([FromQuery] CategoryQuery input) => await _service.GetPaginationList(input);

        [HttpDelete("{id}")]
        public async Task Delete(Guid id) => await _service.DeleteAsync(id);
    }
}
