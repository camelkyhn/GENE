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
    public class ControllerController : BaseController<IControllerService>
    {
        private readonly IControllerService _controllerService;

        public ControllerController(IControllerService controllerService) : base(controllerService)
        {
            _controllerService = controllerService;
        }

        // GET: /Core/Controller/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _controllerService.GetListAsync(new ControllerFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Controller/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ControllerViewModel viewModel)
        {
            var result = await _controllerService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Controller/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _controllerService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Controller/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _controllerService.GetRelatedModelsAsync(new ControllerViewModel {Controller = new ControllerDto()});
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ControllerViewModel viewModel)
        {
            var result = await _controllerService.CreateAsync(viewModel.Controller);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Controller/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _controllerService.GetAsync(id);
            result.Data = (await _controllerService.GetRelatedModelsAsync(result.Data)).Data;
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Controller/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ControllerViewModel viewModel)
        {
            var result = await _controllerService.UpdateAsync(viewModel.Controller);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Controller/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _controllerService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Controller/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _controllerService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}