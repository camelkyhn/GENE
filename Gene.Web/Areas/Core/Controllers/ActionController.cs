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
    public class ActionController : BaseController<IActionService>
    {
        private readonly IActionService _actionService;

        public ActionController(IActionService actionService) : base(actionService)
        {
            _actionService = actionService;
        }

        // GET: /Core/Action/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _actionService.GetListAsync(new ActionFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Action/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] ActionViewModel viewModel)
        {
            var result = await _actionService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Action/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _actionService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Action/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ActionViewModel {Action = new ActionDto()});
        }

        // POST: /Core/Action/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ActionViewModel viewModel)
        {
            var result = await _actionService.CreateAsync(viewModel.Action);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Action/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _actionService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Action/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ActionViewModel viewModel)
        {
            var result = await _actionService.UpdateAsync(viewModel.Action);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Action/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _actionService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Action/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _actionService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}