using System.Linq;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using System.Threading.Tasks;
using Company.WorkflowSystem.Application.Models.ViewModels.Audit;
using Company.WorkflowSystem.Application.Models.Dtos.Audit;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Application.Services
{
    public class AuditService : BaseService
    {
        public AuditService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<AuditEntriesListResponse> List(AuditEntriesListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(AuditEntryListDto.ProjectionFromEntity, listRequest)
                .OrderByDescending(c => c.Id)
                .ConditionalOrder("id", c => c.Id)
                .ConditionalOrder("dateTime", c => c.DateTime)
                .ConditionalOrder("userName", c=> c.User.Name);

            builder.Where(a => a.EntityId == listRequest.EntityId && a.FunctionalityId == (int)listRequest.FunctionalityEnum);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c =>
                    c.User.Name.Contains(str)
                    || c.Tables.Any(t => t.TableName.Contains(str) || t.Fields.Any(f => f.FieldName.Contains(str) || f.OldValue.Contains(str) || f.NewValue.Contains(str)))
                    );
            }

            return new AuditEntriesListResponse
            {
                AuditEntries = await builder.ExecuteAsync(),
                TotalRecords = await builder.CountAsync()
            };
        }
    }
}