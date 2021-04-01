using System;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;

namespace Gene.Business.IServices.Core
{
    public interface IControllerActionService : IService, ICRUDService<Guid?, ControllerActionDto, ControllerActionFilter, ControllerActionViewModel>
    {
        Task<Result<ControllerActionViewModel>> GetRelatedModelsAsync(ControllerActionViewModel viewModel);
    }
}