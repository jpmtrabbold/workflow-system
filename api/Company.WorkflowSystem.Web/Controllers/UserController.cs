using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Company.WorkflowSystem.Application.Models.Dtos.Users;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using Company.WorkflowSystem.Application.Models.ViewModels.Users;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : BaseController
    {
        readonly UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost("List")]
        async public Task<UsersListResponse> List([FromBody] UsersListRequest listRequest) => await _service.List(listRequest);

        [HttpGet("{id}")]
        async public Task<UserDto> Get(int id) => await _service.Get(id);

        [HttpPost]
        async public Task<UserPostResponse> Post([FromBody] UserDto user)
        {
            return await _service.Save(user);
        }

        [HttpGet("GetWorkflowRoles")]
        async public Task<List<LookupRequest>> GetWorkflowRoles() => await _service.GetWorkflowRoles();

        [HttpGet("GetUserRoles")]
        async public Task<List<LookupRequest>> GetUserRoles() => await _service.GetUserRoles();

        [HttpPost("ListFunctionalities")]
        async public Task<List<UserFunctionalityReadDto>> ListFunctionalities() => await _service.ListFunctionalities();

        [HttpGet("GetWorkflowRolesForCurrentUser")]
        public async Task<List<LookupRequest>> GetWorkflowRolesForCurrentUser() => await _service.GetWorkflowRolesForCurrentUser();
        [HttpPost("GetWorkflowRolesForUsersAndCurrent")]
        public async Task<List<LookupRequestHeader>> GetWorkflowRolesForUsersAndCurrent([FromBody] List<int> userIds) => await _service.GetWorkflowRolesForUsersAndCurrent(userIds);
    }
}
