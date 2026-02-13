using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;

namespace SimpleWPFWork.Host.Controllers.Todos
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoAppService _service;

        public TodoController(ITodoAppService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<TodoDto> Create([FromBody] CreateTodoCommand input)
        {
            return await _service.CreateAsync(input);
        }

        [HttpPut]
        public async Task<TodoDto> Update([FromBody] UpdateTodoCommand input)
        {
            return await _service.UpdateAsync(input);
        }

        [HttpGet]
        public async Task<List<TodoDto>> GetList([FromQuery] GetTodoListQuery input)
        {
            return await _service.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public async Task<TodoDto> Get(Guid id)
        {
            return await _service.GetAsync(new GetTodoQuery { Id = id });
        }
    }
}