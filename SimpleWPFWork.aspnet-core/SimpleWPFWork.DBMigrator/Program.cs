// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleWPFWork.EntityFrameworkCore;
var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = configurationBuilder
           .AddEnvironmentVariables()
           .Build();

var connectionString = configuration.GetConnectionString("Default");


var optionsBuilder = new DbContextOptionsBuilder<SimpleWPFWorkDbContext>();
optionsBuilder.UseSqlServer(connectionString);

using (var context = new SimpleWPFWorkDbContext(optionsBuilder.Options))
{
    // Veritabanını en son migration'a güncelle
    context.Database.Migrate();
    Console.WriteLine("Veritabanı başarıyla güncellendi!");
    Console.WriteLine();
}
