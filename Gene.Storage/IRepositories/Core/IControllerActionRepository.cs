using System;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Storage.IRepositories.Core
{
    public interface IControllerActionRepository : IRepository<Guid?, Middleware.Entities.Core.ControllerAction, ControllerActionDto, ControllerActionFilter>
    {
        bool IsExistingToAdd(ControllerActionDto dto);
        bool IsExistingToUpdate(ControllerActionDto dto);
    }
}