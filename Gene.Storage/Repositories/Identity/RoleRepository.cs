using System;
using System.Linq;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Storage.IRepositories.Identity;

namespace Gene.Storage.Repositories.Identity
{
    public class RoleRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Identity.Role, RoleDto, RoleFilter>, IRoleRepository
    {
        public RoleRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Identity.Role> Filter(IQueryable<Middleware.Entities.Identity.Role> queryableSet, RoleFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(r => r.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
