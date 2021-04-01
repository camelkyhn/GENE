using Gene.Business.IServices.Core;
using Gene.Middleware.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Gene.Web.Extensions
{
    public static class ActionExecuting
    {
        public static string GetControllerName(this ActionExecutingContext context)
        {
            return ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor) context.ActionDescriptor).ControllerName;
        }

        public static string GetActionName(this ActionExecutingContext context)
        {
            return ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor) context.ActionDescriptor).ActionName;
        }

        public static bool HasAccess(this ActionExecutingContext context)
        {
            var service   = context.HttpContext.RequestServices.GetRequiredService<IControllerActionRoleService>();
            var hasAccess = service.HasAccess(context.HttpContext.User.GetId(), context.GetControllerName(), context.GetActionName());
            if (hasAccess.IsFailed() || hasAccess.Data == false)
            {
                return false;
            }

            return true;
        }

        public static RedirectToActionResult RedirectToLogin(this ActionExecutingContext context)
        {
            return context.HttpContext.Request.Path.HasValue ?
                new RedirectToActionResult("Login", "Account", new {area = "Identity", ReturnUrl = context.HttpContext.Request.Path}) :
                new RedirectToActionResult("Login", "Account", new {area = "Identity"});
        }

        public static RedirectToActionResult RedirectToAccessDenied(this ActionExecutingContext context)
        {
            return new RedirectToActionResult("AccessDenied", "Account", new {area = "Identity"});
        }
    }
}