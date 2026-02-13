using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;

namespace SimpleWPFWork.Host.Controllers.Todos
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<TodoDto> CreateAsync(CreateTodoCommand input)
        {
            return await _mediator.Send(input);
        }

        [HttpPut]
        public async Task<TodoDto> UpdateAsync(UpdateTodoCommand input)
        {
            return await _mediator.Send(input);
        }

        [HttpGet]
        public async Task<List<TodoDto>> GetListAsync([FromQuery] GetTodoListQuery input)
        {
            return await _mediator.Send(input);
        }

        [HttpGet("{id}")]
        public async Task<TodoDto> GetAsync(Guid id)
        {
            return await _mediator.Send(new GetTodoQuery { Id = id });
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _mediator.Send(new DeleteTodoCommand { Id = id });
        }
    }
}