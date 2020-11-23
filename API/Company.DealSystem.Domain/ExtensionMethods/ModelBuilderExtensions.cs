using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Company.DealSystem.Domain.Entities;

namespace Company.DealSystem.Domain.ExtensionMethods
{
    public static class ModelBuilderExtensions
    {
        public static int HasData<TEntity>(this ModelBuilder mb, TEntity entity) where TEntity : class
        {
            mb.Entity<TEntity>().HasData(entity);

            if (entity is BaseEntity baseEntity)
            {
                return baseEntity.Id;
            }

            return 0;
        }
    }
}
