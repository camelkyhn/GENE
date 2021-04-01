using System;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Storage.IRepositories.Identity
{
    public interface IRoleRepository : IRepository<Guid?, Middleware.Entities.Identity.Role, RoleDto, RoleFilter>
    {
        
    }
}