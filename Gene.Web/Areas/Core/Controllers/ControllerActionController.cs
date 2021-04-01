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
    public class ControllerActionController : BaseController<IControllerActionService>
    {
        private readonly IControllerActionService _controllerActionService;

        public ControllerActionController(IControllerActionService controllerActionService) : base(controllerActionService)
        {
            _controllerActionService = controllerActionService;
        }

        // GET: /Core/ControllerAction/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _controllerActionService.GetListAsync(new ControllerActionFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerAction/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ControllerActionViewModel viewModel)
        {
            var result = await _controllerActionService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/ControllerAction/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _controllerActionService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/ControllerAction/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _controllerActionService.GetRelatedModelsAsync(new ControllerActionViewModel {ControllerAction = new ControllerActionDto()});
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerAction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ControllerActionViewModel viewModel)
        {
            var result = await _controllerActionService.CreateAsync(viewModel.ControllerAction);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/ControllerAction/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _controllerActionService.GetAsync(id);
            result.Data = (await _controllerActionService.GetRelatedModelsAsync(result.Data)).Data;
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerAction/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ControllerActionViewModel viewModel)
        {
            var result = await _controllerActionService.UpdateAsync(viewModel.ControllerAction);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/ControllerAction/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _controllerActionService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/ControllerAction/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _controllerActionService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}