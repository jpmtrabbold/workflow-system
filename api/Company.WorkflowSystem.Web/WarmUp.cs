using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Infrastructure.EmailService;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Company.WorkflowSystem.Application.Models.ViewModels.Deals;

namespace Company.WorkflowSystem.Web
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
