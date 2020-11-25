using System;
using System.Linq.Expressions;
using LinqKit;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealListDto
    {
        public int Id { get; set; }
        public string DealNumber { get; set; }
        public string DealStatusName { get; set; }
        public string CounterpartyName { get; set; }
        public int DealCategoryId { get; set; }
        public string DealCategoryName { get; set; }
        public string DealTypeName { get; set; }
        public bool ForceMajeure { get; set; }
        public string AssignedTo { get; set; }
        public bool Executed { get; set; }
        /// <summary>
        /// whether the deal is in a status that it can be executed or have its execution cancelled
        /// </summary>
        public bool IsExecutionStatus { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string CreationUserName { get; set; }
        public bool IsFinalized { get; set; }
        public bool DealAssignedToCurrentUserOrHisRole { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<Deal, DealListDto>> ProjectionFromEntity(int userId)
        {
            return entity => new DealListDto()
            {
                Id = entity.Id,
                DealNumber = entity.DealNumber,
                DealStatusName = (entity.CurrentDealWorkflowStatus != null ? entity.CurrentDealWorkflowStatus.WorkflowStatus.Name : "No Status"),
                CounterpartyName = entity.Counterparty.Name,
                DealCategoryId = entity.DealCategory.Id,
                DealCategoryName = entity.DealCategory.Name,
                DealTypeName = entity.DealType.Name,
                ForceMajeure = entity.ForceMajeure,
                AssignedTo = Deal.AssignedToDescription.Invoke(entity),
                Executed = entity.Executed,
                IsExecutionStatus = (entity.CurrentDealWorkflowStatus != null ? entity.CurrentDealWorkflowStatus.WorkflowStatus.AllowsDealExecution : false),
                CreationDate = entity.CreationDate,
                CreationUserName = entity.CreationUser.Name,
                IsFinalized = entity.CurrentDealWorkflowStatus.Finalized,
                DealAssignedToCurrentUserOrHisRole = Deal.AssignedToSpecificUser.Invoke(entity, userId) || Deal.AssignedToUsersRole.Invoke(entity, userId),
            };
        }
    }
}
