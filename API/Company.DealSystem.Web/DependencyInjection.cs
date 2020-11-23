using Microsoft.Extensions.DependencyInjection;
using Company.DealSystem.Domain.Interfaces;
using Company.DealSystem.Infrastructure.EmailService;
using Company.DealSystem.Application.Services;
using InversionRepo.Interfaces;
using InversionRepo;
using Company.DealSystem.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Company.DealSystem.Domain.Interfaces.EmsTradepoint;
using Company.DealSystem.Infrastructure.EmsIntegration;
using Company.DealSystem.Domain.Services;

namespace Company.DealSystem.Web
{
    public static class DependencyInjection
    {
        internal static void Apply(IServiceCollection services)
        {
            // scoped injected dependencies
            services.AddScoped<IRepository<TradingDealsContext>, Repository<TradingDealsContext>>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IEmsTradepointService, EmsTradepointService>();

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
