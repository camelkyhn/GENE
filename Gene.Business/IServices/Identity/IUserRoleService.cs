using System;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;

namespace Gene.Business.IServices.Identity
{
    public interface IUserRoleService : IService, ICRUDService<Guid?, UserRoleDto, UserRoleFilter, UserRoleViewModel>
    {
        Task<Result<UserRoleViewModel>> GetRelatedModelsAsync(UserRoleViewModel viewModel);
    }
}