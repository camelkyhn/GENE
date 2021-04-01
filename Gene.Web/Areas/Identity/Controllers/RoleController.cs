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
    public class RoleController : BaseController<IRoleService>
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) : base(roleService)
        {
            _roleService = roleService;
        }

        // GET: /Identity/Role/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetListAsync(new RoleFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Role/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] RoleViewModel viewModel)
        {
            var result = await _roleService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Role/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _roleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoleViewModel {Role = new RoleDto()});
        }

        // POST: /Identity/Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RoleViewModel viewModel)
        {
            var result = await _roleService.CreateAsync(viewModel.Role);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/Role/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _roleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Role/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] RoleViewModel viewModel)
        {
            var result = await _roleService.UpdateAsync(viewModel.Role);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/Role/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _roleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Role/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _roleService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}