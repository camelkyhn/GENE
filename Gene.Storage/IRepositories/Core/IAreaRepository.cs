using System;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Storage.IRepositories.Core
{
    public interface IAreaRepository : IRepository<Guid?, Middleware.Entities.Core.Area, AreaDto, AreaFilter>
    {
        
    }
}