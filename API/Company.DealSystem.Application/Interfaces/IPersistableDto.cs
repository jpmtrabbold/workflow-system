using System;
using System.Collections.Generic;
using System.Text;
using Company.DealSystem.Domain.Interfaces;
using InversionRepo.Interfaces;
using Company.DealSystem.Infrastructure.Context;
using Company.DealSystem.Application.Services;

namespace Company.DealSystem.Application.Interfaces
{
    internal interface IPersistableDto<TEntity, TService>
        where TEntity : class
        where TService : BaseService
    {
        TEntity ToEntity(TEntity entity, TService service);

    }
}
