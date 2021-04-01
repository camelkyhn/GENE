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
    public class UserController : BaseController<IUserService>
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }

        // GET: /Identity/User/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetListAsync(new UserFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/User/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromForm] UserViewModel viewModel)
        {
            var result = await _userService.GetListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/User/Detail/5
        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var result = await _userService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserViewModel {User = new UserDto()});
        }

        // POST: /Identity/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserViewModel viewModel)
        {
            var result = await _userService.CreateAsync(viewModel.User);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            var result = await _userService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UserViewModel viewModel)
        {
            var result = await _userService.UpdateAsync(viewModel.User);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }

        // GET: /Identity/User/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var result = await _userService.GetAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/User/Delete/5
        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([FromForm] Guid? id)
        {
            var result = await _userService.DeleteAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index");
        }
    }
}