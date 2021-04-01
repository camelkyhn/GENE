using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Middleware.ViewModels.Identity
{
    public class RoleViewModel : BaseViewModel
    {
        public IEnumerable<Role> Roles { get; set; }
        public Role RoleDetail { get; set; }
        public RoleDto Role { get; set; }
        public RoleFilter Filter { get; set; }
    }
}