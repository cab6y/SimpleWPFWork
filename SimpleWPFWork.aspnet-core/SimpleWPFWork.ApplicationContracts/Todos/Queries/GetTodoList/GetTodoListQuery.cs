using MediatR;
using SimpleWPFWork.ApplicationContracts.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.ApplicationContracts.Todos.Queries.GetTodoList
{
    public class GetTodoListQuery : IRequest<List<CategoryDto>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CategoryId { get; set; }
        public string Username { get; set; }
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 0;
    }
}
