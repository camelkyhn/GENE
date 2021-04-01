using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Storage.IRepositories.Identity
{
    public interface INotificationRepository : IRepository<Guid?, Middleware.Entities.Identity.Notification, NotificationDto, NotificationFilter>
    {
        Task CreateMultipleAsync(NotificationDto dto, IEnumerable<Middleware.Entities.Identity.User> userList);
        Task OpenNotificationAsync(Middleware.Entities.Identity.Notification entity);
        Task<int> GetNewNotificationsCountAsync(Guid? userId);
    }
}