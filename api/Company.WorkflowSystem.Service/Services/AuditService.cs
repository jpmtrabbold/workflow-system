using System.Linq;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Models.ViewModels.Audit;
using Company.WorkflowSystem.Service.Models.Dtos.Audit;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Service.Services
{
    public class AuditService : BaseService
    {
        public AuditService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {

        }

        async public Task<AuditEntriesListResponse> List(AuditEntriesListRequest listRequest)
        {
            var builder = _repo.ProjectedListBuilder(AuditEntryListDto.ProjectionFromEntity, listRequest)
                .OrderBy(c => c.Id, descending: true)
                .ConditionalOrder("id", c => c.Id)
                .ConditionalOrder("dateTime", c => c.DateTime)
                .ConditionalOrder("userName", c=> c.UserName);

            builder.WhereEntity(a => a.EntityId == listRequest.EntityId && a.FunctionalityId == (int)listRequest.FunctionalityEnum);

            var str = listRequest.SearchString;
            if (!string.IsNullOrWhiteSpace(str))
            {
                builder.Where(c =>
                    c.UserName.Contains(str)
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