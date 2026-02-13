using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Todos.Commands.UpdateTodo
{
    public class UpdateTodoCommand : IRequest<TodoDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Username { get; set; }
    }
}
