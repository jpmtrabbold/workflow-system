using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;

namespace Company.WorkflowSystem.Service.Models.Dtos.Users
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string UserRoleName { get; set; }
        public List<string> WorkflowRoleNames { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<User, UserListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserListDto()
                {
                    Id = entity.Id,
                    Username = entity.Username,
                    Name = entity.Name,
                    UserRoleName = (entity.UserRoleId.HasValue ? entity.UserRole.Name : ""),
                    WorkflowRoleNames = entity.WorkflowRolesInUser.AsQueryable().Where(wru => wru.Active).Select(wu => wu.WorkflowRole.Name).ToList(),
                    Active = entity.Active
                };
            }
        }
    }
}
