using System;
using System.Security.Claims;

namespace Gene.Middleware.Extensions
{
    public static class Claims
    {
        public static Guid? GetId(this ClaimsPrincipal principal)
        {
            return !string.IsNullOrEmpty(principal?.FindFirstValue(ClaimTypes.NameIdentifier)) ? Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)) : (Guid?)null;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.Email);
        }
    }
}