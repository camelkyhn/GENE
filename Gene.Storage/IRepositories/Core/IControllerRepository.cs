using System;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Storage.IRepositories.Core
{
    public interface IControllerRepository : IRepository<Guid?, Middleware.Entities.Core.Controller, ControllerDto, ControllerFilter>
    {
        
    }
}