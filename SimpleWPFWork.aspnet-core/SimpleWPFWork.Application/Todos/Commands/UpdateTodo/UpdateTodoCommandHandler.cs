using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.Domain.Entities.Todos;

namespace SimpleWPFWork.Application.Todos.Commands.UpdateTodo
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoDto>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public UpdateTodoCommandHandler(
            ITodoRepository todoRepository,
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<TodoDto> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = new Todo
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                Priority = request.Priority,
                DueDate = request.DueDate,
                CategoryId = request.CategoryId,
                Username = request.Username
            };

            var updatedTodo = await _todoRepository.UpdateAsync(todo);

            return _mapper.Map<TodoDto>(updatedTodo);
        }
    }
}