using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.Domain.Entities.Todos;
using System;

namespace SimpleWPFWork.EntityFrameworkCore
{
    public class SimpleWPFWorkDbContext : DbContext
    {
        public SimpleWPFWorkDbContext(DbContextOptions<SimpleWPFWorkDbContext> options)
            : base(options)
        {
        }

        // DbSet'ler
        public DbSet<Category> Categories { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category yapılandırması
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(7); // #FF5733 formatı için
            });

            // Todo yapılandırması
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);

                // Category ile ilişki
                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                // İndeksler
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.Username);
                entity.HasIndex(e => e.IsCompleted);
                entity.HasIndex(e => e.DueDate);
            });

            // Seed data (opsiyonel - başlangıç verileri)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Sabit Guid'ler (her migration'da aynı olsun)
            var isKategoriId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var kisiselKategoriId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var alisverisKategoriId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Örnek kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = isKategoriId,
                    Name = "İş",
                    Color = "#FF5733",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1)
                },
                new Category
                {
                    Id = kisiselKategoriId,
                    Name = "Kişisel",
                    Color = "#3498DB",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1)
                },
                new Category
                {
                    Id = alisverisKategoriId,
                    Name = "Alışveriş",
                    Color = "#2ECC71",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1)
                }
            );

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Priority).HasMaxLength(20);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);

                entity.HasOne(e => e.Category)
                    .WithMany()
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.Username);
                entity.HasIndex(e => e.IsCompleted);
                entity.HasIndex(e => e.DueDate);
            });
        }
    }
}