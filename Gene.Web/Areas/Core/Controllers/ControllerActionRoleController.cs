using System;
using System.Threading.Tasks;
using Gene.Business.IServices.Core;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Core;
using Gene.Middleware.Extensions;
using Gene.Middleware.Filters.Core;
using Gene.Middleware.ViewModels.Core;
using Gene.Web.Attributes;
using Gene.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Gene.Web.Areas.Core.Controllers
{
    [Area(nameof(Core))]
    [Route(Urls.DefaultAreaFormat)]
    [GeneAuthorize]
    public class ControllerActionRoleController : BaseController<IControllerActionRoleService>
    {
        private readonly IControllerActionRoleService _controllerActionRoleService;

        public ControllerActionRoleController(IControllerActionRoleService controllerActionRoleService) : base(controllerActionRoleService)
        {
            _controllerActionRoleService = controllerActionRoleService;
        }

        // GET: /Core/ControllerActionRole/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _controllerActionRoleService.GetListAsync(new ControllerActionRoleFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerActionRole/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ControllerActionRoleViewModel viewModel)
        {
            var result = await _controllerActionRoleService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/ControllerActionRole/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _controllerActionRoleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/ControllerActionRole/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _controllerActionRoleService.GetRelatedModelsAsync(new ControllerActionRoleViewModel {ControllerActionRole = new ControllerActionRoleDto()});
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerActionRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ControllerActionRoleViewModel viewModel)
        {
            var result = await _controllerActionRoleService.CreateAsync(viewModel.ControllerActionRole);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/ControllerActionRole/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _controllerActionRoleService.GetAsync(id);
            result.Data = (await _controllerActionRoleService.GetRelatedModelsAsync(result.Data)).Data;
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerActionRole/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ControllerActionRoleViewModel viewModel)
        {
            var result = await _controllerActionRoleService.UpdateAsync(viewModel.ControllerActionRole);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/ControllerActionRole/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _controllerActionRoleService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerActionRole/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _controllerActionRoleService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}