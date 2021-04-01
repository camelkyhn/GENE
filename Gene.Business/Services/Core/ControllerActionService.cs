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
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;
using Gene.Storage;
using Microsoft.AspNetCore.Http;

namespace Gene.Business.Services.Core
{
    public class ControllerActionService : Service, IControllerActionService
    {
        public ControllerActionService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<ControllerActionViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                var entity = await RepositoryContext.ControllerActions.GetAsSelectedAsync(id, e => new ControllerAction
                {
                    Id = e.Id,
                    ControllerId = e.ControllerId,
                    Controller = new Controller { Id = e.Controller.Id, Name = e.Controller.Name },
                    ActionId = e.ActionId,
                    Action = new Middleware.Entities.Core.Action { Id = e.Action.Id, Name = e.Action.Name },
                    CreatedUser = new User { Email = e.CreatedUser.Email },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerActionViewModel { ControllerActionDetail = entity, ControllerAction = entity.MapTo(new ControllerActionDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionViewModel>> GetListAsync(ControllerActionFilter filter)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.ControllerActionDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.ControllerActionEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.ControllerActionDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Controller, DataPath  = Properties.ControllerName, DataType   = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Action, DataPath      = Properties.ActionName, DataType       = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return await GetFailedResultAsync(new ControllerActionViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        ControllerActions = new List<Middleware.Entities.Core.ControllerAction>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.ControllerActions.GetListAsSelectedAsync(filter, e => new ControllerAction
                {
                    Id = e.Id,
                    Controller = new Controller { Name = e.Controller.Name },
                    Action = new Middleware.Entities.Core.Action { Name = e.Action.Name },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerActionViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, ControllerActions = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionViewModel>> CreateAsync(ControllerActionDto dto)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionViewModel { ControllerAction = dto });
                }

                if (RepositoryContext.ControllerActions.IsExistingToAdd(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionViewModel { ControllerAction = dto }, Errors.ExistingData);
                }

                var createResult = await RepositoryContext.ControllerActions.CreateAsync(dto);
                result.Success(new ControllerActionViewModel { ControllerActionDetail = createResult, ControllerAction = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionViewModel>> UpdateAsync(ControllerActionDto dto)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionViewModel { ControllerAction = dto });
                }

                if (RepositoryContext.ControllerActions.IsExistingToUpdate(dto))
                {
                    return await GetFailedResultAsync(new ControllerActionViewModel { ControllerAction = dto }, Errors.ExistingData);
                }

                var updateResult = await RepositoryContext.ControllerActions.UpdateAsync(dto);
                result.Success(new ControllerActionViewModel { ControllerActionDetail = updateResult, ControllerAction = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                await RepositoryContext.ControllerActions.DeleteAsync(id);
                result.Success(new ControllerActionViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerActionViewModel>> GetRelatedModelsAsync(ControllerActionViewModel viewModel)
        {
            var result = new Result<ControllerActionViewModel>();
            try
            {
                viewModel.Controllers = await RepositoryContext.Controllers.GetListAsSelectedAsync(new ControllerFilter { IsAllData = true }, e => new Controller
                {
                    Id = e.Id,
                    Name = e.Name
                });
                viewModel.Actions = await RepositoryContext.Actions.GetListAsSelectedAsync(new ActionFilter { IsAllData = true }, e => new Middleware.Entities.Core.Action
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

        private async Task<Result<ControllerActionViewModel>> GetFailedResultAsync(ControllerActionViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            var relatedModelsResult = await GetRelatedModelsAsync(viewModel);
            if (relatedModelsResult.IsFailed())
            {
                return relatedModelsResult;
            }

            return new Result<ControllerActionViewModel>
            {
                IsSucceeded = false,
                Data = relatedModelsResult.Data
            };
        }
    }
}