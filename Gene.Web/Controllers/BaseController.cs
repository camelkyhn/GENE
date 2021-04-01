using System.Diagnostics;
using Gene.Business.IServices;
using Gene.Middleware.Bases;
using Gene.Middleware.Extensions;
using Gene.Middleware.System;
using Gene.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gene.Web.Controllers
{
    public class BaseController<TService> : Controller where TService : IService
    {
        private readonly TService _service;

        public BaseController(TService service)
        {
            _service = service;
        }

        protected IActionResult HandleResult<TViewModel>(Result<TViewModel> result) where TViewModel : BaseViewModel
        {
            if (result.IsFailedWithData())
            {
                return View(result.Data);
            }

            return RedirectToAction("Error", "Home", new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ExceptionType = result.ExceptionType, ExceptionMessage = result.ExceptionMessage});
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _service.ModelState = ModelState;
            base.OnActionExecuting(context);
        }
    }
}