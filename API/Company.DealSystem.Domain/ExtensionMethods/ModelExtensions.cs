using System.Linq;
using Company.DealSystem.Domain.Entities;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelExtensions
    {
        public static IQueryable<T> OnlyActive<T>(this DbSet<T> entities) where T : DeactivatableBaseEntity
        {
            return Queryable.Where(entities, e => e.Active);
        }
        public static IQueryable<T> OnlyActive<T>(this IQueryable<T> entities) where T : DeactivatableBaseEntity
        {
            return entities.Where(e => e.Active);
        }


    }
}
