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
    public class RoleService : Service, IRoleService
    {
        public RoleService(IHttpContextAccessor httpContextAccessor, IRepositoryContext repositoryContext) : base(httpContextAccessor, repositoryContext)
        {
        }

        public async Task<Result<RoleViewModel>> GetAsync(Guid? id)
        {
            var result = new Result<RoleViewModel>();
            try
            {
                var entity = await RepositoryContext.Roles.GetAsSelectedAsync(id, r => new Role
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedUser = new User { Email = r.CreatedUser.Email },
                    UpdatedUser = new User { Email = r.UpdatedUser.Email },
                    CreatedDate = r.CreatedDate,
                    UpdatedDate = r.UpdatedDate,
                    Status = r.Status
                });
                result.Success(new RoleViewModel { RoleDetail = entity, Role = entity.MapTo(new RoleDto()) });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RoleViewModel>> GetListAsync(RoleFilter filter)
        {
            var result = new Result<RoleViewModel>();
            try
            {
                var links = new[]
                {
                    new TableRowActionLink {Text = LinkTexts.Detail, Url = Urls.RoleDetail, ObjectPropertyPath = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Edit, Url   = Urls.RoleEdit, ObjectPropertyPath   = Properties.Id},
                    new TableRowActionLink {Text = LinkTexts.Delete, Url = Urls.RoleDelete, ObjectPropertyPath = Properties.Id}
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
                    return GetFailedResult(new RoleViewModel
                    {
                        Filter = filter,
                        TableRowActionLinks = links,
                        TableColumns = columns,
                        Roles = new List<Middleware.Entities.Identity.Role>(),
                        NotifyMessage = ModelState.Values.First().Errors.FirstOrDefault()?.ErrorMessage
                    });
                }

                var list = await RepositoryContext.Roles.GetListAsSelectedAsync(filter, r => new Role
                {
                    Id = r.Id,
                    Name = r.Name,
                    UpdatedUser = new User { Email = r.UpdatedUser.Email },
                    UpdatedDate = r.UpdatedDate,
                    Status = r.Status
                });
                result.Success(new RoleViewModel { Filter = filter, TableRowActionLinks = links, TableColumns = columns, Roles = list });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RoleViewModel>> CreateAsync(RoleDto dto)
        {
            var result = new Result<RoleViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new RoleViewModel { Role = dto });
                }

                var createResult = await RepositoryContext.Roles.CreateAsync(dto);
                result.Success(new RoleViewModel { RoleDetail = createResult, Role = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RoleViewModel>> UpdateAsync(RoleDto dto)
        {
            var result = new Result<RoleViewModel>();
            try
            {
                dto.UpdatedUserId = CurrentUserId;
                if (!IsValid(dto))
                {
                    return GetFailedResult(new RoleViewModel { Role = dto });
                }

                var updateResult = await RepositoryContext.Roles.UpdateAsync(dto);
                result.Success(new RoleViewModel { RoleDetail = updateResult, Role = dto });
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<RoleViewModel>> DeleteAsync(Guid? id)
        {
            var result = new Result<RoleViewModel>();
            try
            {
                await RepositoryContext.Roles.DeleteAsync(id);
                result.Success(new RoleViewModel());
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        private static Result<RoleViewModel> GetFailedResult(RoleViewModel viewModel, string message = null)
        {
            viewModel.NotifyMessage = message ?? viewModel.NotifyMessage;
            return new Result<RoleViewModel>
            {
                IsSucceeded = false,
                Data = viewModel
            };
        }
    }
}