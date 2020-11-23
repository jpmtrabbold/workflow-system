using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Infrastructure.EmailService;
using InversionRepo.Interfaces; using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Company.DealSystem.Application.Models.ViewModels.Deals;

namespace Company.DealSystem.Web
{
    public static class WarmUp
    {
        internal static void Run(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dealService = scope.ServiceProvider.GetService<DealService>();

                var unused = dealService.List(new DealsListRequest
                {
                    PageSize = 1,
                    PageNumber = 1,
                });
            }
            
        }
    }
}
