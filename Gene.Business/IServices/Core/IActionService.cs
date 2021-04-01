using System;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.ViewModels.Core;

namespace Gene.Business.IServices.Core
{
    public interface IActionService : IService, ICRUDService<Guid?, ActionDto, ActionFilter, ActionViewModel>
    {
    }
}