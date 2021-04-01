using System;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.ViewModels.Identity;

namespace Gene.Business.IServices.Identity
{
    public interface IUserService : IService, ICRUDService<Guid?, UserDto, UserFilter, UserViewModel>
    {
    }
}