using System;
using Gene.Storage.IRepositories.Core;
using Gene.Storage.IRepositories.Identity;
using Gene.Storage.Repositories.Core;
using Gene.Storage.Repositories.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Gene.Storage
{
    public class RepositoryContext : IRepositoryContext
    {
        private readonly IMemoryCache _cache;
        private readonly DatabaseContext _databaseContext;

        private IActionRepository _actionRepository;
        private IAreaRepository _areaRepository;
        private IControllerRepository _controllerRepository;
        private IControllerActionRepository _controllerActionRepository;
        private IControllerActionRoleRepository _controllerActionRoleRepository;

        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private IUserRoleRepository _userRoleRepository;
        private INotificationRepository _notificationRepository;

        public RepositoryContext(IMemoryCache cache, DatabaseContext databaseContext)
        {
            _cache           = cache;
            _databaseContext = databaseContext;
        }

        public IActionRepository Actions {get { return _actionRepository ??= new ActionRepository(_databaseContext); }}
        public IAreaRepository Areas {get { return _areaRepository ??= new AreaRepository(_databaseContext); }}
        public IControllerRepository Controllers {get { return _controllerRepository ??= new ControllerRepository(_databaseContext); }}
        public IControllerActionRepository ControllerActions {get { return _controllerActionRepository ??= new ControllerActionRepository(_databaseContext); }}
        public IControllerActionRoleRepository ControllerActionRoles {get { return _controllerActionRoleRepository ??= new ControllerActionRoleRepository(_databaseContext, _cache); }}

        public IRoleRepository Roles {get { return _roleRepository ??= new RoleRepository(_databaseContext); }}
        public IUserRepository Users {get { return _userRepository ??= new UserRepository(_databaseContext); }}
        public IUserRoleRepository UserRoles {get { return _userRoleRepository ??= new UserRoleRepository(_databaseContext); }}
        public INotificationRepository Notifications {get { return _notificationRepository ??= new NotificationRepository(_databaseContext); }}

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _databaseContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}