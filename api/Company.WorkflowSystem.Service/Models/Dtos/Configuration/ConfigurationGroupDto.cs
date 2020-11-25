using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Helpers;
using InversionRepo.Interfaces; using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Domain.Entities.Configuration;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Models.Dtos.Configuration
{
    public class ConfigurationGroupDto : IPersistableDto<ConfigurationGroup, BaseService>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ConfigurationEntryDto> Entries { get; private set; } = new List<ConfigurationEntryDto>();

        internal static Expression<Func<ConfigurationGroup, ConfigurationGroupDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new ConfigurationGroupDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Entries = entity.Entries.AsQueryable().Select(ConfigurationEntryDto.ProjectionFromEntity).ToList(),
                };
            }
        }

        public ConfigurationGroup ToEntity(ConfigurationGroup entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new ConfigurationGroup
                {

                };
            }

            Updatable.ToEntityCollection(Entries, entity.Entries, service);

            return entity;
        }
    }
}
