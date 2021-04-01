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

namespace Gene.Business.Services.Identity
{
    public class UserRoleService : Service, IUserRoleService
    {
        public UserRoleService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<UserRoleViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                var entity = await RepositoryContext.UserRoles.GetAsSelectedAsync(id, ur => new UserRole
                {
                    Id = ur.Id,
                    RoleId = ur.RoleId,
                    Role = new Role { Id = ur.Role.Id, Name = ur.Role.Name },
                    UserId = ur.UserId,
                    User = new User { Id = ur.User.Id, Email = ur.User.Email },
                    CreatedUser = new User { Email = ur.CreatedUser.Email },
                    UpdatedUser = new User { Email = ur.UpdatedUser.Email },
                    CreatedDate = ur.CreatedDate,
                    UpdatedDate = ur.UpdatedDate,
                    Status = ur.Status
                });
                result.Success(new UserRoleViewModel { UserRoleDetail = entity, UserRole = entity.MapTo(new UserRoleDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleViewModel>> GetListAsync(UserRoleFilter filter)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.UserRoleDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.UserRoleEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.UserRoleDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.User, DataPath        = Properties.UserEmail, DataType        = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Role, DataPath        = Properties.RoleName, DataType         = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return await GetFailedResultAsync(new UserRoleViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        UserRoles = new List<Middleware.Entities.Identity.UserRole>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.UserRoles.GetListAsSelectedAsync(filter, ur => new UserRole
                {
                    Id = ur.Id,
                    Role = new Role { Name = ur.Role.Name },
                    User = new User { Email = ur.User.Email },
                    UpdatedUser = new User { Email = ur.UpdatedUser.Email },
                    UpdatedDate = ur.UpdatedDate,
                    Status = ur.Status
                });
                result.Success(new UserRoleViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, UserRoles = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleViewModel>> CreateAsync(UserRoleDto dto)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                if (dto == null)
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = new UserRoleDto() }, Errors.EmptyModel);
                }

                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = dto });
                }

                if (RepositoryContext.UserRoles.IsExistingToAdd(dto))
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = dto }, Errors.ExistingData);
                }

                var createResult = await RepositoryContext.UserRoles.CreateAsync(dto);
                result.Success(new UserRoleViewModel { UserRoleDetail = createResult, UserRole = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleViewModel>> UpdateAsync(UserRoleDto dto)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                if (dto == null)
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = new UserRoleDto() }, Errors.EmptyModel);
                }

                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = dto });
                }

                if (RepositoryContext.UserRoles.IsExistingToUpdate(dto))
                {
                    return await GetFailedResultAsync(new UserRoleViewModel { UserRole = dto }, Errors.ExistingData);
                }

                var updateResult = await RepositoryContext.UserRoles.UpdateAsync(dto);
                result.Success(new UserRoleViewModel { UserRoleDetail = updateResult, UserRole = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                await RepositoryContext.UserRoles.DeleteAsync(id);
                result.Success(new UserRoleViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserRoleViewModel>> GetRelatedModelsAsync(UserRoleViewModel viewModel)
        {
            var result = new Result<UserRoleViewModel>();
            try
            {
                viewModel.Users = await RepositoryContext.Users.GetListAsSelectedAsync(new UserFilter { IsAllData = true }, e => new User
                {
                    Id = e.Id,
                    Email = e.Email
                });
                viewModel.Roles = await RepositoryContext.Roles.GetListAsSelectedAsync(new RoleFilter { IsAllData = true }, e => new Role
                {
                    Id = e.Id,
                    Name = e.Name
                });
                result.Success(viewModel);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private async Task<Result<UserRoleViewModel>> GetFailedResultAsync(UserRoleViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            var relatedModelsResult = await GetRelatedModelsAsync(viewModel);
            if (relatedModelsResult.IsFailed())
            {
                return relatedModelsResult;
            }

            return new Result<UserRoleViewModel>
            {
                IsSucceeded = false,
                Data = relatedModelsResult.Data
            };
        }
    }
}