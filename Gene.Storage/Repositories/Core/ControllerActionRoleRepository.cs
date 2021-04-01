using System;
using System.Collections.Generic;
using System.Linq;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Filters.Core;
using Gene.Storage.IRepositories.Core;
using Microsoft.Extensions.Caching.Memory;

namespace Gene.Storage.Repositories.Core
{
    public class ControllerActionRoleRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Core.ControllerActionRole, ControllerActionRoleDto, ControllerActionRoleFilter>, IControllerActionRoleRepository
    {
        private readonly IMemoryCache _cache;

        public ControllerActionRoleRepository(DatabaseContext context, IMemoryCache cache) : base(context)
        {
            _cache = cache;
        }

        public override IQueryable<Middleware.Entities.Core.ControllerActionRole> Filter(IQueryable<Middleware.Entities.Core.ControllerActionRole> queryableSet, ControllerActionRoleFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.ControllerName))
            {
                queryableSet = queryableSet.Where(car => car.ControllerAction.Controller.Name.ToLower().Contains(filter.ControllerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.ActionName))
            {
                queryableSet = queryableSet.Where(car => car.ControllerAction.Action.Name.ToLower().Contains(filter.ActionName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                queryableSet = queryableSet.Where(car => car.Role.Name.ToLower().Contains(filter.RoleName.ToLower()));
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public bool IsExistingToAdd(ControllerActionRoleDto dto)
        {
            return Context.ControllerActionRoles.Any(car => car.ControllerActionId == dto.ControllerActionId && car.RoleId == dto.RoleId);
        }

        public bool IsExistingToUpdate(ControllerActionRoleDto dto)
        {
            return Context.ControllerActionRoles.Any(car => car.Id != dto.Id && car.ControllerActionId == dto.ControllerActionId && car.RoleId == dto.RoleId);
        }

        public List<Middleware.Entities.Core.Controller> GetAuthorizedControllerList(IEnumerable<Guid?> roleIdList)
        {
            var controllerActionRoleList = GetAllControllerActionRoleListFromCache();
            var queryableControllers     = new List<Middleware.Entities.Core.Controller>().AsQueryable();
            queryableControllers = roleIdList.Select(roleId => controllerActionRoleList
                                                               .Where(car => car.RoleId == roleId && car.ControllerAction.Action.Name == Actions.Index)
                                                               .Select(car => new Middleware.Entities.Core.Controller
                                                               {
                                                                   Id          = car.ControllerAction.Controller.Id,
                                                                   Name        = car.ControllerAction.Controller.Name,
                                                                   DisplayName = car.ControllerAction.Controller.DisplayName,
                                                                   IconText    = car.ControllerAction.Controller.IconText,
                                                                   AreaId      = car.ControllerAction.Controller.AreaId,
                                                                   Area = new Middleware.Entities.Core.Area
                                                                   {
                                                                       Id          = car.ControllerAction.Controller.Area.Id,
                                                                       Name        = car.ControllerAction.Controller.Area.Name,
                                                                       DisplayName = car.ControllerAction.Controller.Area.DisplayName,
                                                                       IconText    = car.ControllerAction.Controller.Area.IconText
                                                                   }
                                                               })).Aggregate(queryableControllers, (current, controllers) => current.Union(controllers));
            return queryableControllers.ToList();
        }

        public bool HasAccess(IEnumerable<Guid?> roleIdList, string controllerName, string actionName)
        {
            var controllerActionRoleList = GetAllControllerActionRoleListFromCache();
            return roleIdList.Any(roleId => controllerActionRoleList.Any(car => car.RoleId == roleId && car.ControllerAction.Controller.Name == controllerName && car.ControllerAction.Action.Name == actionName));
        }

        private List<Middleware.Entities.Core.ControllerActionRole> GetAllControllerActionRoleListFromCache()
        {
            var controllerActionRoleList = _cache.Get<List<Middleware.Entities.Core.ControllerActionRole>>(Lists.ControllerActionRole);
            if (controllerActionRoleList == null)
            {
                controllerActionRoleList = Context.ControllerActionRoles.Select(car => new Middleware.Entities.Core.ControllerActionRole
                {
                    Id     = car.Id,
                    RoleId = car.RoleId,
                    ControllerAction = new Middleware.Entities.Core.ControllerAction
                    {
                        Action = new Middleware.Entities.Core.Action
                        {
                            Id   = car.ControllerAction.Action.Id,
                            Name = car.ControllerAction.Action.Name
                        },
                        Controller = new Middleware.Entities.Core.Controller
                        {
                            Id          = car.ControllerAction.Controller.Id,
                            Name        = car.ControllerAction.Controller.Name,
                            DisplayName = car.ControllerAction.Controller.DisplayName,
                            IconText    = car.ControllerAction.Controller.IconText,
                            AreaId      = car.ControllerAction.Controller.AreaId,
                            Area = new Middleware.Entities.Core.Area
                            {
                                Id          = car.ControllerAction.Controller.Area.Id,
                                Name        = car.ControllerAction.Controller.Area.Name,
                                DisplayName = car.ControllerAction.Controller.Area.DisplayName,
                                IconText    = car.ControllerAction.Controller.Area.IconText,
                            }
                        }
                    }
                }).ToList();
                _cache.Set(Lists.ControllerActionRole, controllerActionRoleList);
            }

            return controllerActionRoleList;
        }
    }
}