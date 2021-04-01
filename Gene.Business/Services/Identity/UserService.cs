using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Business.IServices.Identity;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Identity;
using Gene.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Gene.Business.Services.Identity
{
    public class UserService : Service, IUserService
    {
        private readonly IMemoryCache _cache;

        public UserService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext, IMemoryCache cache) : base(httpContextAccessor, repositoryContext)
        {
            _cache = cache;
        }

        public async Task<Result<UserViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<UserViewModel>();
            try
            {
                var entity = await RepositoryContext.Users.GetAsSelectedAsync(id, u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    LockoutEnd = u.LockoutEnd,
                    PhoneNumber = u.PhoneNumber,
                    AccessFailedCount = u.AccessFailedCount,
                    IsEmailConfirmed = u.IsEmailConfirmed,
                    IsPhoneNumberConfirmed = u.IsPhoneNumberConfirmed,
                    IsEmailEnabled = u.IsEmailEnabled,
                    IsSmsEnabled = u.IsSmsEnabled,
                    IsLockoutEnabled = u.IsLockoutEnabled,
                    IsTwoFactorEnabled = u.IsTwoFactorEnabled,
                    CreatedUser = new User { Email = u.CreatedUser.Email },
                    UpdatedUser = new User { Email = u.UpdatedUser.Email },
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate,
                    Status = u.Status
                });
                result.Success(new UserViewModel { UserDetail = entity, User = entity.MapTo(new UserDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserViewModel>> GetListAsync(UserFilter filter)
        {
            var result = new Result<UserViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.UserDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.UserEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.UserDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Email, DataPath            = Properties.Email, DataType            = TableCellDataType.Text},
                    new TableColumn {Header = Labels.IsEmailConfirmed, DataPath = Properties.IsEmailConfirmed, DataType = TableCellDataType.Boolean},
                    new TableColumn {Header = Labels.Status, DataPath           = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath      = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath      = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return GetFailedResult(new UserViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Users = new List<Middleware.Entities.Identity.User>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Users.GetListAsSelectedAsync(filter, u => new User
                {
                    Id = u.Id,
                    Email = u.Email,
                    IsEmailConfirmed = u.IsEmailConfirmed,
                    IsEmailEnabled = u.IsEmailEnabled,
                    UpdatedUser = new User { Email = u.UpdatedUser.Email },
                    UpdatedDate = u.UpdatedDate,
                    Status = u.Status
                });
                result.Success(new UserViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Users = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserViewModel>> CreateAsync(UserDto dto)
        {
            var result = new Result<UserViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new UserViewModel { User = dto });
                }

                if (RepositoryContext.Users.IsEmailTaken(dto.Email))
                {
                    return GetFailedResult(new UserViewModel { User = dto }, "This email is already taken!");
                }

                var createResult = await RepositoryContext.Users.CreateAsync(dto);
                await RepositoryContext.UserRoles.CreateAsync(new UserRoleDto
                {
                    UserId = createResult.Id,
                    RoleId = _cache.Get<Middleware.Entities.Identity.Role>(Roles.MemberKey).Id,
                    UpdatedUserId = dto.UpdatedUserId,
                    Status = Status.Active
                });
                result.Success(new UserViewModel { UserDetail = createResult, User = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserViewModel>> UpdateAsync(UserDto dto)
        {
            var result = new Result<UserViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new UserViewModel { User = dto });
                }

                if (RepositoryContext.Users.IsEmailTaken(dto.Email, dto.Id))
                {
                    return GetFailedResult(new UserViewModel { User = dto }, "This email is already taken!");
                }

                var updateResult = await RepositoryContext.Users.UpdateAsync(dto);
                result.Success(new UserViewModel { UserDetail = updateResult, User = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<UserViewModel>();
            try
            {
                await RepositoryContext.Users.DeleteAsync(id);
                result.Success(new UserViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private static Result<UserViewModel> GetFailedResult(UserViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            return new Result<UserViewModel>
            {
                IsSucceeded = false,
                Data = viewModel
            };
        }
    }
}