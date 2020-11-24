using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Web.Middleware;
using Hangfire;
using Hangfire.SqlServer;
using System;
using NJsonSchema.Generation;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Company.WorkflowSystem.Web.Hangfire;

namespace Company.WorkflowSystem.Web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(Configuration.GetValue<string>("FrontEndBaseUrl"))
                    .AllowAnyMethod()
                    .WithExposedHeaders("Content-Length", "Access-Control-Allow-Origin", "TotalRecords", "Origin")
                    .AllowCredentials()
                    .AllowAnyHeader());
            });

            services.AddAuthentication("Bearer")
                .AddScheme<BasicAuthenticationOptions, CustomAuthenticationHandler>("Bearer", null);

            services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();

            //// Add authentication (Azure AD) 
            //services
            //    .AddAuthentication(sharedOptions =>
            //    {
            //        sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.Audience = Configuration["AzureAd:Audience"];
            //        options.Authority = $"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}";
            //    });

            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddAzureAD(options =>
            //    {
            //        options.Instance = Configuration["AzureAd:Instance"];
            //        options.TenantId = Configuration["AzureAd:TenantId"];
            //        options.ClientId = Configuration["AzureAd:Audience"];
            //        options.CallbackPath = Configuration["AzureAd:CallbackPath"];

            //    });

            //services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            //{
            //    options.Authority = options.Authority + "/v2.0/";         // Microsoft identity platform

            //    options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            //});

            //services.RegisterHangfire(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
                options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                
            });

            // Register the Swagger services
            services.AddSwaggerDocument((x) =>
            {
                x.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "WorkflowSystem API";
                    document.Info.Description = $"API for the following WorkflowSystem front-end: {Configuration.GetValue<string>("FrontEndBaseUrl")}";
                };
                x.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
                x.DefaultResponseReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
            });

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TradingDealsContext>
                (options => options
                .UseSqlServer(connection,  b => b.MigrationsAssembly("Company.WorkflowSystem.Web"))
                );

            // ** Any local dependency injections go inside DependencyInjection.Apply
            DependencyInjection.Apply(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TradingDealsContext context)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            var options = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                AppPath = Configuration.GetValue<string>("FrontEndBaseUrl"),
            };
            
            //app.UseHangfireDashboard("/hangfire", options);
            
            WarmUp.Run(app.ApplicationServices);
        }
    }
}
