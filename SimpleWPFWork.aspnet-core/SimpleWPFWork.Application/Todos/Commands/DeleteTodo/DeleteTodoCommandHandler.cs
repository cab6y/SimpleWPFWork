using AutoMapper;
using MediatR;
using SimpleWPFWork.ApplicationContracts.Todos;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.CreateTodo;
using SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo;
using SimpleWPFWork.Domain.Entities.Todos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Application.Todos.Commands.DeleteTodo
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly ITodoRepository _todoRepository;

        public DeleteTodoCommandHandler(
            ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            await _todoRepository.DeleteAsync(request.Id);
        }
    }
}
