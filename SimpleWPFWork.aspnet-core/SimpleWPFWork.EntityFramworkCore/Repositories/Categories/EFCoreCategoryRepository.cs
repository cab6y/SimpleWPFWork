using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SimpleWPFWork.EntityFramworkCore.Repositories.Categories
{
    public class EFCoreCategoryRepository : ICategoryRepository
    {
        private readonly SimpleWPFWorkDbContext _context;

        public EFCoreCategoryRepository(SimpleWPFWorkDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetFilteredAsync(
           string? name = null,
           string? color = null,
           int page = 0,
           int limit = 100)
        {
            var categories = new List<Category>();

            var connection = _context.Database.GetDbConnection();

            await using var command = connection.CreateCommand();
            command.CommandText = "GetFilteredCategories";
            command.CommandType = CommandType.StoredProcedure;

            // Parametreler
            command.Parameters.Add(new SqlParameter("@Name", (object?)name ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Color", (object?)color ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Page", page));
            command.Parameters.Add(new SqlParameter("@Limit", limit));

            // Connection aç
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            // Execute ve map
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Color = reader.GetString(reader.GetOrdinal("Color")),
                    CreationTime = reader.GetDateTime(reader.GetOrdinal("CreationTime")),
                    LastModificationTime = reader.GetDateTime(reader.GetOrdinal("LastModificationTime")),
                    DeleterUserId = reader.IsDBNull(reader.GetOrdinal("DeleterUserId"))
                        ? null
                        : reader.GetGuid(reader.GetOrdinal("DeleterUserId")),
                    DeletionTime = reader.IsDBNull(reader.GetOrdinal("DeletionTime"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("DeletionTime")),
                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                });
            }

            return categories;
        }
    }
}
