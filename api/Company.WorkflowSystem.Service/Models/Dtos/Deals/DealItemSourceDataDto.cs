using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Service.Models.Helpers;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Service.Models.Dtos.Shared;
using Company.WorkflowSystem.Service.Interfaces;
using Company.WorkflowSystem.Domain.ExtensionMethods;
using Company.WorkflowSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.WorkflowSystem.Service.Models.ViewModels.Shared;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Database.Context;
using System.Linq;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Service.Models.Dtos.Deals
{
    public class DealItemSourceDataDto
    {
        public long? SourceId { get; set; }
        public DealItemSourceTypeEnum? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}