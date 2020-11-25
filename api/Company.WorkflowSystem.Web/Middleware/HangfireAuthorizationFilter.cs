using Hangfire.Dashboard;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Web.Middleware
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            var username = httpContext.User.Claims.Where(c => c.Type == "preferred_username").FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return false;

            var _loginService = context.GetHttpContext().RequestServices.GetService<LoginService>();
            try
            {
                _loginService.GetUserId(username);
            }
            catch
            {
                return false;
            }
            
            return true;
        }
    }
}
