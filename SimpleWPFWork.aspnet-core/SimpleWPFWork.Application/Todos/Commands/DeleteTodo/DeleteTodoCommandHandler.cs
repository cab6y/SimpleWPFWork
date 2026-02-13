using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.Domain.Entities.Todos;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Application.Todos.Commands.DeleteTodo
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly SimpleWPFWorkDbContext _context;
        private readonly IMapper _mapper;

        public DeleteTodoCommandHandler(
            SimpleWPFWorkDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);

            if (todo == null)
                throw new Exception($"Kategori bulunamadı: {request.Id}");

            _context.Todos.Remove(todo); // Soft delete (interceptor sayesinde)
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
