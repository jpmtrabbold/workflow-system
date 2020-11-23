
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.SqlServer;
using System;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Web.Hangfire
{
    public static class HangfireRegistrations
    {
        public static void RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangfireConnection");
            var options = new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromHours(1),
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true
            };
            var SQLServerStorage = new SqlServerStorage(connectionString, options);

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, options));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            JobStorage.Current = SQLServerStorage;
            
            // Add/Update jobs to be processed
            var cron = Cron.Daily(16, 0); // 16h UTC - 4am NZT
            RecurringJob.AddOrUpdate<DealIntegrationService>("FetchEmsTrades", service => service.EmsFetchFromYesterday(), cron);

            cron = Cron.Daily(18, 0); // 18h UTC - 6am NZT
            //cron = Cron.Daily(2, 0); // 2h UTC - 2pm NZT - for testing
            RecurringJob.AddOrUpdate<ReminderService>("SendReminders", service => service.SendReminders(), cron);

        }
    }
}
