using System;
using System.Linq.Expressions;
using Company.DealSystem.Domain.Entities;
using Company.DealSystem.Application.Models.Helpers;
using Company.DealSystem.Domain.Models.Enum;
using Company.DealSystem.Application.Models.Dtos.Shared;
using Company.DealSystem.Application.Interfaces;
using Company.DealSystem.Domain.ExtensionMethods;
using Company.DealSystem.Domain.Interfaces;
using System.Collections.Generic;
using Company.DealSystem.Application.Models.ViewModels.Shared;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using System.Linq;
using Company.DealSystem.Domain.Util;

namespace Company.DealSystem.Application.Models.Dtos.Deals
{
    public class DealItemSourceDataDto
    {
        public long? SourceId { get; set; }
        public DealItemSourceTypeEnum? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}