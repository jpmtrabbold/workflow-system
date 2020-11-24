using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.Dtos.Users;

namespace Company.WorkflowSystem.Application.Models.ViewModels.Users
{
    public class UsersListResponse : ListResponse
    {
        public List<UserListDto> Users { get; set; }
    }
}
