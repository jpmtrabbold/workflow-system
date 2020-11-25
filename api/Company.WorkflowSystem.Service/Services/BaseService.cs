using InversionRepo.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Transactions;
using Company.WorkflowSystem.Service.Exceptions;
using Company.WorkflowSystem.Service.Models.Dtos.Users;
using Company.WorkflowSystem.Domain.Entities.Configuration;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Services;
using Company.WorkflowSystem.Database.Context;

namespace Company.WorkflowSystem.Service.Services
{
    public class BaseService
    {
        public readonly ScopedDataService scopedDataService;
        public readonly IRepository<TradingDealsContext> _repo;
        public BaseService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService)
        {
            _repo = repo;
            this.scopedDataService = scopedDataService;
        }

        public int? TryGetUserId(string name = null)
        {
            var userId = GetUserId(name, false);
            if (userId == 0)
                return null;
            else
                return userId;
        }

        public int GetUserId(string name = null, bool throwsExceptionIfNotLoggedIn = true)
        {
            var userId = scopedDataService.GetUserId();
            if (userId.HasValue)
                return userId.Value;

            var userName = name ?? scopedDataService.GetUsername();
            if (userName == null)
            {
                if (throwsExceptionIfNotLoggedIn)
                    throw new InvalidLoginException("User is not logged in.");
                else
                    return 0;
            }

            userId = GetUserIdByUserName(userName).Result;
            return userId.Value;
        }

        async Task<int> GetUserIdByUserName(string userName)
        {
            userName = userName.ToLower().Trim();
            var user = (await _repo.ProjectedListBuilder(UserListDto.ProjectionFromEntity).FirstOrDefaultAsync(u => u.Username.ToLower().Trim() == userName));

            if (user == null)
                throw new InvalidLoginException($"The user {userName} does not have access to this application. Please contact your systems admin.");
            else if (!user.Active)
                throw new InvalidLoginException($"The user {userName} is deactivated. Please contact your systems admin.");

            return user.Id;
        }

        async Task<string> GetUsernameById(int id)
        {
            var user = (await _repo.ProjectedListBuilder(UserListDto.ProjectionFromEntity).FirstOrDefaultAsync(u => u.Id == id));

            if (user == null)
                throw new InvalidLoginException($"The user {id} does not have access to this application. Please contact your systems admin.");
            else if (!user.Active)
                throw new InvalidLoginException($"The user {id} is deactivated. Please contact your systems admin.");

            return user.Username;
        }

        internal async Task LogUserIn(int userId)
        {
            var username = await GetUsernameById(userId);
            var identity = new GenericIdentity(username);
            var user = new GenericPrincipal(identity, new string[0]);
            scopedDataService.SetUser(user);
            scopedDataService.SetUserId(userId);
        }


        public async Task<ConfigurationReader> GetConfigurationEntries(ConfigurationGroupIdentifiersEnum groupIdentifier)
        {
            return new ConfigurationReader
            {
                Entries = (await _repo.ProjectedListBuilder((ConfigurationEntry group) => new { group.Identifier, group.Content })
                    .WhereEntity(group => group.ConfigurationGroup.Identifier == groupIdentifier).ExecuteAsync())
                    .ToDictionary(e => e.Identifier, e => e.Content)
            };
        }

        static int? TransactionScopeTimeOutMinutes;
        public TransactionScope CreateTransactionScope()
        {
            if (!TransactionScopeTimeOutMinutes.HasValue)
                TransactionScopeTimeOutMinutes = (GetConfigurationEntries(ConfigurationGroupIdentifiersEnum.GeneralSettings)).Result
                    .GetEntryAsInt(ConfigurationIdentifiersEnum.TimeOutTransactionScopeIdentifier);

            return new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, TransactionScopeTimeOutMinutes.Value, 0), TransactionScopeAsyncFlowOption.Enabled);
        }

    }
    public class ConfigurationReader
    {
        public Dictionary<ConfigurationIdentifiersEnum, string> Entries { get; set; }
        public int GetEntryAsInt(ConfigurationIdentifiersEnum identifier) => int.Parse(Entries[identifier]);
        public bool GetEntryAsBoolean(ConfigurationIdentifiersEnum identifier) => bool.Parse(Entries[identifier]);
        public string GetEntryAsString(ConfigurationIdentifiersEnum identifier) => Entries[identifier];
    }
}
