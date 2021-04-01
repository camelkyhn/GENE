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
    public class ControllerService : Service, IControllerService
    {
        public ControllerService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<ControllerViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                var entity = await RepositoryContext.Controllers.GetAsSelectedAsync(id, e => new Controller
                {
                    Id = e.Id,
                    Name = e.Name,
                    DisplayName = e.DisplayName,
                    IconText = e.IconText,
                    AreaId = e.AreaId,
                    Area = new Area { Id = e.Area.Id, Name = e.Area.Name },
                    CreatedUser = new User { Email = e.CreatedUser.Email },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerViewModel { ControllerDetail = entity, Controller = entity.MapTo(new ControllerDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerViewModel>> GetListAsync(ControllerFilter filter)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.ControllerDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.ControllerEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.ControllerDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Name, DataPath        = Properties.Id, DataType               = TableCellDataType.Text},
                    new TableColumn {Header = Labels.DisplayName, DataPath = Properties.DisplayName, DataType      = TableCellDataType.Text},
                    new TableColumn {Header = Labels.IconText, DataPath    = Properties.IconText, DataType         = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Area, DataPath        = Properties.AreaName, DataType         = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return await GetFailedResultAsync(new ControllerViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Controllers = new List<Middleware.Entities.Core.Controller>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Controllers.GetListAsSelectedAsync(filter, e => new Controller
                {
                    Id = e.Id,
                    Name = e.Name,
                    DisplayName = e.DisplayName,
                    IconText = e.IconText,
                    Area = new Area { Name = e.Area.Name },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new ControllerViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Controllers = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerViewModel>> CreateAsync(ControllerDto dto)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerViewModel { Controller = dto });
                }

                var createResult = await RepositoryContext.Controllers.CreateAsync(dto);
                result.Success(new ControllerViewModel { ControllerDetail = createResult, Controller = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerViewModel>> UpdateAsync(ControllerDto dto)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return await GetFailedResultAsync(new ControllerViewModel { Controller = dto });
                }

                var updateResult = await RepositoryContext.Controllers.UpdateAsync(dto);
                result.Success(new ControllerViewModel { ControllerDetail = updateResult, Controller = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                await RepositoryContext.Controllers.DeleteAsync(id);
                result.Success(new ControllerViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<ControllerViewModel>> GetRelatedModelsAsync(ControllerViewModel viewModel)
        {
            var result = new Result<ControllerViewModel>();
            try
            {
                viewModel.Areas = await RepositoryContext.Areas.GetListAsSelectedAsync(new AreaFilter { IsAllData = true }, e => new Area
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

        private async Task<Result<ControllerViewModel>> GetFailedResultAsync(ControllerViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            var relatedModelsResult = await GetRelatedModelsAsync(viewModel);
            if (relatedModelsResult.IsFailed())
            {
                return relatedModelsResult;
            }

            return new Result<ControllerViewModel>
            {
                IsSucceeded = false,
                Data = relatedModelsResult.Data
            };
        }
    }
}