using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Company.WorkflowSystem.Domain.Entities
{
    public partial class Deal : BaseEntity
    {
        /// <summary>
        /// Returns if the deal is assigned to that specific userId
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Expression<Func<Deal, int, bool>> AssignedToSpecificUser
        {
            get => (entity, userId) => entity.CurrentDealWorkflowStatusId.HasValue &&
                entity.CurrentDealWorkflowStatus != null &&
                entity.CurrentDealWorkflowStatus.AssigneeUserId.HasValue &&
                entity.CurrentDealWorkflowStatus.AssigneeUserId == userId;
        }

        /// <summary>
        /// returns a description for the deal's assignment, like "Assigned to [User]" or "Assigned to [Workflow Role]"
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Expression<Func<Deal, string>> AssignedToDescription
        {
            get => entity => (!entity.CurrentDealWorkflowStatusId.HasValue || entity.CurrentDealWorkflowStatus == null ?
                 "No one" :
                 entity.CurrentDealWorkflowStatus.AssigneeUserId.HasValue && entity.CurrentDealWorkflowStatus.AssigneeUser != null ?
                 entity.CurrentDealWorkflowStatus.AssigneeUser.Name :
                 entity.CurrentDealWorkflowStatus.AssigneeWorkflowRoleId.HasValue && entity.CurrentDealWorkflowStatus.AssigneeWorkflowRole != null ?
                 entity.CurrentDealWorkflowStatus.AssigneeWorkflowRole.Name :
                 "No one");
        }

        /// <summary>
        /// Checks whether userId participated on this deal at all
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Expression<Func<Deal, int, bool>> UserParticipatedOnThisDeal
        {
            get => (entity, userId) => entity.DealWorkflowStatuses.Any(s => s.AssigneeUserId == userId);
        }

        /// <summary>
        /// Returns whether the deal is assigned to userId's workflow role
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Expression<Func<Deal, int, bool>> AssignedToUsersRole
        {
            get => (entity, userId) => entity.CurrentDealWorkflowStatusId.HasValue &&
                 entity.CurrentDealWorkflowStatus != null &&
                 entity.CurrentDealWorkflowStatus.AssigneeWorkflowRoleId.HasValue &&
                 entity.CurrentDealWorkflowStatus.AssigneeWorkflowRole.UsersInWorkflowRole.Any(uw => uw.Active && uw.UserId == userId);
        }

    }
}
