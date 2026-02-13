using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;

namespace SimpleWPFWork.ApplicationContracts.Todos
{
    public interface ITodoAppService
    {
        Task<List<TodoDto>> GetAllAsync(GetTodoListQuery input);
        Task<TodoDto> GetByIdAsync(GetTodoQuery input);
        Task CreateAsync(CreateTodoCommand input);
        Task UpdateAsync(UpdateTodoCommand input);
        Task DeleteAsync(DeleteTodoCommand input);
    }
}
