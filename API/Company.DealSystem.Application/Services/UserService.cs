using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Infrastructure;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Models.Dtos.Deals;
using Company.DealSystem.Application.Models.Dtos.Users;
using Company.DealSystem.Application.Models.ViewModels.Deals;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Threading.Tasks;
using Company.DealSystem.Application.Models.ViewModels.Users;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Enum;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Application.Services
{
    public class UserService : BaseService
    {
        public UserService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<UsersListResponse> List(UsersListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(UserListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("name", c => c.Name)
                .ConditionalOrder("username", c => c.Username)
                .ConditionalOrder("workflowRoleNames", c => c.WorkflowRolesInUser.Any(wru => wru.Active) ? c.WorkflowRolesInUser.First().WorkflowRole.Name : "")
                .ConditionalOrder("userRoleName", c => c.UserRole.Name)
                .ConditionalOrder("active", c => (c.Active ? "Yes" : "No"));

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c =>
                c.Name.Contains(str)
                || c.Username.Contains(str)
                || (c.WorkflowRolesInUser.Any(wu => wu.Active && wu.WorkflowRole.Name.Contains(str)))
                || (c.UserRole != null && c.UserRole.Name.Contains(str))
                || (c.Active ? "Yes" : "No").Contains(str)
                );
            }

            if (listRequest.OnlyActive)
                builder.Where(c => c.Active);

            if (listRequest.OnlyUsersWithLevel == true)
                builder.Where(c => c.WorkflowRolesInUser.Any(r => r.WorkflowRole.ApprovalLevel > 0));

            return new UsersListResponse
            {
                Users = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        public async Task<int?> GetUserHighestApprovalWorkflowRoleId(int userId)
        {
            var workflowRoleId = (await _repo.ProjectedListBuilder((WorkflowRole wr) => wr.Id)
                .Where(wr => wr.UsersInWorkflowRole.Any(u => u.UserId == userId))
                .OrderByDescending(wr => wr.ApprovalLevel ?? 0)
                .FirstOrDefaultAsync());

            if (workflowRoleId > 0)
                return workflowRoleId;
            else
                return null;
        }

        async public Task<UserDto> Get(int userId)
        {
            var user = await _repo.ProjectedGetById(userId, UserDto.ProjectionFromEntity);

            return user;
        }

        async public Task<UserPostResponse> Save(UserDto user, int? userId = null)
        {
            Validate(user);

            var creation = !user.Id.HasValue || user.Id == 0;

            // retrieve entity from db
            var entity = await _repo.GetById<User>(user.Id);

            if (!creation)
            {
                _repo.LoadCollection(entity, d => d.WorkflowRolesInUser);
                _repo.LoadCollection(entity, d => d.IntegrationData);
            }

            entity = user.ToEntity(entity, this);

            await _repo.Context.SaveEntityWithAudit(entity, FunctionalityEnum.Users);

            return new UserPostResponse { Name = entity.Name };
        }

        public void Validate(UserDto user)
        {
            if (Updatable.IsUpdatedButEmpty(user.Name))
                throw new BusinessRuleException("The field 'Name' is mandatory.");

            if (Updatable.IsUpdatedButEmpty(user.Username))
                throw new BusinessRuleException("The field 'Username' is mandatory.");

            if (_repo.Context.Users.Any(u => u.Id != user.Id && u.Username == user.Username.Value))
                throw new BusinessRuleException($"There is another user using {user.Username.Value} as username.", "Usernames must be unique");
        }
        public async Task<int> GetWorkflowRoleForUserId(int userId)
        {
            var workflowRoleId = await _repo.ProjectedGetById(userId, (User user) => user.WorkflowRolesInUser.Where(r => r.Active).Select(w => w.WorkflowRoleId).FirstOrDefault());
            if (workflowRoleId <= 0)
                throw new BusinessRuleException($"Your user (id: {userId}) does not have an active workflow role.");

            return workflowRoleId;
        }
        public async Task<List<LookupRequest>> GetWorkflowRolesForCurrentUser()
        {
            var userId = GetUserId();
            return await _repo.ProjectedListBuilder((WorkflowRole wr) => new LookupRequest { Id = wr.Id, Name = wr.Name })
                .Where(wr => wr.UsersInWorkflowRole.Any(uwr => uwr.UserId == userId && uwr.Active))
                .ExecuteAsync();
        }
        public async Task<List<LookupRequestHeader>> GetWorkflowRolesForUsersAndCurrent(List<int> userIds)
        {
            var userId = GetUserId();
            if (!userIds.Contains(userId))
                userIds.Add(userId);
                

            return await _repo.ProjectedListBuilder(
                (User user) => new LookupRequestHeader 
                { 
                    Id = user.Id,
                    Name = user.Name,
                    Results = user.WorkflowRolesInUser.AsQueryable().Select(wr => new LookupRequest { Id = wr.Id, Name = wr.WorkflowRole.Name }).ToList(),
                    CurrentUser = (user.Id == userId),
                })
                .Where(user => userIds.Contains(user.Id) && user.Active)
                .ExecuteAsync();
        }



        public async Task<List<LookupRequest>> GetWorkflowRoles()
        {
            return await _repo.ProjectedList(LookupRequest.ProjectionFromWorkflowRole);
        }
        public async Task<List<LookupRequest>> GetUserRoles()
        {
            return await _repo.ProjectedList(LookupRequest.ProjectionFromUserRole);
        }

        async public Task<List<UserFunctionalityReadDto>> ListFunctionalities()
        {
            var userId = GetUserId();
            var list = await _repo.ProjectedList(UserFunctionalityReadDto.ProjectionFromEntity, f => f.UserRole.Users.Any(u => u.Id == userId));
            return list;
        }

        async public Task<int> GetCorrespondingUserIdForIntegration(IntegrationTypeEnum type, string userIdAtSource) =>
            (await _repo.ProjectedList((UserIntegrationData ud) => ud.User.Id,
                ud => ud.IntegrationType == type
                && ud.Field == UserIntegrationFieldEnum.UserIdAtTheSource
                && ud.Data == userIdAtSource)).FirstOrDefault();

        class UserLogin
        {
            public int Id { get; set; }
            public bool Active { get; set; }
        }
    }
}