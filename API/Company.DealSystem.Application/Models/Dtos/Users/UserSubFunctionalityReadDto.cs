using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Domain.Models.Enum;

namespace Company.DealSystem.Application.Models.Dtos.Users
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
