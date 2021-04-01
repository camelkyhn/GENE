using System;
using System.Linq;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Storage.IRepositories.Core;

namespace Gene.Storage.Repositories.Core
{
    public class AreaRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Core.Area, AreaDto, AreaFilter>, IAreaRepository
    {
        public AreaRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Core.Area> Filter(IQueryable<Middleware.Entities.Core.Area> queryableSet, AreaFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(a => a.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.DisplayName))
            {
                queryableSet = queryableSet.Where(a => a.DisplayName.ToLower().Contains(filter.DisplayName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
