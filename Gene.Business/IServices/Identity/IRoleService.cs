using System;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.ViewModels.Identity;

namespace Gene.Business.IServices.Identity
{
    public interface IRoleService : IService, ICRUDService<Guid?, RoleDto, RoleFilter, RoleViewModel>
    {
    }
}