using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Util;

namespace Company.DealSystem.Application.Models.ViewModels.Shared
{
    public class LookupRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }

        internal static Expression<Func<DealType, LookupRequest>> ProjectionFromDealType
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Active = entity.Active,
                };
            }
        }

        internal static Expression<Func<DealCategory, LookupRequest>> ProjectionFromDealCategory
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Active = entity.Active,
                };
            }
        }

        internal static Expression<Func<Counterparty, LookupRequest>> ProjectionFromCounterparty
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Active = entity.Active && DateUtils.GetDateTimeOffsetNow() < entity.ExpiryDate && entity.ApprovalDate.HasValue,
                };
            }
        }

        internal static Expression<Func<Product, LookupRequest>> ProjectionFromProduct
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Active = entity.Active,
                };
            }
        }

        internal static Expression<Func<DealItemFieldset, LookupRequest>> ProjectionFromDealItemFieldset
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                };
            }
        }

        internal static Expression<Func<WorkflowSet, LookupRequest>> ProjectionFromWorkflowSet
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Active = entity.Active,
                };
            }
        }

        internal static Expression<Func<WorkflowRole, LookupRequest>> ProjectionFromWorkflowRole
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = (entity.ApprovalLevel.HasValue ? $"Approval level: {entity.ApprovalLevel}" : "No approval level"),
                };
            }
        }

        internal static Expression<Func<UserRole, LookupRequest>> ProjectionFromUserRole
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Active = entity.Active,
                };
            }
        }

        internal static Expression<Func<Country, LookupRequest>> ProjectionFromCountry
        {
            get
            {
                return entity => new LookupRequest()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Code,
                };
            }
        }
    }
}
