using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace SimpleWPFWork.EntityFrameworkCore.Seed.Procedures.Todos
{
    public static class TodoProcedureSeed
    {
        public static async Task SeedAsync(SimpleWPFWorkDbContext context)
        {
            Console.WriteLine("  ⏳ Seeding Todo stored procedures...");

            try
            {
                // GetFilteredTodos - Varsa sil
                await context.Database.ExecuteSqlRawAsync(@"
                    IF EXISTS (SELECT * FROM sys.objects 
                               WHERE object_id = OBJECT_ID(N'[dbo].[GetFilteredTodos]') 
                               AND type in (N'P', N'PC'))
                    BEGIN
                        DROP PROCEDURE [dbo].[GetFilteredTodos]
                    END
                ");

                // GetFilteredTodos - Oluştur
                await context.Database.ExecuteSqlRawAsync(@"
                    CREATE PROCEDURE [dbo].[GetFilteredTodos]
                        @Title NVARCHAR(MAX) = NULL,
                        @Description NVARCHAR(MAX) = NULL,
                        @IsCompleted BIT = NULL,
                        @Priority NVARCHAR(50) = NULL,
                        @DueDate DATETIMEOFFSET = NULL,
                        @CategoryId UNIQUEIDENTIFIER = NULL,
                        @Username NVARCHAR(255) = NULL,
                        @Page INT = 0,
                        @Limit INT = 100
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        DECLARE @Offset INT = @Page * @Limit;

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
                            LastModificationTime
                        FROM Todos
                        WHERE 
                            (@Title IS NULL OR Title LIKE '%' + @Title + '%')
                            AND (@Description IS NULL OR Description LIKE '%' + @Description + '%')
                            AND (@IsCompleted IS NULL OR IsCompleted = @IsCompleted)
                            AND (@Priority IS NULL OR Priority = @Priority)
                            AND (@DueDate IS NULL OR DueDate = @DueDate)
                            AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                            AND (@Username IS NULL OR Username LIKE '%' + @Username + '%')
                            AND IsDeleted = 0
                        ORDER BY DueDate DESC
                        OFFSET @Offset ROWS
                        FETCH NEXT @Limit ROWS ONLY;
                    END
                ");

                Console.WriteLine("  ✅ GetFilteredTodos procedure created successfully");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ❌ Error: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}