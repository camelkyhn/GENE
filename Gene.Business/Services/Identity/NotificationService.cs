using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Business.IServices.Identity;
using Gene.Business.IServices.System;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Dtos.System;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;
using Gene.Storage;
using Microsoft.AspNetCore.Http;

namespace Gene.Business.Services.Identity
{
    public class NotificationService : Service, INotificationService
    {
        private readonly IMailService _mailService;

        public NotificationService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext, IMailService mailService) : base(httpContextAccessor, repositoryContext)
        {
            _mailService = mailService;
        }

        public async Task<Result<NotificationViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                var entity = await RepositoryContext.Notifications.GetAsSelectedAsync(id, u => new Notification
                {
                    Id = u.Id,
                    IsOpened = u.IsOpened,
                    Title = u.Title,
                    Description = u.Description,
                    ReceiverUserId = u.ReceiverUserId,
                    ReceiverUser = new User { Id = u.ReceiverUser.Id, Email = u.ReceiverUser.Email },
                    CreatedUser = new User { Email = u.CreatedUser.Email },
                    UpdatedUser = new User { Email = u.UpdatedUser.Email },
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate,
                    Status = u.Status
                });
                result.Success(new NotificationViewModel { NotificationDetail = entity, Notification = entity.MapTo(new NotificationDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> GetListAsync(NotificationFilter filter)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.NotificationDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.NotificationEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.NotificationDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Title, DataPath       = Properties.Title, DataType             = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Receiver, DataPath    = Properties.ReceiverUserEmail, DataType = TableCellDataType.Text},
                    new TableColumn {Header = Labels.IsOpened, DataPath    = Properties.IsOpened, DataType          = TableCellDataType.Boolean},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType            = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType       = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType  = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return await GetFailedResultAsync(new NotificationViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Notifications = new List<Middleware.Entities.Identity.Notification>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Notifications.GetListAsSelectedAsync(filter, u => new Notification
                {
                    Id = u.Id,
                    IsOpened = u.IsOpened,
                    Title = u.Title,
                    ReceiverUser = new User { Email = u.ReceiverUser.Email },
                    UpdatedUser = new User { Email = u.UpdatedUser.Email },
                    UpdatedDate = u.UpdatedDate,
                    Status = u.Status
                });
                result.Success(new NotificationViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Notifications = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> CreateAsync(NotificationDto dto)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new NotificationViewModel { Notification = dto });
                }

                var createResult = await RepositoryContext.Notifications.CreateAsync(dto);
                var userResult = await RepositoryContext.Users.GetAsSelectedAsync(dto.ReceiverUserId, u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsEmailEnabled = u.IsEmailEnabled
                });
                if (userResult.IsEmailEnabled)
                {
                    await _mailService.SendMailAsync(new MailDto
                    {
                        ToEmailAddress = userResult.Email,
                        Subject = dto.Title,
                        Message = AddNotificationsLink(dto.Description)
                    });
                }

                result.Success(new NotificationViewModel { NotificationDetail = createResult, Notification = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> UpdateAsync(NotificationDto dto)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new NotificationViewModel { Notification = dto });
                }

                var updateResult = await RepositoryContext.Notifications.UpdateAsync(dto);
                result.Success(new NotificationViewModel { NotificationDetail = updateResult, Notification = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                await RepositoryContext.Notifications.DeleteAsync(id);
                result.Success(new NotificationViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> SendNotificationGlobalAsync(NotificationDto dto)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                dto.ReceiverUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new NotificationViewModel { Notification = dto });
                }

                var userList = await RepositoryContext.Users.GetListAsSelectedAsync(new UserFilter { IsAllData = true }, u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsEmailEnabled = u.IsEmailEnabled
                });
                await RepositoryContext.Notifications.CreateMultipleAsync(dto, userList);
                var mailResult = await _mailService.SendMultipleMailAsync(new MailDto { Subject = dto.Title, Message = dto.Description }, userList.Where(u => u.IsEmailEnabled).Select(u => u.Email));
                if (mailResult.IsFailed())
                {
                    return mailResult.MapTo(result);
                }

                result.Success(new NotificationViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<NotificationViewModel>> GetRelatedModelsAsync(NotificationViewModel viewModel)
        {
            var result = new Result<NotificationViewModel>();
            try
            {
                viewModel.Users = await RepositoryContext.Users.GetListAsSelectedAsync(new UserFilter { IsAllData = true }, e => new User
                {
                    Id = e.Id,
                    Email = e.Email
                });
                result.Success(viewModel);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<int?>> GetNewNotificationsCountAsync()
        {
            var result = new Result<int?>();
            try
            {
                var count = await RepositoryContext.Notifications.GetNewNotificationsCountAsync(CurrentUserId);
                result.Success(count);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<Result<NotificationViewModel>> GetFailedResultAsync(NotificationViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            var relatedModelsResult = await GetRelatedModelsAsync(viewModel);
            if (relatedModelsResult.IsFailed())
            {
                return relatedModelsResult;
            }

            return new Result<NotificationViewModel>
            {
                IsSucceeded = false,
                Data = relatedModelsResult.Data
            };
        }
    }
}