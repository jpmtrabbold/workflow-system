using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Models.Enum;

namespace Company.WorkflowSystem.Application.Models.Dtos.Users
{
    public class UserSubFunctionalityReadDto
    {
        public SubFunctionalityEnum SubFunctionalityEnum { get; set; }
        internal static Expression<Func<SubFunctionalityInUserRole, UserSubFunctionalityReadDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new UserSubFunctionalityReadDto()
                {
                    SubFunctionalityEnum = entity.SubFunctionality.SubFunctionalityEnum,
                };
            }
        }
    }
}
