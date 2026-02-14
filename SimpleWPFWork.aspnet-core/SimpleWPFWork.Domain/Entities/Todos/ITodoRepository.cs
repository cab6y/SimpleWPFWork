using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.Domain.Entities.Todos
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetFilteredAsync(
             string? title = null,
             string? description = null,
             bool? isCompleted = null,
             string? priority = null,
             DateTimeOffset? dueDate = null,
             Guid? categoryId = null,
             string? username = null,
             int page = 0,
             int limit = 100);
        Task<Todo> UpdateAsync(Todo todo);
    }
}
