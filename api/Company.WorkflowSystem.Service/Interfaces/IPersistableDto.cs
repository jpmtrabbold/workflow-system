using System;
using System.Collections.Generic;
using System.Text;
using Company.WorkflowSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.WorkflowSystem.Database.Context;
using Company.WorkflowSystem.Service.Services;

namespace Company.WorkflowSystem.Service.Interfaces
{
    internal interface IPersistableDto<TEntity, TService>
        where TEntity : class
        where TService : BaseService
    {
        TEntity ToEntity(TEntity entity, TService service);

    }
}
