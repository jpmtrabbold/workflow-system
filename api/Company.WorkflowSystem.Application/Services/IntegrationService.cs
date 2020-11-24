using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Infrastructure;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Application.Models.Dtos.Deals;
using Company.WorkflowSystem.Application.Models.Dtos.Users;
using Company.WorkflowSystem.Application.Models.ViewModels.Deals;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Threading.Tasks;
using Company.WorkflowSystem.Application.Models.ViewModels.Users;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Services;
using Company.WorkflowSystem.Application.Models.Dtos.Integration;
using Company.WorkflowSystem.Domain.Entities.Integrations;

namespace Company.WorkflowSystem.Application.Services
{
    public class IntegrationService : BaseService
    {
        public IntegrationService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<IntegrationRunsListResponse> List(IntegrationRunsListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(IntegrationRunListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("startedBy", c => c.UserId.HasValue ? c.User.Name : "System")
                .ConditionalOrder("started", c => c.Started)
                .ConditionalOrder("ended", c => c.Ended);

            if (listRequest.IntegrationRunId.HasValue)
            {
                builder.Where(c => c.Id == listRequest.IntegrationRunId);
            }
            else
            {
                var str = listRequest.SearchString;
                if (listRequest.IntegrationType.HasValue)
                    builder.Where(c => c.Type == listRequest.IntegrationType.Value);
                if (listRequest.Statuses?.Any() ?? false)
                    builder.Where(c => listRequest.Statuses.Contains(c.Status));
            }
            
            return new IntegrationRunsListResponse
            {
                Runs = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }

        async public Task ChangeIntegrationRunStatus(int integrationRunId, IntegrationRunStatusEnum currentStatus, IntegrationRunStatusEnum newStatus)
        {
            var run = await _repo.GetById<IntegrationRun>(integrationRunId);
            if (run.Status != currentStatus)
                throw new Exception("Please try again. This integration changed status while you took this action.");

            run.Status = newStatus;
            await _repo.SaveEntity(run);
        }

        async public Task<List<IntegrationRunEntryDto>> GetEntries(int integrationRunId)
        {
            return await _repo.ProjectedListBuilder(IntegrationRunEntryDto.ProjectionFromEntity)
                .Where(re => re.IntegrationRunId == integrationRunId)
                .ExecuteAsync();
        }
    }
}