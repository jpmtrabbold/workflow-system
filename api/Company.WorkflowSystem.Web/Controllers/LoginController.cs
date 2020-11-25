using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Company.WorkflowSystem.Service.Services;
using Microsoft.Identity.Web;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        LoginService _service;
        IConfiguration _configuration;
        public LoginController(LoginService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [Authorize(AuthenticationSchemes = AzureADDefaults.AuthenticationScheme)]
        [HttpGet("HangfireLogin")]
        public ActionResult HangfireLogin()
        {
            return Redirect("/hangfire");
        }

        [Authorize(AuthenticationSchemes = AzureADDefaults.AuthenticationScheme)]
        [HttpGet("SignOut")]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl }, AzureADDefaults.AuthenticationScheme);
        }

        [Authorize(AuthenticationSchemes = AzureADDefaults.AuthenticationScheme)]
        [HttpGet]
        public ActionResult<string> SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return Redirect("/hangfire");
            }

            return "Signed out";
        }

        [HttpGet("HealthCheck")]
        public async Task<ActionResult<string>> HealthCheck()
        {
            try
            {
                await _service.TestDb();
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.ToString()}";
            }
            return "API up and running";
        }
    }
}
