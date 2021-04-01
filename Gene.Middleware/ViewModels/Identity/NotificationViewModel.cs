using System.Collections.Generic;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Filters.Identity;

namespace Gene.Middleware.ViewModels.Identity
{
    public class NotificationViewModel : BaseViewModel
    {
        public IEnumerable<Notification> Notifications { get; set; }
        public IEnumerable<User> Users { get; set; }
        public Notification NotificationDetail { get; set; }
        public NotificationDto Notification { get; set; }
        public NotificationFilter Filter { get; set; }
    }
}