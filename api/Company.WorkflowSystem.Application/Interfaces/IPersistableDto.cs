using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Infrastructure.Context;
using Company.WorkflowSystem.Application.Services;

namespace Company.WorkflowSystem.Application.Interfaces
{
    internal interface IPersistableDto<TEntity, TService>
        where TEntity : class
        where TService : BaseService
    {
        TEntity ToEntity(TEntity entity, TService service);

    }
}
