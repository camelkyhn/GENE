using System;
using System.Threading.Tasks;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;

namespace Gene.Business.IServices.Identity
{
    public interface INotificationService : IService, ICRUDService<Guid?, NotificationDto, NotificationFilter, NotificationViewModel>
    {
        Task<Result<NotificationViewModel>> SendNotificationGlobalAsync(NotificationDto dto);
        Task<Result<NotificationViewModel>> GetRelatedModelsAsync(NotificationViewModel viewModel);
        Task<Result<int?>> GetNewNotificationsCountAsync();
    }
}