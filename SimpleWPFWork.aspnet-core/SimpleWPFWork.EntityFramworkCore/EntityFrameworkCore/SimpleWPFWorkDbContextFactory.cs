using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SimpleWPFWork.EntityFrameworkCore;
using System.IO;

namespace SimpleWPFWork.EntityFrameworkCore 
{
    public class SimpleWPFWorkDbContextFactory : IDesignTimeDbContextFactory<SimpleWPFWorkDbContext>
    {
        public SimpleWPFWorkDbContext CreateDbContext(string[] args)
        {
            // appsettings.json'u oku
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<SimpleWPFWorkDbContext>();
            builder.UseSqlServer(connectionString);

            return new SimpleWPFWorkDbContext(builder.Options);
        }
    }
}