using System;
using System.Linq;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Storage.IRepositories.Core;

namespace Gene.Storage.Repositories.Core
{
    public class ActionRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Core.Action, ActionDto, ActionFilter>, IActionRepository
    {
        public ActionRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Core.Action> Filter(IQueryable<Middleware.Entities.Core.Action> queryableSet, ActionFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                queryableSet = queryableSet.Where(a => a.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }
    }
}
