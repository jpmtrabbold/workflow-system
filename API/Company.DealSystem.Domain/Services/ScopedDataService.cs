using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Company.DealSystem.Domain.Services
{
    public class ScopedDataService
    {
        string Username;
        int? UserId;
        int? TemporaryUserId;
        IHttpContextAccessor httpContextAccessor;
        public ScopedDataService(IHttpContextAccessor httpContextAccessor)
        {
            Username = "user@user.com"; //httpContextAccessor.HttpContext?.User?.Identity.Name;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void SetUserId(int userId) => UserId = userId;
        public void SetTemporaryUserId(int userId) => TemporaryUserId = userId;
        public void ClearTemporaryUserId() => TemporaryUserId = null;
        public int? GetUserId() => TemporaryUserId.HasValue ? TemporaryUserId.Value : UserId;
        public string GetUsername() => Username;
        public void SetUser(GenericPrincipal user) => httpContextAccessor.HttpContext.User = user;
    }
}
