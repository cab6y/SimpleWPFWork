using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SimpleWPFWork.Domain.Entities;

namespace SimpleWPFWork.EntityFrameworkCore.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            ApplySoftDeleteConcepts(eventData.Context);
            ApplyAuditConcepts(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            ApplySoftDeleteConcepts(eventData.Context);
            ApplyAuditConcepts(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplySoftDeleteConcepts(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletionTime = DateTime.UtcNow;
                        break;

                    case EntityState.Added:
                        entry.Entity.IsDeleted = false;
                        break;
                }
            }
        }

        private void ApplyAuditConcepts(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationTime = DateTime.UtcNow;
                        entry.Entity.LastModificationTime = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModificationTime = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}