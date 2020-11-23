using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Infrastructure;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Entities;
using System.Threading.Tasks;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Application.Services
{
    public class LoginService : BaseService
    {
        public LoginService(IRepository<TradingDealsContext> repo, ScopedDataService scopedDataService) : base(repo, scopedDataService)
        {
        }
        public async Task TestDb()
        {
            await _repo.ProjectedList((Deal d) => d.Id, d => d.Id == 1);
        }
    }
}
