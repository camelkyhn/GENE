using System;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;

namespace Gene.Business.IServices.Core
{
    public interface IControllerService : IService, ICRUDService<Guid?, ControllerDto, ControllerFilter, ControllerViewModel>
    {
        Task<Result<ControllerViewModel>> GetRelatedModelsAsync(ControllerViewModel viewModel);
    }
}