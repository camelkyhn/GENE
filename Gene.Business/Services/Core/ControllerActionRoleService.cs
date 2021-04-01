using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Business.IServices.Core;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;
using Gene.Storage;
using Microsoft.AspNetCore.Http;

namespace Gene.Business.Services.Core
{
    public class ControllerActionRoleService : Service, IControllerActionRoleService
    {
        public ControllerActionRoleService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<List<Middleware.Entities.Core.Controller>>> GetAuthorizedControllerListAsync(Guid? userId)
        {
            var result = new Result<List<Middleware.Entities.Core.Controller>>();
            try
            {
                var userRoleIdListResult = await RepositoryContext.UserRoles.GetUserRoleIdListAsync(userId);
                var controllerList = RepositoryContext.ControllerActionRoles.GetAuthorizedControllerList(userRoleIdListResult);
                result.Success(controllerList);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public Result<bool?> HasAccess(Guid? userId, string controllerName, string actionName)
        {
            var result = new Result<bool?>();
            try
            {
                var userRoleListResult = RepositoryContext.UserRoles.GetListAsSelectedAsync(new UserRoleFilter { UserId = userId }, ur => new UserRole
                {
                    Id = ur.Id,
                    UserId = ur.UserId,
                    RoleId = ur.RoleId
                }).Result;
                var hasAccess = RepositoryContext.ControllerActionRoles.HasAccess(userRoleListResult.Select(ur => ur.RoleId), controllerName, actionName);
                result.Success(hasAccess);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                var entity = await RepositoryContext.ControllerActionRoles.GetAsSelectedAsync(id, e => new ControllerActionRole
                {
                    Id = e.Id,
                    ControllerActionId = e.ControllerActionId,
                    ControllerAction = new ControllerAction
                    {
                        Id = e.ControllerAction.Id,
                        ControllerId = e.ControllerAction.ControllerId,
                        Controller = new Controller { Id = e.ControllerAction.Controller.Id, Name = e.ControllerAction.Controller.Name },
                        ActionId = e.ControllerAction.ActionId,
                        Action = new Middleware.Entities.Core.Action { Id = e.ControllerAction.Action.Id, Name = e.ControllerAction.Action.Name }
                    },
                    RoleId = e.RoleId,
                    Role = new Role { Id = e.Role.Id, Name = e.Role.Name },
                    CreatedUser = new User { Email = e.CreatedUser.Email },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerActionRoleViewModel { ControllerActionRoleDetail = entity, ControllerActionRole = entity.MapTo(new ControllerActionRoleDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> GetListAsync(ControllerActionRoleFilter filter)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.ControllerActionRoleDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.ControllerActionRoleEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.ControllerActionRoleDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Controller, DataPath  = Properties.ControllerActionControllerName, DataType = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Action, DataPath      = Properties.ControllerActionActionName, DataType     = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Role, DataPath        = Properties.RoleName, DataType                       = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType                         = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType                    = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType               = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return await GetFailedResultAsync(new ControllerActionRoleViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        ControllerActionRoles = new List<Middleware.Entities.Core.ControllerActionRole>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.ControllerActionRoles.GetListAsSelectedAsync(filter, e => new ControllerActionRole
                {
                    Id = e.Id,
                    ControllerAction = new ControllerAction
                    {
                        Controller = new Controller { Name = e.ControllerAction.Controller.Name },
                        Action = new Middleware.Entities.Core.Action { Name = e.ControllerAction.Action.Name }
                    },
                    Role = new Role { Name = e.Role.Name },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerActionRoleViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, ControllerActionRoles = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> CreateAsync(ControllerActionRoleDto dto)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionRoleViewModel { ControllerActionRole = dto });
                }

                if (RepositoryContext.ControllerActionRoles.IsExistingToAdd(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionRoleViewModel { ControllerActionRole = dto }, Errors.ExistingData);
                }

                var createResult = await RepositoryContext.ControllerActionRoles.CreateAsync(dto);
                result.Success(new ControllerActionRoleViewModel { ControllerActionRoleDetail = createResult, ControllerActionRole = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> UpdateAsync(ControllerActionRoleDto dto)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionRoleViewModel { ControllerActionRole = dto });
                }

                if (RepositoryContext.ControllerActionRoles.IsExistingToUpdate(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionRoleViewModel { ControllerActionRole = dto }, Errors.ExistingData);
                }

                var updateResult = await RepositoryContext.ControllerActionRoles.UpdateAsync(dto);
                result.Success(new ControllerActionRoleViewModel { ControllerActionRoleDetail = updateResult, ControllerActionRole = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                await RepositoryContext.ControllerActionRoles.DeleteAsync(id);
                result.Success(new ControllerActionRoleViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionRoleViewModel>> GetRelatedModelsAsync(ControllerActionRoleViewModel viewModel)
        {
            var result = new Result<ControllerActionRoleViewModel>();
            try
            {
                viewModel.ControllerActions = await RepositoryContext.ControllerActions.GetListAsSelectedAsync(new ControllerActionFilter { IsAllData = true }, e => new ControllerAction
                {
                    Id = e.Id,
                    Controller = new Controller { Name = e.Controller.Name },
                    Action = new Middleware.Entities.Core.Action { Name = e.Action.Name }
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

        private async Task<Result<ControllerActionRoleViewModel>> GetFailedResultAsync(ControllerActionRoleViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            var relatedModelsResult = await GetRelatedModelsAsync(viewModel);
            if (relatedModelsResult.IsFailed())
            {
                return relatedModelsResult;
            }

            return new Result<ControllerActionRoleViewModel>
            {
                IsSucceeded = false,
                Data = relatedModelsResult.Data
            };
        }
    }
}