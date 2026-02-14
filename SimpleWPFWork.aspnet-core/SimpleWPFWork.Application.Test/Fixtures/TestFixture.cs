using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleWPFWork.EntityFrameworkCore;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.Domain.Entities.Todos;
using System;
using System.IO;

namespace SimpleWPFWork.Application.Test.Fixtures
{
    public class TestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; }

        public TestFixture()
        {
            var services = new ServiceCollection();

            // Host projesinin bulunduğu dizini bul
            var currentDirectory = Directory.GetCurrentDirectory();
            var projectRoot = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;
            var hostProjectPath = Path.Combine(projectRoot!, "SimpleWPFWork.Host");

            if (!Directory.Exists(hostProjectPath))
            {
                throw new DirectoryNotFoundException($"Host project not found at: {hostProjectPath}");
            }

            var appsettingsPath = Path.Combine(hostProjectPath, "appsettings.json");

            if (!File.Exists(appsettingsPath))
            {
                throw new FileNotFoundException($"appsettings.json not found at: {appsettingsPath}");
            }

            Console.WriteLine($"Reading configuration from: {appsettingsPath}");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(hostProjectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Connection string'i farklı key'lerle dene
            var connectionString =
                configuration.GetConnectionString("DefaultConnection") ??
                configuration.GetConnectionString("Default") ??
                configuration["ConnectionStrings:DefaultConnection"] ??
                configuration["ConnectionStrings:Default"];

            
            Console.WriteLine($"Connection String: {connectionString}");

            // SQL Server Database
            services.AddDbContext<SimpleWPFWorkDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<SimpleWPFWork.Application.Categories.Commands.CreateCategory.CreateCategoryCommandHandler>();
            });

            // AutoMapper
            services.AddAutoMapper(
                typeof(SimpleWPFWork.Application.Categories.Commands.CreateCategory.CreateCategoryCommandHandler).Assembly
            );

            // Repositories
            services.AddScoped<ICategoryRepository,
                SimpleWPFWork.EntityFramworkCore.Repositories.Categories.EFCoreCategoryRepository>();

            services.AddScoped<ITodoRepository,
                SimpleWPFWork.EntityFramworkCore.Repositories.Todos.EFCoreTodoRepository>();

            ServiceProvider = services.BuildServiceProvider();

            // Database'i migrate et
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SimpleWPFWorkDbContext>();

            try
            {
                context.Database.Migrate();
                Console.WriteLine("Database migrated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}