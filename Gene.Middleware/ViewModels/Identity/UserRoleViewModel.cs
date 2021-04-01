using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Middleware.ViewModels.Identity
{
    public class UserRoleViewModel : BaseViewModel
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public UserRole UserRoleDetail { get; set; }
        public UserRoleDto UserRole { get; set; }
        public UserRoleFilter Filter { get; set; }
    }
}