using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.Dtos.Users;

namespace Company.WorkflowSystem.Service.Models.ViewModels.Users
{
    public class UsersListResponse : ListResponse
    {
        public List<UserListDto> Users { get; set; }
    }
}
