using Microsoft.EntityFrameworkCore;
using SimpleWPFWork.Domain.Entities;
using SimpleWPFWork.Domain.Entities.Categories;
using SimpleWPFWork.Domain.Entities.Todos;
using SimpleWPFWork.EntityFrameworkCore.Interceptors;
using System;
using System.Linq.Expressions;

namespace SimpleWPFWork.EntityFrameworkCore
{
    public class SimpleWPFWorkDbContext : DbContext
    {
        public SimpleWPFWorkDbContext(DbContextOptions<SimpleWPFWorkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Interceptor'ı ekle
            optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category yapılandırması
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(7);
            });

            // Todo yapılandırması (TEK SEFER)
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

            // Soft Delete Query Filter
            ApplySoftDeleteQueryFilter(modelBuilder);

            // Seed Data
            SeedData(modelBuilder);
        }

        private void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var filter = Expression.Lambda(Expression.Not(property), parameter);

                    entityType.SetQueryFilter(filter);
                }
            }
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Sabit Guid'ler
            // SABİT BİR NESNEYE GENERIC GUID ATANARAK DA YÖNETİLEBİLİRDİ BÖYLE KOLAYIMA GELDİ.
            var isKategoriId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var kisiselKategoriId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var alisverisKategoriId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Kategoriler
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = isKategoriId,
                    Name = "İş",
                    Color = "#FF5733",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1),
                    IsDeleted = false
                },
                new Category
                {
                    Id = kisiselKategoriId,
                    Name = "Kişisel",
                    Color = "#3498DB",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1),
                    IsDeleted = false
                },
                new Category
                {
                    Id = alisverisKategoriId,
                    Name = "Alışveriş",
                    Color = "#2ECC71",
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1),
                    IsDeleted = false
                }
            );

            // Todo'lar
            modelBuilder.Entity<Todo>().HasData(
                new Todo
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Title = "Proje sunumu hazırla",
                    Description = "Q1 sonuçları için sunum",
                    Priority = "High",
                    DueDate = new DateTime(2025, 2, 15),
                    Username = "admin",
                    CategoryId = isKategoriId,
                    IsCompleted = false,
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1),
                    IsDeleted = false
                },
                new Todo
                {
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Title = "Market alışverişi",
                    Description = "Süt, ekmek, yumurta",
                    Priority = "Normal",
                    DueDate = new DateTime(2025, 2, 13),
                    Username = "admin",
                    CategoryId = alisverisKategoriId,
                    IsCompleted = false,
                    CreationTime = new DateTime(2025, 1, 1),
                    LastModificationTime = new DateTime(2025, 1, 1),
                    IsDeleted = false
                }
            );
        }
    }
}