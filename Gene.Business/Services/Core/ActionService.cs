using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gene.Business.IServices.Core;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Entities.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.System;
using Gene.Middleware.ViewModels.Core;
using Gene.Storage;
using Microsoft.AspNetCore.Http;

namespace Gene.Business.Services.Core
{
    public class ActionService : Service, IActionService
    {
        public ActionService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<ActionViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<ActionViewModel>();
            try
            {
                var entity = await RepositoryContext.Actions.GetAsSelectedAsync(id, e => new Middleware.Entities.Core.Action
                {
                    Id = e.Id,
                    Name = e.Name,
                    CreatedUser = new User { Email = e.CreatedUser.Email },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ActionViewModel { ActionDetail = entity, Action = entity.MapTo(new ActionDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ActionViewModel>> GetListAsync(ActionFilter filter)
        {
            var result = new Result<ActionViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.ActionDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.ActionEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.ActionDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Name, DataPath        = Properties.Name, DataType             = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return GetFailedResult(new ActionViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Actions = new List<Middleware.Entities.Core.Action>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Actions.GetListAsSelectedAsync(filter, e => new Middleware.Entities.Core.Action
                {
                    Id = e.Id,
                    Name = e.Name,
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ActionViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Actions = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ActionViewModel>> CreateAsync(ActionDto dto)
        {
            var result = new Result<ActionViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new ActionViewModel { Action = dto });
                }

                var createResult = await RepositoryContext.Actions.CreateAsync(dto);
                result.Success(new ActionViewModel { ActionDetail = createResult, Action = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ActionViewModel>> UpdateAsync(ActionDto dto)
        {
            var result = new Result<ActionViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new ActionViewModel { Action = dto });
                }

                var updateResult = await RepositoryContext.Actions.UpdateAsync(dto);
                result.Success(new ActionViewModel { ActionDetail = updateResult, Action = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ActionViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<ActionViewModel>();
            try
            {
                await RepositoryContext.Actions.DeleteAsync(id);
                result.Success(new ActionViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private static Result<ActionViewModel> GetFailedResult(ActionViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            return new Result<ActionViewModel>
            {
                IsSucceeded = false,
                Data = viewModel
            };
        }
    }
}