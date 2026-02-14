using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleWPFWork.EntityFrameworkCore;
using SimpleWPFWork.EntityFrameworkCore.Seed.Procedures.Todos;

Console.WriteLine("=== Database Migration & Seeding ===");
Console.WriteLine();

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
    // 1️⃣ Veritabanını en son migration'a güncelle
    Console.WriteLine("Applying migrations...");
    context.Database.Migrate();
    Console.WriteLine("Database migrated successfully!");
    Console.WriteLine();

    // 2️⃣ Stored Procedures seed et
    Console.WriteLine("Seeding stored procedures...");
    await TodoProcedureSeed.SeedAsync(context);
    Console.WriteLine("Stored procedures seeded successfully!");
    Console.WriteLine();
}

Console.WriteLine("All done!");
Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();