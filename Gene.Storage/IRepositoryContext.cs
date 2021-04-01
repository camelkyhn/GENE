using Gene.Storage.IRepositories.Core;
using Gene.Storage.IRepositories.Identity;
using System;

namespace Gene.Storage
{
    public interface IRepositoryContext : IDisposable
    {
        public IActionRepository Actions { get; }
        public IAreaRepository Areas { get; }
        public IControllerRepository Controllers { get; }
        public IControllerActionRepository ControllerActions { get; }
        public IControllerActionRoleRepository ControllerActionRoles { get; }

        public IRoleRepository Roles { get; }
        public IUserRepository Users { get; }
        public IUserRoleRepository UserRoles { get; }
        public INotificationRepository Notifications { get; }
    }
}