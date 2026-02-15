using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodo;
using SimpleWPFWork.Domain.Entities.Todos;
using SimpleWPFWork.EntityFrameworkCore;

namespace SimpleWPFWork.Application.Todos.Queries.GetTodo
{
    internal class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, TodoDto>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public GetTodoQueryHandler(
            ITodoRepository todoRepository,
        IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<TodoDto> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.GetAsync(request.Id);

            if (todo == null)
                throw new Exception($"Todo bulunamadı: {request.Id}");

            return _mapper.Map<TodoDto>(todo);
        }
    }
}