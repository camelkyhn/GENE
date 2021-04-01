using System;
using System.Linq;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Storage.IRepositories.Core;

namespace Gene.Storage.Repositories.Core
{
    public class ControllerActionRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Core.ControllerAction, ControllerActionDto, ControllerActionFilter>, IControllerActionRepository
    {
        public ControllerActionRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Core.ControllerAction> Filter(IQueryable<Middleware.Entities.Core.ControllerAction> queryableSet, ControllerActionFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.ControllerName))
            {
                queryableSet = queryableSet.Where(ca => ca.Controller.Name.ToLower().Contains(filter.ControllerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.ActionName))
            {
                queryableSet = queryableSet.Where(ca => ca.Action.Name.ToLower().Contains(filter.ActionName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public bool IsExistingToAdd(ControllerActionDto dto)
        {
            return Context.ControllerActions.Any(ca => ca.ControllerId == dto.ControllerId && ca.ActionId == dto.ActionId);
        }

        public bool IsExistingToUpdate(ControllerActionDto dto)
        {
            return Context.ControllerActions.Any(ca => ca.Id != dto.Id && ca.ControllerId == dto.ControllerId && ca.ActionId == dto.ActionId);
        }
    }
}
