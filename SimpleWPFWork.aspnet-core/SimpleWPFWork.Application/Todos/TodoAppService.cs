using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;

namespace SimpleWPFWork.Application.Todos
{
    public class TodoAppService : ITodoAppService
    {
        private readonly IMediator _mediatr;
        public TodoAppService(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        public async Task<TodoDto> CreateAsync(CreateTodoCommand input)
        {
            return await _mediatr.Send(input);
        }

        public async Task DeleteAsync(DeleteTodoCommand input)
        {
           await _mediatr.Send(input);
        }

        public async Task<TodoDto> GetAsync(GetTodoQuery input)
        {
            return await _mediatr.Send(input);
        }

        public async Task<List<TodoDto>> GetListAsync(GetTodoListQuery input)
        {
            return await _mediatr.Send(input);
        }

        public async Task<TodoDto> UpdateAsync(UpdateTodoCommand input)
        {
            return await _mediatr.Send(input);
        }
    }
}
