using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Middleware.ViewModels.Identity
{
    public class UserViewModel : BaseViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public User UserDetail { get; set; }
        public UserDto User { get; set; }
        public UserFilter Filter { get; set; }
    }
}