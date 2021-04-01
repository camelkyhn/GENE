using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gene.Business.IServices.Identity;
using Gene.Business.IServices.System;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Dtos.System;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Exceptions;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;
using Gene.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Gene.Business.Services.Identity
{
    public class AccountService : Service, IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly IMailService _mailService;

        public AccountService(IRepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IMailService mailService) : base(httpContextAccessor, repositoryContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _mailService = mailService;
        }

        public async Task<Result<ProfileViewModel>> GetProfileAsync(string notifyMessage)
        {
            var result = new Result<ProfileViewModel>();
            try
            {
                var userResult = await RepositoryContext.Users.GetAsync(CurrentUserId);
                var viewModel = userResult.MapTo(new ProfileViewModel());
                viewModel.NotifyMessage = notifyMessage;
                result.Success(viewModel);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<LoginViewModel>> LoginAsync(LoginDto dto)
        {
            var result = new Result<LoginViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<LoginViewModel>
                    {
                        IsSucceeded = false,
                        Data = new LoginViewModel { Login = dto }
                    };
                }

                var userResult = await RepositoryContext.Users.GetByEmailAsync(dto.Email);
                if (!userResult.IsEmailConfirmed)
                {
                    return GetFailedLoginResult(dto, Errors.LoginConfirmEmail);
                }

                if (userResult.IsLockoutEnabled && userResult.LockoutEnd != null && userResult.LockoutEnd > DateTimeOffset.UtcNow)
                {
                    return GetFailedLoginResult(dto, Errors.AccountLockedOut + userResult.LockoutEnd);
                }

                var hasher = new PasswordHasher<Middleware.Entities.Identity.User>();
                var hashResult = hasher.VerifyHashedPassword(userResult, userResult.PasswordHash, dto.Password);
                if (hashResult == PasswordVerificationResult.Success)
                {
                    await SignInAsync(_httpContextAccessor.HttpContext, userResult, dto.RememberMe);
                    if (userResult.LockoutEnd != null || userResult.AccessFailedCount > 0)
                    {
                        await RepositoryContext.Users.ClearFailedAttemptsAsync(userResult);
                    }
                }
                else
                {
                    if (userResult.IsLockoutEnabled)
                    {
                        await RepositoryContext.Users.IncreaseFailedAttemptsAsync(userResult);
                        if (userResult.LockoutEnd != null)
                        {
                            return GetFailedLoginResult(dto, Errors.AccountLockedOut + userResult.LockoutEnd);
                        }
                    }

                    return GetFailedLoginResult(dto, Errors.WrongLoginAttempt);
                }

                result.Success(new LoginViewModel());
            }
            catch (NotFoundException)
            {
                return GetFailedLoginResult(dto, Errors.NotFoundEmail);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<BaseViewModel>> LogoutAsync()
        {
            var result = new Result<BaseViewModel>();
            try
            {
                await (_httpContextAccessor.HttpContext ?? throw new ServiceNotAvailableException(nameof(IHttpContextAccessor))).SignOutAsync();
                result.Success(new BaseViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RegisterViewModel>> RegisterAsync(RegisterDto dto)
        {
            var result = new Result<RegisterViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<RegisterViewModel>
                    {
                        IsSucceeded = false,
                        Data = new RegisterViewModel { Register = dto }
                    };
                }

                if (IsPasswordFormatValid(dto.Password))
                {
                    return GetFailedRegisterResult(dto, Errors.Password);
                }

                if (RepositoryContext.Users.IsEmailTaken(dto.Email))
                {
                    return GetFailedRegisterResult(dto, Errors.EmailTaken);
                }

                var user = new User
                {
                    AccessFailedCount = 0,
                    Email = dto.Email,
                    LockoutEnd = null,
                    IsEmailConfirmed = false,
                    IsEmailEnabled = false,
                    IsLockoutEnabled = true,
                    IsPhoneNumberConfirmed = false,
                    IsSmsEnabled = false,
                    IsTwoFactorEnabled = false,
                    PhoneNumber = dto.PhoneNumber,
                    CreatedUserId = _cache.Get<User>(Users.AdminKey).Id,
                    UpdatedUserId = _cache.Get<User>(Users.AdminKey).Id,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Status = Status.Active
                };
                var hasher = new PasswordHasher<Middleware.Entities.Identity.User>();
                user.PasswordHash = hasher.HashPassword(user, dto.Password);
                var createdUserResult = await RepositoryContext.Users.CreateAsync(user.MapTo(new UserDto()));
                var token = GenerateToken(createdUserResult.SecurityStamp);
                var callbackUrl = Urls.MailLinkAccountConfirmEmail + "?userId=" + createdUserResult.Id + "&token=" + token;
                var mail = new MailDto
                {
                    ToEmailAddress = createdUserResult.Email,
                    Subject = Titles.EmailConfirmation,
                    Message = Messages.EmailConfirmation(callbackUrl),
                    IsBodyPlainText = false
                };
                var mailResult = await _mailService.SendMailAsync(mail);
                if (mailResult.IsFailed())
                {
                    return mailResult.MapTo(result);
                }

                result.Success(new RegisterViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ConfirmEmailViewModel>> ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            var result = new Result<ConfirmEmailViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<ConfirmEmailViewModel>
                    {
                        IsSucceeded = false,
                        Data = new ConfirmEmailViewModel { ConfirmEmail = dto }
                    };
                }

                var userResult = await RepositoryContext.Users.GetAsync(dto.UserId);
                if (userResult.IsEmailConfirmed)
                {
                    return GetFailedConfirmEmailResult(dto, Errors.AlreadyConfirmedEmail);
                }

                if (!IsConfirmationTokenCorrect(dto.Token, userResult.SecurityStamp))
                {
                    return GetFailedConfirmEmailResult(dto, Errors.ExpiredToken);
                }

                userResult.IsEmailConfirmed = true;
                await RepositoryContext.Users.UpdateAsync(userResult.MapTo(new UserDto()));
                await RepositoryContext.UserRoles.CreateAsync(new UserRoleDto
                {
                    UserId = userResult.Id,
                    RoleId = _cache.Get<Middleware.Entities.Identity.Role>(Roles.MemberKey).Id,
                    UpdatedUserId = userResult.Id,
                    Status = Status.Active
                });
                result.Success(new ConfirmEmailViewModel());
            }
            catch (NotFoundException)
            {
                return GetFailedConfirmEmailResult(dto, Errors.NotFoundId);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ForgotPasswordViewModel>> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var result = new Result<ForgotPasswordViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<ForgotPasswordViewModel>
                    {
                        IsSucceeded = false,
                        Data = new ForgotPasswordViewModel { ForgotPassword = dto }
                    };
                }

                var userResult = await RepositoryContext.Users.GetByEmailAsync(dto.Email);
                if (!userResult.IsEmailConfirmed)
                {
                    return GetFailedForgotPasswordResult(dto, Errors.ResetPasswordConfirmEmail);
                }

                var token = GenerateToken(userResult.SecurityStamp);
                var callbackUrl = Urls.MailLinkAccountResetPassword + "?userId=" + userResult.Id + "&token=" + token;
                var mail = new MailDto
                {
                    ToEmailAddress = userResult.Email,
                    Subject = Titles.ResetPassword,
                    Message = Messages.ResetPassword(callbackUrl),
                    IsBodyPlainText = false
                };
                var mailResult = await _mailService.SendMailAsync(mail);
                if (mailResult.IsFailed())
                {
                    return mailResult.MapTo(result);
                }

                result.Success(new ForgotPasswordViewModel());
            }
            catch (NotFoundException)
            {
                return GetFailedForgotPasswordResult(dto, Errors.NotFoundEmail);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ResetPasswordViewModel>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var result = new Result<ResetPasswordViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<ResetPasswordViewModel>
                    {
                        IsSucceeded = false,
                        Data = new ResetPasswordViewModel { ResetPassword = dto }
                    };
                }

                var userResult = await RepositoryContext.Users.GetAsync(dto.UserId);
                if (!IsConfirmationTokenCorrect(dto.Token, userResult.SecurityStamp))
                {
                    return GetFailedResetPasswordResult(dto, Errors.ExpiredToken);
                }

                var hasher = new PasswordHasher<Middleware.Entities.Identity.User>();
                userResult.PasswordHash = hasher.HashPassword(userResult, dto.Password);
                userResult.SecurityStamp = Guid.NewGuid().ToString();
                await RepositoryContext.Users.UpdateAsync(userResult.MapTo(new UserDto()));
                result.Success(new ResetPasswordViewModel());
            }
            catch (NotFoundException)
            {
                return GetFailedResetPasswordResult(dto, Errors.ResetPasswordNotFoundId);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ChangePasswordViewModel>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var result = new Result<ChangePasswordViewModel>();
            try
            {
                if (!IsValid(dto))
                {
                    return new Result<ChangePasswordViewModel>
                    {
                        IsSucceeded = false,
                        Data = new ChangePasswordViewModel { ChangePassword = dto }
                    };
                }

                if (IsPasswordFormatValid(dto.NewPassword))
                {
                    return GetFailedChangePasswordResult(dto, Errors.Password);
                }

                var userResult = await RepositoryContext.Users.GetAsync(CurrentUserId);
                var hasher = new PasswordHasher<Middleware.Entities.Identity.User>();
                var hashResult = hasher.VerifyHashedPassword(userResult, userResult.PasswordHash, dto.CurrentPassword);
                if (hashResult != PasswordVerificationResult.Success)
                {
                    return GetFailedChangePasswordResult(dto, Errors.CurrentPassword);
                }

                userResult.PasswordHash = hasher.HashPassword(userResult, dto.NewPassword);
                await RepositoryContext.Users.UpdateAsync(userResult.MapTo(new UserDto()));
                result.Success(new ChangePasswordViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> GetUserNotificationListAsync(NotificationFilter filter)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                filter.ReceiverUserId = CurrentUserId;
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.AccountGetNotification, ObjectPropertyPath    = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.AccountDeleteNotification, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Title, DataPath        = Properties.Title, DataType       = TableCellDataType.Text},
                    new TableColumn {Header = Labels.IsOpened, DataPath     = Properties.IsOpened, DataType    = TableCellDataType.Boolean},
                    new TableColumn {Header = Labels.ReceivedDate, DataPath = Properties.UpdatedDate, DataType = TableCellDataType.Date}
                };
                if (!IsValid(filter))
                {
                    return GetFailedNotificationResult(new NotificationViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Users = new List<Middleware.Entities.Identity.User>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Notifications.GetListAsSelectedAsync(filter, n => new Notification
                {
                    Id = n.Id,
                    Title = n.Title,
                    IsOpened = n.IsOpened,
                    ReceiverUserId = n.ReceiverUserId,
                    UpdatedDate = n.UpdatedDate
                });
                result.Success(new NotificationViewModel { Notifications = list, Filter = filter, TableRowActionLinks = links, TableColumns = columns });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> OpenAndGetNotificationAsync(Guid? id)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                var entity = await RepositoryContext.Notifications.GetAsSelectedAsync(id, n => new Notification
                {
                    Id = n.Id,
                    IsOpened = n.IsOpened,
                    Title = n.Title,
                    Description = n.Description,
                    ReceiverUserId = n.ReceiverUserId
                });
                if (entity.ReceiverUserId != CurrentUserId)
                {
                    throw new NotPossessedException(nameof(Middleware.Entities.Identity.Notification));
                }

                await RepositoryContext.Notifications.OpenNotificationAsync(entity);
                result.Success(new NotificationViewModel { NotificationDetail = entity });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> DeleteNotificationAsync(Guid? id)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                var entity = await RepositoryContext.Notifications.GetAsSelectedAsync(id, n => new Notification
                {
                    Id = n.Id,
                    ReceiverUserId = n.ReceiverUserId
                });
                if (entity.ReceiverUserId != CurrentUserId)
                {
                    throw new NotPossessedException(nameof(Middleware.Entities.Identity.Notification));
                }

                await RepositoryContext.Notifications.DeleteAsync(id);
                result.Success(new NotificationViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        #region Helper Methods

        private static async Task SignInAsync(HttpContext httpContext, Middleware.Entities.Identity.User user, bool rememberMe)
        {
            var claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claimList, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };
            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authenticationProperties);
        }

        private static bool IsPasswordFormatValid(string password)
        {
            return password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Any(char.IsSymbol);
        }

        private static string GenerateToken(string securityStamp)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(securityStamp))));
        }

        private static bool IsConfirmationTokenCorrect(string token, string securityStamp)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(Convert.FromBase64String(token)))) == securityStamp;
        }

        #endregion

        #region Failed Results

        private static Result<LoginViewModel> GetFailedLoginResult(LoginDto dto, string message)
        {
            return new Result<LoginViewModel>
            {
                IsSucceeded = false,
                Data = new LoginViewModel { Login = dto, NotifyMessage = message }
            };
        }

        private static Result<RegisterViewModel> GetFailedRegisterResult(RegisterDto dto, string message)
        {
            return new Result<RegisterViewModel>
            {
                IsSucceeded = false,
                Data = new RegisterViewModel { Register = dto, NotifyMessage = message }
            };
        }

        private static Result<ConfirmEmailViewModel> GetFailedConfirmEmailResult(ConfirmEmailDto dto, string message)
        {
            return new Result<ConfirmEmailViewModel>
            {
                IsSucceeded = false,
                Data = new ConfirmEmailViewModel { ConfirmEmail = dto, NotifyMessage = message }
            };
        }

        private static Result<ForgotPasswordViewModel> GetFailedForgotPasswordResult(ForgotPasswordDto dto, string message)
        {
            return new Result<ForgotPasswordViewModel>
            {
                IsSucceeded = false,
                Data = new ForgotPasswordViewModel { ForgotPassword = dto, NotifyMessage = message }
            };
        }

        private static Result<ResetPasswordViewModel> GetFailedResetPasswordResult(ResetPasswordDto dto, string message)
        {
            return new Result<ResetPasswordViewModel>
            {
                IsSucceeded = false,
                Data = new ResetPasswordViewModel { ResetPassword = dto, NotifyMessage = message }
            };
        }

        private static Result<ChangePasswordViewModel> GetFailedChangePasswordResult(ChangePasswordDto dto, string message)
        {
            return new Result<ChangePasswordViewModel>
            {
                IsSucceeded = false,
                Data = new ChangePasswordViewModel { ChangePassword = dto, NotifyMessage = message }
            };
        }

        private static Result<NotificationViewModel> GetFailedNotificationResult(NotificationViewModel viewModel)
        {
            return new Result<NotificationViewModel>
            {
                IsSucceeded = false,
                Data = viewModel
            };
        }

        #endregion
    }
}