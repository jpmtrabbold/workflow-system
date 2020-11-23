using Company.DealSystem.Application.Models.ViewModels.Shared;

namespace Company.DealSystem.Application.Models.ViewModels.Users
{
    public class UsersListRequest : ListRequest
    {
        /// <summary>
        /// asks the system to return only users that have approval level
        /// </summary>
        public bool? OnlyUsersWithLevel { get; set; }
    }
}
