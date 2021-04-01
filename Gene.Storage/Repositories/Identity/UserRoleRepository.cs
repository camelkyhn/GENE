using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Storage.IRepositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Repositories.Identity
{
    public class UserRoleRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Identity.UserRole, UserRoleDto, UserRoleFilter>, IUserRoleRepository
    {
        public UserRoleRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Identity.UserRole> Filter(IQueryable<Middleware.Entities.Identity.UserRole> queryableSet, UserRoleFilter filter)
        {
            if (filter.UserId != null)
            {
                queryableSet = queryableSet.Where(ur => ur.UserId == filter.UserId);
            }

            if (!string.IsNullOrEmpty(filter.UserEmail))
            {
                queryableSet = queryableSet.Where(ur => ur.User.Email.ToLower().Contains(filter.UserEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                queryableSet = queryableSet.Where(ur => ur.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public bool IsExistingToAdd(UserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public bool IsExistingToUpdate(UserRoleDto dto)
        {
            return Context.UserRoles.Any(ur => ur.Id != dto.Id && ur.UserId == dto.UserId && ur.RoleId == dto.RoleId);
        }

        public async Task<List<Guid?>> GetUserRoleIdListAsync(Guid? userId)
        {
            return await Context.UserRoles.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync();
        }
    }
}
