using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Todos.Commands.DeleteTodo
{
    public class DeleteTodoCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
