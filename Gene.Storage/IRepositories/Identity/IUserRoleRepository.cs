using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Storage.IRepositories.Identity
{
    public interface IUserRoleRepository : IRepository<Guid?, Middleware.Entities.Identity.UserRole, UserRoleDto, UserRoleFilter>
    {
        bool IsExistingToAdd(UserRoleDto dto);
        bool IsExistingToUpdate(UserRoleDto dto);
        Task<List<Guid?>> GetUserRoleIdListAsync(Guid? userId);
    }
}