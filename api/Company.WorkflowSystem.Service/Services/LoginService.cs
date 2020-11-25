using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Database;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Entities;
using System.Threading.Tasks;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Service.Services
{
    public class LoginService : BaseService
    {
        public LoginService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {
        }
        public async Task TestDb()
        {
            await _repo.ProjectedListBuilder((Deal d) => d.Id).WhereEntity(d => d.Id == 1).ExecuteAsync();
        }
    }
}
