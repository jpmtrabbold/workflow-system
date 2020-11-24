using System;
using System.Linq.Expressions;
using Company.WorkflowSystem.Domain.Entities;
using Company.WorkflowSystem.Application.Models.Helpers;
using Company.WorkflowSystem.Domain.Models.Enum;
using Company.WorkflowSystem.Application.Models.Dtos.Shared;
using Company.WorkflowSystem.Application.Interfaces;
using Company.WorkflowSystem.Domain.ExtensionMethods;
using Company.WorkflowSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.WorkflowSystem.Application.Models.ViewModels.Shared;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Infrastructure.Context;
using System.Linq;
using Company.WorkflowSystem.Domain.Util;

namespace Company.WorkflowSystem.Application.Models.Dtos.Deals
{
    public class DealItemSourceDataDto
    {
        public long? SourceId { get; set; }
        public DealItemSourceTypeEnum? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}