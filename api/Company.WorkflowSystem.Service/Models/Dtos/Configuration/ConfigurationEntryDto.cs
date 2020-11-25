using InversionRepo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Service.Services;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Domain.Entities.Configuration;
using Company.WorkflowSystem.Domain.Enum;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Database.Context;

namespace Company.WorkflowSystem.Service.Models.Dtos.Configuration
{
    public class ConfigurationEntryDto : UpdatableListItemDto, IPersistableDto<ConfigurationEntry, BaseService>
    {
        public string Name { get; set; }
        public ConfigurationIdentifiersEnum Identifier { get; set; }
        public Updatable<string> Content { get; set; }
        public ConfigurationEntryContentType ContentType { get; set; }
        public FunctionalityEnum? FunctionalityForLookup { get; set; }
        internal static Expression<Func<ConfigurationEntry, ConfigurationEntryDto>> ProjectionFromEntity
        {
            get
            {
                return entity => new ConfigurationEntryDto()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Identifier = entity.Identifier,
                    Content = Updatable.Create(entity.Content),
                    ContentType = entity.ContentType,
                    FunctionalityForLookup = entity.FunctionalityForLookup,
                };
            }
        }

        public ConfigurationEntry ToEntity(ConfigurationEntry entity, BaseService service)
        {
            if (entity == null)
            {
                entity = new ConfigurationEntry
                {
                    
                };
            }

            if (Updatable.IsUpdated(Content))
                entity.Content = Content.Value;

            return entity;
        }
    }
}
