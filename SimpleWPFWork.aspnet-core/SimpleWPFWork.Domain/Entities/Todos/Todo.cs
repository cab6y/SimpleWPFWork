using SimpleWPFWork.Domain.Entities.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Domain.Entities.Todos
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }
        public string CategoryId { get; set; }
        public string Username { get; set; }
        public Category Category { get; set; }
    }
}
