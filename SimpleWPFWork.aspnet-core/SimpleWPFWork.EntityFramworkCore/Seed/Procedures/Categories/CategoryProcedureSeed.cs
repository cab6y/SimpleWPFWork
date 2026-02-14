using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleWPFWork.EntityFramworkCore.Seed.Procedures.Categories
{
    public static class CategoryProcedureSeed
    {
        public static async Task SeedAsync(SimpleWPFWorkDbContext context)
        {
            Console.WriteLine("  ⏳ Seeding Category stored procedures...");

            try
            {
                // GetFilteredCategories - Varsa sil
                await context.Database.ExecuteSqlRawAsync(@"
                    IF EXISTS (SELECT * FROM sys.objects 
                               WHERE object_id = OBJECT_ID(N'[dbo].[GetFilteredCategories]') 
                               AND type in (N'P', N'PC'))
                    BEGIN
                        DROP PROCEDURE [dbo].[GetFilteredCategories]
                    END
                ");

                // GetFilteredCategories - Oluştur
                await context.Database.ExecuteSqlRawAsync(@"
                    CREATE PROCEDURE [dbo].[GetFilteredCategories]
                        @Name NVARCHAR(50) = NULL,
                        @Color NVARCHAR(7) = NULL,
                        @Page INT = 0,
                        @Limit INT = 100
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        DECLARE @Offset INT = @Page * @Limit;

                        SELECT 
                            Id,
                            Name,
                            Color,
                            CreationTime,
                            LastModificationTime,
                            DeleterUserId,
                            DeletionTime,
                            IsDeleted
                        FROM Categories
                        WHERE 
                            (@Name IS NULL OR Name LIKE '%' + @Name + '%')
                            AND (@Color IS NULL OR Color = @Color)
                            AND IsDeleted = 0
                        ORDER BY Name ASC
                        OFFSET @Offset ROWS
                        FETCH NEXT @Limit ROWS ONLY;
                    END
                ");

                Console.WriteLine("  ✅ GetFilteredCategories procedure created successfully");
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
