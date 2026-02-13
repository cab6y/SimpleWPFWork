using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;

namespace SimpleWPFWork.ApplicationContracts.Todos
{
    public interface ITodoAppService
    {
        Task<TodoDto> CreateAsync(CreateTodoCommand input);
        Task<TodoDto> UpdateAsync(UpdateTodoCommand input);
        Task DeleteAsync(DeleteTodoCommand input);
        Task<List<TodoDto>> GetListAsync(GetTodoListQuery input);
        Task<TodoDto> GetAsync(GetTodoQuery input);
    }
}
