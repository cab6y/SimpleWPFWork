using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;
using SimpleWPFWork.Domain.Entities.Todos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWPFWork.Application.Todos.Queries.GetTodoList
{
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, List<TodoDto>>
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(
            ITodoRepository todoRepository,
            IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<List<TodoDto>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            // ✅ Repository ile Stored Procedure çağrısı
            var todos = await _todoRepository.GetFilteredAsync(
                title: request.Title,
                description: request.Description,
                isCompleted: request.IsCompleted,
                priority: request.Priority,
                dueDate: request.DueDate,
                categoryId: request.CategoryId,
                username: request.Username,
                page: request.Page,
                limit: request.Limit);

            return _mapper.Map<List<TodoDto>>(todos);
        }
    }
}