using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using Company.WorkflowSystem.Application.Exceptions;
using Company.WorkflowSystem.Web.Models;

namespace Company.WorkflowSystem.Web.Middleware
{
    
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration Configuration { get; }

        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BusinessRuleException ex)
            {
                /* log error with any injected LOGGER here */
                await HandleBusinessRuleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                /* log error with any injected LOGGER here */
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = $"{exception.Message}";
            if (exception.InnerException != null)
                message += $"\n\n Inner exception: {exception.InnerException.Message}";

            var invalidLogin = false;
            if (exception is InvalidLoginException || exception.InnerException is InvalidLoginException)
                invalidLogin = true;

            return context.Response.WriteAsync(new ExceptionData()
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                InvalidLogin = invalidLogin,

                StackTrace = exception.ToString() + (exception.InnerException != null ? "\n\nInnerException Trace:\n\n" + exception.InnerException?.ToString(): ""),
            }.ToString());
        }

        private Task HandleBusinessRuleExceptionAsync(HttpContext context, BusinessRuleException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var message = $"{exception.Message}";
            if (exception.InnerException != null)
                message += $"\n\n Inner exception: {exception.InnerException.Message}";

            var invalidLogin = false;
            if (exception.InnerException is InvalidLoginException)
                invalidLogin = true;

            return context.Response.WriteAsync(new ExceptionData()
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Title = exception.Title,
                InvalidLogin = invalidLogin,
            }.ToString());
        }
    }
}
