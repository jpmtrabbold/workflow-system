using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Application.Models.Dtos.Users
{
    public class UserFunctionalityReadDto
    {
        public FunctionalityEnum FunctionalityEnum { get; set; }
        public List<UserSubFunctionalityReadDto> SubFunctionalities { get; set; }
        internal static Expression<Func<FunctionalityInUserRole, UserFunctionalityReadDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserFunctionalityReadDto()
                {
                    FunctionalityEnum = entity.Functionality.FunctionalityEnum,
                    SubFunctionalities = entity.SubFunctionalitiesInUserRole.AsQueryable().Select(UserSubFunctionalityReadDto.ProjectionFromEntity).ToList()
                };
            }
        }
    }
}
