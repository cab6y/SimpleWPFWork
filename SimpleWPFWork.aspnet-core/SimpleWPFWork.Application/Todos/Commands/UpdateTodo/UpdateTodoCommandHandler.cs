using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo;
using SimpleWPFWork.EntityFrameworkCore;

namespace SimpleWPFWork.Application.Todos.Commands.UpdateTodo
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, TodoDto>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTodoCommandHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoDto> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var gettodo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);
            gettodo.Title = request.Title;
            gettodo.Description = request.Description;
            gettodo.IsCompleted = request.IsCompleted;
            gettodo.Priority = request.Priority;
            gettodo.DueDate = request.DueDate;
            gettodo.CategoryId = request.CategoryId;
            gettodo.Username = request.Username;
            //_context.Todos.Update(gettodo);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TodoDto>(gettodo);
        }
    }
}