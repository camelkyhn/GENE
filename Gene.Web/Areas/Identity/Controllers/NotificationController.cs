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
    public class NotificationController : BaseController<INotificationService>
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) : base(notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: /Identity/Notification/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _notificationService.GetListAsync(new NotificationFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Notification/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] NotificationViewModel viewModel)
        {
            var result = await _notificationService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Notification/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _notificationService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Notification/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _notificationService.GetRelatedModelsAsync(new NotificationViewModel {Notification = new NotificationDto()});
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Notification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NotificationViewModel viewModel)
        {
            var result = await _notificationService.CreateAsync(viewModel.Notification);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/Notification/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _notificationService.GetAsync(id);
            result.Data = (await _notificationService.GetRelatedModelsAsync(result.Data)).Data;
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Notification/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] NotificationViewModel viewModel)
        {
            var result = await _notificationService.UpdateAsync(viewModel.Notification);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/Notification/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _notificationService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Notification/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _notificationService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/Notification/SendGlobalNotification
        [HttpGet]
        public IActionResult SendGlobalNotification()
        {
            return View(new NotificationViewModel {Notification = new NotificationDto()});
        }

        // POST: /Identity/Notification/SendGlobalNotification
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendGlobalNotification([FromForm] NotificationViewModel model)
        {
            var result = await _notificationService.SendNotificationGlobalAsync(model.Notification);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}