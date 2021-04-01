using System;
using System.Collections.Generic;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;

namespace Gene.Storage.IRepositories.Core
{
    public interface IControllerActionRoleRepository : IRepository<Guid?, Middleware.Entities.Core.ControllerActionRole, ControllerActionRoleDto, ControllerActionRoleFilter>
    {
        bool IsExistingToAdd(ControllerActionRoleDto dto);
        bool IsExistingToUpdate(ControllerActionRoleDto dto);
        List<Middleware.Entities.Core.Controller> GetAuthorizedControllerList(IEnumerable<Guid?> roleIdList);
        bool HasAccess(IEnumerable<Guid?> roleIdList, string controllerName, string actionName);
    }
}