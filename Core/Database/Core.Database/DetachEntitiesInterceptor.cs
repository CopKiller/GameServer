using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Core.Database;

internal class DetachEntitiesInterceptor : SaveChangesInterceptor
{
        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context != null)
            {
                // Detach all tracked entities after saving changes
                foreach (var entry in context.ChangeTracker.Entries())
                {
                    context.Entry(entry.Entity).State = EntityState.Detached;
                }
            }

            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
}