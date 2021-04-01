using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Filters.Core;

namespace Gene.Middleware.ViewModels.Core
{
    public class ControllerActionRoleViewModel : BaseViewModel
    {
        public IEnumerable<ControllerActionRole> ControllerActionRoles { get; set; }
        public IEnumerable<ControllerAction> ControllerActions { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public ControllerActionRole ControllerActionRoleDetail { get; set; }
        public ControllerActionRoleDto ControllerActionRole { get; set; }
        public ControllerActionRoleFilter Filter { get; set; }
    }
}