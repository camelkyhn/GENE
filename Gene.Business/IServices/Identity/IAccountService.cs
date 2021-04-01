using System;
using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;

namespace Gene.Business.IServices.Identity
{
    public interface IAccountService : IService
    {
        Task<Result<ProfileViewModel>> GetProfileAsync(string notifyMessage);
        Task<Result<LoginViewModel>> LoginAsync(LoginDto dto);
        Task<Result<BaseViewModel>> LogoutAsync();
        Task<Result<RegisterViewModel>> RegisterAsync(RegisterDto dto);
        Task<Result<ConfirmEmailViewModel>> ConfirmEmailAsync(ConfirmEmailDto dto);
        Task<Result<ForgotPasswordViewModel>> ForgotPasswordAsync(ForgotPasswordDto dto);
        Task<Result<ResetPasswordViewModel>> ResetPasswordAsync(ResetPasswordDto dto);
        Task<Result<ChangePasswordViewModel>> ChangePasswordAsync(ChangePasswordDto dto);
        Task<Result<NotificationViewModel>> GetUserNotificationListAsync(NotificationFilter filter);
        Task<Result<NotificationViewModel>> OpenAndGetNotificationAsync(Guid? id);
        Task<Result<NotificationViewModel>> DeleteNotificationAsync(Guid? id);
    }
}