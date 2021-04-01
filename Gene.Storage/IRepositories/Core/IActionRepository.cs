using System;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Storage.IRepositories.Core
{
    public interface IActionRepository : IRepository<Guid?, Middleware.Entities.Core.Action, ActionDto, ActionFilter>
    {
        
    }
}