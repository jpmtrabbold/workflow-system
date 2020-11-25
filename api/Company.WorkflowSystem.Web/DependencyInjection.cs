using Microsoft.Extensions.DependencyInjection;
using Company.WorkflowSystem.Domain.Interfaces;
using Company.WorkflowSystem.Infrastructure.EmailService;
using Company.WorkflowSystem.Service.Services;
using InversionRepo.Interfaces;
using InversionRepo;
using Company.WorkflowSystem.Database.Context;
using Microsoft.AspNetCore.Http;
using Company.WorkflowSystem.Domain.Interfaces.EmsTradepoint;
using Company.WorkflowSystem.Domain.Services;

namespace Company.WorkflowSystem.Web
{
    public static class DependencyInjection
    {
        internal static void Apply(IServiceCollection services)
        {
            // scoped injected dependencies
            services.AddScoped<IRepository<TradingDealsContext>, Repository<TradingDealsContext>>();
            services.AddSingleton<IEmailService, EmailService>();

            // scoped singletons
            services.AddScoped<ScopedDataService>();
            services.AddScoped<AuditService>();
            services.AddScoped<ConfigurationService>();
            services.AddScoped<DealService>();
            services.AddScoped<DealWorkflowService>();
            services.AddScoped<DealIntegrationService>();
            services.AddScoped<IntegrationService>();
            services.AddScoped<LoginService>();
            services.AddScoped<UserService>();
            services.AddScoped<CounterpartyService>();
            services.AddScoped<DealCategoryService>();
            services.AddScoped<SalesForecastService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ReminderService>();
            services.AddScoped<DealTypeService>();
            services.AddScoped<DealItemFieldsetService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
