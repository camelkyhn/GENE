using System;
using System.Threading.Tasks;
using Gene.Business.IServices.Identity;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Identity;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Identity;
using Gene.Middleware.ViewModels.Identity;
using Gene.Web.Attributes;
using Gene.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gene.Web.Areas.Identity.Controllers
{
    [Area(nameof(Identity))]
    [Route(Urls.DefaultAreaFormat)]
    [GeneAuthorize]
    public class UserRoleController : BaseController<IUserRoleService>
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService) : base(userRoleService)
        {
            _userRoleService = userRoleService;
        }

        // GET: /Identity/UserRole/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _userRoleService.GetListAsync(new UserRoleFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/UserRole/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] UserRoleViewModel viewModel)
        {
            var result = await _userRoleService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/UserRole/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _userRoleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/UserRole/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _userRoleService.GetRelatedModelsAsync(new UserRoleViewModel {UserRole = new UserRoleDto()});
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/UserRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserRoleViewModel viewModel)
        {
            var result = await _userRoleService.CreateAsync(viewModel.UserRole);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/UserRole/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _userRoleService.GetAsync(id);
            result.Data = (await _userRoleService.GetRelatedModelsAsync(result.Data)).Data;
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/UserRole/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UserRoleViewModel viewModel)
        {
            var result = await _userRoleService.UpdateAsync(viewModel.UserRole);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/UserRole/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _userRoleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/UserRole/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _userRoleService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}