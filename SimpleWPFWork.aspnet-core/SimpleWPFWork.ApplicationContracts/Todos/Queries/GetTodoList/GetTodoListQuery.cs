using MediatR;
using System;
using System.Collections.Generic;

namespace SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList
{
    public class GetTodoListQuery : IRequest<List<TodoDto>>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }      // ✅ bool? (nullable)
        public string? Priority { get; set; }          // ✅ int? (string değil!)
        public DateTime? DueDate { get; set; }      // ✅ DateTime? (nullable)
        public Guid? CategoryId { get; set; }       // ✅ Guid? (nullable)
        public string? Username { get; set; }
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 0;
    }
}