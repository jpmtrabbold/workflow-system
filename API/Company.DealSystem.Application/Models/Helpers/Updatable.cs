using LinqExpander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Models.Helpers
{
    public class Updatable<T>
    {
        public bool Updated { get; set; }
        public T Value { get; set; }
    }
    public class UpdatableWithPrevious<T> : Updatable<T>
    {
        public T PreviousValue { get; set; }

    }

    public static class Updatable
    {
        /// <summary>
        /// Creates an updatable object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Updatable<T> Create<T>(T value)
        {
            return new Updatable<T>() { Value = value };
        }

        public static Updatable<T> Create<T>(T value, bool updated)
        {
            return new Updatable<T>() { Value = value, Updated = updated };
        }

        public static UpdatableWithPrevious<T> CreateWithPrevious<T>(T value)
        {
            return new UpdatableWithPrevious<T>() { Value = value, PreviousValue = value };
        }

        public static Updatable<T?> CreateNullable<T>(T value, bool updated) where T : struct
        {
            return new Updatable<T?>() { Value = value, Updated = updated };
        }

        public static Updatable<T?> CreateNullable<T>(T value) where T: struct
        {
            return new Updatable<T?>() { Value = value };
        }

        /// <summary>
        /// Checks whether the Updatable field is not null and was indeed updated or not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updatable"></param>
        /// <returns></returns>
        public static bool IsUpdated<T>(Updatable<T> updatable)
        {
            return (updatable != null && updatable.Updated);
        }

        public static bool IsUpdatedButEmpty(Updatable<decimal?> updatable)
        {
            return IsUpdated(updatable) && (!updatable.Value.HasValue || updatable.Value == 0);
        }

        public static bool IsUpdatedButEmpty(Updatable<decimal> updatable)
        {
            return IsUpdated(updatable) && updatable.Value == 0;
        }

        public static bool IsUpdatedButEmpty(Updatable<int?> updatable)
        {
            return IsUpdated(updatable) && (!updatable.Value.HasValue || updatable.Value == 0);
        }

        public static bool IsUpdatedButEmpty(Updatable<int> updatable)
        {
            return IsUpdated(updatable) && updatable.Value == 0;
        }

        public static bool IsUpdatedButEmpty(Updatable<string> updatable)
        {
            return IsUpdated(updatable) && string.IsNullOrWhiteSpace(updatable.Value);
        }

        public static bool IsUpdatedButEmpty(Updatable<DateTime> updatable)
        {
            return IsUpdated(updatable) && updatable.Value == default;
        }

        public static bool IsUpdatedButEmpty(Updatable<DateTime?> updatable)
        {
            return IsUpdated(updatable) && (!updatable.Value.HasValue || updatable.Value == default);
        }

        public static bool IsUpdatedButEmpty(Updatable<DateTimeOffset> updatable)
        {
            return IsUpdated(updatable) && updatable.Value == default;
        }

        public static bool IsUpdatedButEmpty(Updatable<DateTimeOffset?> updatable)
        {
            return IsUpdated(updatable) && (!updatable.Value.HasValue || updatable.Value == default);
        }

        /// <summary>
        /// saves the Dto list changes (insertions, updates and deletes) to its corresponding Entity list
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dtoList"></param>
        /// <param name="entityCollection"></param>
        internal static void ToEntityCollection<TDto, TEntity, TService>(List<TDto> dtoList, ICollection<TEntity> entityCollection, TService service)
            where TDto : UpdatableListItemDto, IPersistableDto<TEntity, TService>
            where TEntity : BaseEntity
            where TService : BaseService
        {
            if (dtoList == null || entityCollection == null)
                return;
            
            foreach (var dtoItem in dtoList)
            {
                if (!dtoItem.Id.HasValue)
                {
                    if (!dtoItem.Deleted)
                    {
                        entityCollection.Add(dtoItem.ToEntity(null, service));
                    }
                }
                else if (dtoItem.Deleted || dtoItem.Updated)
                {
                    var entity = entityCollection.FirstOrDefault(e => e.Id == dtoItem.Id.Value);

                    if (entity != null) // ignores if null, because other user might have deleted this entry
                    {
                        if (dtoItem.Deleted)
                        {
                            service._repo.Remove(entity);
                        }
                        else
                        {
                            dtoItem.ToEntity(entity, service);
                        }
                    }
                }
            }
        }
    }
}
