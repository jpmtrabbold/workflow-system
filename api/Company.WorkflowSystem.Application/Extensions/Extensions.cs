using Microsoft.EntityFrameworkCore;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Company.WorkflowSystem.Application.Extensions
{
    internal static class Extensions
    {
        public static IAsyncEnumerable<TEntity> AsAsyncEnumerable<TEntity>(this DbSet<TEntity> obj) where TEntity : class
        {
            return EntityFrameworkQueryableExtensions.AsAsyncEnumerable(obj);
        }
        public static IQueryable<TEntity> Where<TEntity>(this DbSet<TEntity> obj, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Queryable.Where(obj, predicate);
        }

        public async static Task<TEntity> SingleAsync<TEntity>(this IQueryable<TEntity> obj, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await EntityFrameworkQueryableExtensions.SingleAsync(obj, cancellationToken);
        }

        public async static Task<TSource> SingleAsync<TSource>([NotNullAttribute] this IQueryable<TSource> obj, [NotNullAttribute] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await EntityFrameworkQueryableExtensions.SingleAsync(obj, predicate, cancellationToken);
        }
        public async static Task<TEntity> SingleAsync<TEntity>(this DbSet<TEntity> obj, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await EntityFrameworkQueryableExtensions.SingleAsync(obj, predicate, cancellationToken);
        }

        public async static Task<TSource> FirstOrDefaultAsync<TSource>([NotNullAttribute] this IQueryable<TSource> source, [NotNullAttribute] Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(source, predicate, cancellationToken);
        }
        public async static Task<TEntity> FirstOrDefaultAsync<TEntity>([NotNullAttribute] this DbSet<TEntity> obj, [NotNullAttribute] Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(obj, predicate, cancellationToken);
        }
        public async static Task<int> CountAsync<TEntity>(this DbSet<TEntity> obj, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await EntityFrameworkQueryableExtensions.CountAsync(obj, cancellationToken);
        }
        public async static Task<TEntity> FirstAsync<TEntity>(this DbSet<TEntity> obj, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TEntity : class
        {
            return await EntityFrameworkQueryableExtensions.FirstAsync(obj, predicate, cancellationToken);
        }


    }
}
