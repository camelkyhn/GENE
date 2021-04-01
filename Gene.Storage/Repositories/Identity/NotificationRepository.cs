using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Identity;
using Gene.Storage.IRepositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gene.Storage.Repositories.Identity
{
    public class NotificationRepository : Repository<DatabaseContext, Guid?, Middleware.Entities.Identity.Notification, NotificationDto, NotificationFilter>, INotificationRepository
    {
        public NotificationRepository(DatabaseContext context) : base(context)
        {
        }

        public override IQueryable<Middleware.Entities.Identity.Notification> Filter(IQueryable<Middleware.Entities.Identity.Notification> queryableSet, NotificationFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Title))
            {
                queryableSet = queryableSet.Where(n => n.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (filter.ReceiverUserId != null)
            {
                queryableSet = queryableSet.Where(n => n.ReceiverUserId == filter.ReceiverUserId);
            }

            if (!string.IsNullOrEmpty(filter.ReceiverUserEmail))
            {
                queryableSet = queryableSet.Where(n => n.ReceiverUser.Email.ToLower().Contains(filter.ReceiverUserEmail.ToLower()));
            }

            if (filter.IsOpened != null)
            {
                queryableSet = queryableSet.Where(n => n.IsOpened == filter.IsOpened);
            }

            queryableSet = base.Filter(queryableSet, filter);
            return queryableSet;
        }

        public async Task CreateMultipleAsync(NotificationDto dto, IEnumerable<Middleware.Entities.Identity.User> userList)
        {
            foreach (var user in userList)
            {
                var entity = dto.MapTo(new Middleware.Entities.Identity.Notification());
                entity.ReceiverUserId = user.Id;
                entity.IsOpened       = false;
                entity.CreatedUserId  = dto.UpdatedUserId;
                entity.CreatedDate    = DateTimeOffset.UtcNow;
                entity.UpdatedDate    = DateTimeOffset.UtcNow;
                await Context.AddAsync(entity);
            }

            await Context.SaveChangesAsync();
        }

        public async Task OpenNotificationAsync(Middleware.Entities.Identity.Notification entity)
        {
            entity.IsOpened = true;
            Context.Entry(entity).Property(a => a.IsOpened).IsModified = true;
            await Context.SaveChangesAsync();
        }

        public async Task<int> GetNewNotificationsCountAsync(Guid? userId)
        {
            return await Context.Notifications.CountAsync(n => !n.IsOpened && n.ReceiverUserId == userId);
        }
    }
}