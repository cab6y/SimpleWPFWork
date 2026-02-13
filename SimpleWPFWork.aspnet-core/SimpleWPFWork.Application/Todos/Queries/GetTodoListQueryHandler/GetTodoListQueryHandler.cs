using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList;
using SimpleWPFWork.EntityFrameworkCore;

namespace SimpleWPFWork.Application.Todos.Queries.GetTodoList
{
    public class GetTodoListQueryHandler : IRequestHandler<GetTodoListQuery, List<TodoDto>>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoListQueryHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TodoDto>> Handle(GetTodoListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
                query = query.Where(x => x.Title.Contains(request.Title));

            if (!string.IsNullOrWhiteSpace(request.Description))
                query = query.Where(x => x.Description.Contains(request.Description));

            if (request.IsCompleted.HasValue) 
                query = query.Where(x => x.IsCompleted == request.IsCompleted.Value);

            if (!string.IsNullOrEmpty(request.Priority)) 
                query = query.Where(x => x.Priority == request.Priority);

            if (request.DueDate.HasValue) 
                query = query.Where(x => x.DueDate== request.DueDate);

            if (request.CategoryId.HasValue)
                query = query.Where(x => x.CategoryId == request.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(request.Username))
                query = query.Where(x => x.Username == request.Username);

            // Pagination (Skip/Take)
            var todos = await query
                .OrderByDescending(x => x.DueDate)
                .Skip(request.Page * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<TodoDto>>(todos);
        }
    }
}