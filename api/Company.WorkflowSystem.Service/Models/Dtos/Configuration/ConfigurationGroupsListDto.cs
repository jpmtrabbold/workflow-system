using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Entities.Configuration;

namespace Company.WorkflowSystem.Service.Models.Dtos.Configuration
{
    public class ConfigurationGroupsListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// using expressions allow entity framework to just select the exact fields we need.
        /// this expression is to be used with collections, in the select clause
        /// </summary>
        internal static Expression<Func<ConfigurationGroup, ConfigurationGroupsListDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new ConfigurationGroupsListDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,                    
                };
            }
        }
    }
}
