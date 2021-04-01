using Gene.Middleware.Extensions;
using Gene.Web.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gene.Web.Attributes
{
    public class GeneAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.GetId() == null || context.HttpContext.User.Identity == null)
            {
                context.HttpContext.SignOutAsync();
                context.Result = context.RedirectToLogin();
            }
            else if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (!context.HasAccess())
                {
                    context.Result = context.RedirectToAccessDenied();
                }
            }
        }
    }
}