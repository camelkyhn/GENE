using System;
using System.Linq;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Storage.IRepositories.Core;

namespace Gene.Storage.Repositories.Core
{
    public class ControllerRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Core.Controller, ControllerDto, ControllerFilter>, IControllerRepository
    {
        public ControllerRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Core.Controller> Filter(IQueryable<Middleware.Entities.Core.Controller> queryableSet, ControllerFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.AreaName))
            {
                queryableSet = queryableSet.Where(c => c.Area.Name.ToLower().Contains(filter.AreaName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(c => c.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.DisplayName))
            {
                queryableSet = queryableSet.Where(c => c.DisplayName.ToLower().Contains(filter.DisplayName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
