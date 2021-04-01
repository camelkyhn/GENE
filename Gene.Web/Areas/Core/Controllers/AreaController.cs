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
    public class AreaController : BaseController<IAreaService>
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService) : base(areaService)
        {
            _areaService = areaService;
        }

        // GET: /Core/Area/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _areaService.GetListAsync(new AreaFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Area/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] AreaViewModel viewModel)
        {
            var result = await _areaService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Area/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _areaService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Core/Area/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new AreaViewModel {Area = new AreaDto()});
        }

        // POST: /Core/Area/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AreaViewModel viewModel)
        {
            var result = await _areaService.CreateAsync(viewModel.Area);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Area/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _areaService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Area/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] AreaViewModel viewModel)
        {
            var result = await _areaService.UpdateAsync(viewModel.Area);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Core/Area/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _areaService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Core/Area/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _areaService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}