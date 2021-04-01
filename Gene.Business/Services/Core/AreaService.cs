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
    public class AreaService : Service, IAreaService
    {
        public AreaService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<AreaViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<AreaViewModel>();
            try
            {
                var entity = await RepositoryContext.Areas.GetAsSelectedAsync(id, e => new Middleware.Entities.Core.Area
                {
                    Id = e.Id,
                    Name = e.Name,
                    DisplayName = e.DisplayName,
                    IconText = e.IconText,
                    CreatedUser = new User { Email = e.CreatedUser.Email },
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    CreatedDate = e.CreatedDate,
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new AreaViewModel { AreaDetail = entity, Area = entity.MapTo(new AreaDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<AreaViewModel>> GetListAsync(AreaFilter filter)
        {
            var result = new Result<AreaViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.AreaDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.AreaEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.AreaDelete, ObjectPropertyPath = Properties.Id}
                };
                var columns = new[]
                {
                    new TableColumn {Header = Labels.Name, DataPath        = Properties.Id, DataType               = TableCellDataType.Text},
                    new TableColumn {Header = Labels.DisplayName, DataPath = Properties.DisplayName, DataType      = TableCellDataType.Text},
                    new TableColumn {Header = Labels.IconText, DataPath    = Properties.IconText, DataType         = TableCellDataType.Text},
                    new TableColumn {Header = Labels.Status, DataPath      = Properties.Status, DataType           = TableCellDataType.Enum},
                    new TableColumn {Header = Labels.UpdatedDate, DataPath = Properties.UpdatedDate, DataType      = TableCellDataType.Date},
                    new TableColumn {Header = Labels.UpdatedUser, DataPath = Properties.UpdatedUserEmail, DataType = TableCellDataType.Text}
                };
                if (!IsValid(filter))
                {
                    return GetFailedResult(new AreaViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Areas = new List<Middleware.Entities.Core.Area>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Areas.GetListAsSelectedAsync(filter, e => new Middleware.Entities.Core.Area
                {
                    Id = e.Id,
                    Name = e.Name,
                    DisplayName = e.DisplayName,
                    IconText = e.IconText,
                    UpdatedUser = new User { Email = e.UpdatedUser.Email },
                    UpdatedDate = e.UpdatedDate,
                    Status = e.Status
                });
                result.Success(new AreaViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Areas = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<AreaViewModel>> CreateAsync(AreaDto dto)
        {
            var result = new Result<AreaViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new AreaViewModel { Area = dto });
                }

                var createResult = await RepositoryContext.Areas.CreateAsync(dto);
                result.Success(new AreaViewModel { AreaDetail = createResult, Area = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<AreaViewModel>> UpdateAsync(AreaDto dto)
        {
            var result = new Result<AreaViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new AreaViewModel { Area = dto });
                }

                var updateResult = await RepositoryContext.Areas.UpdateAsync(dto);
                result.Success(new AreaViewModel { AreaDetail = updateResult, Area = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<AreaViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<AreaViewModel>();
            try
            {
                await RepositoryContext.Areas.DeleteAsync(id);
                result.Success(new AreaViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private static Result<AreaViewModel> GetFailedResult(AreaViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            return new Result<AreaViewModel>
            {
                IsSucceeded = false,
                Data = viewModel
            };
        }
    }
}