using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.Domain.Entities.Todos;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SimpleWPFWork.EntityFramworkCore.Repositories.Todos
{
    public class EFCoreTodoRepository : ITodoRepository
    {
        private readonly SimpleWPFWorkDbContext _context;

        public EFCoreTodoRepository(SimpleWPFWorkDbContext context)
        {
            _context = context;
        }

        public async Task<List<Todo>> GetFilteredAsync(
            string? title = null,
            string? description = null,
            bool? isCompleted = null,
            string? priority = null,
            DateTimeOffset? dueDate = null,
            Guid? categoryId = null,
            string? username = null,
            int page = 0,
            int limit = 100)
        {
            var todos = new List<Todo>();

            var connection = _context.Database.GetDbConnection();

            await using var command = connection.CreateCommand();
            command.CommandText = "GetFilteredTodos";
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add(new SqlParameter("@Title", (object?)title ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Description", (object?)description ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@IsCompleted", (object?)isCompleted ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Priority", (object?)priority ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@DueDate", (object?)dueDate ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@CategoryId", (object?)categoryId ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Username", (object?)username ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Page", page));
            command.Parameters.Add(new SqlParameter("@Limit", limit));

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    IsCompleted = reader.GetBoolean(reader.GetOrdinal("IsCompleted")),
                    Priority = reader.GetString(reader.GetOrdinal("Priority")),
                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                    CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    CreationTime = reader.GetDateTime(reader.GetOrdinal("CreationTime")),
                    LastModificationTime = reader.GetDateTime(reader.GetOrdinal("LastModificationTime"))
                });
            }

            return todos;
        }
        public async Task<Todo> UpdateAsync(Todo todo)
        {
            var connection = _context.Database.GetDbConnection() as SqlConnection;

            var sql = @"
                UPDATE Todos 
                SET 
                    Title = @Title,
                    Description = @Description,
                    IsCompleted = @IsCompleted,
                    Priority = @Priority,
                    DueDate = @DueDate,
                    CategoryId = @CategoryId,
                    Username = @Username,
                    LastModificationTime = @LastModificationTime
                WHERE Id = @Id;

                SELECT 
                    Id,
                    Title,
                    Description,
                    IsCompleted,
                    Priority,
                    DueDate,
                    CategoryId,
                    Username,
                    CreationTime,
                    LastModificationTime,
                    DeleterUserId,
                    DeletionTime,
                    IsDeleted
                FROM Todos 
                WHERE Id = @Id;
            ";

            var parameters = new
            {
                todo.Id,
                todo.Title,
                todo.Description,
                todo.IsCompleted,
                todo.Priority,
                todo.DueDate,
                todo.CategoryId,
                todo.Username,
                LastModificationTime = DateTime.Now
            };

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var updatedTodo = await connection.QuerySingleAsync<Todo>(sql, parameters);

            return updatedTodo;
        }
    }
}