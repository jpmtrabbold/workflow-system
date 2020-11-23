using Company.DealSystem.Application.Models.ViewModels.Shared;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.Dtos.Users;

namespace Company.DealSystem.Application.Models.ViewModels.Users
{
    public class UsersListResponse : ListResponse
    {
        public List<UserListDto> Users { get; set; }
    }
}
