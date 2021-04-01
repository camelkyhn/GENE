using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;

namespace Gene.Business.IServices.Core
{
    public interface IControllerActionRoleService : IService, ICRUDService<Guid?, ControllerActionRoleDto, ControllerActionRoleFilter, ControllerActionRoleViewModel>
    {
        Task<Result<ControllerActionRoleViewModel>> GetRelatedModelsAsync(ControllerActionRoleViewModel viewModel);
        Task<Result<List<Middleware.Entities.Core.Controller>>> GetAuthorizedControllerListAsync(Guid? userId);
        Result<bool?> HasAccess(Guid? userId, string controllerName, string actionName);
    }
}