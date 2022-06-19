using System;
using Gene.Middleware.Constants;
using Gene.Middleware.System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gene.Middleware.Extensions
{
    public static class ServiceCollection
    {
        public static void ConfigureServices<TContext>(this IServiceCollection services, string database) where TContext : DbContext
        {
            services.AddAuthenticationAndCookie();
            services.ConfigureCookiePolicyOptions();
            services.AddDbContext<TContext>(options => options.UseSqlServer(database));
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort          = 5001;
            });
            services.AddControllersWithViews();
            services.AddRazorPages(options =>
            {
                // Redirect "/" request to "/Identity/Account/Login" route.
                options.Conventions.AddAreaPageRoute(Areas.Identity, $"/{Controllers.Account}/{Actions.Login}", "");
            });
            services.AddAutoMapper(typeof(MapperProfile));
        }

        public static void ConfigureCookiePolicyOptions(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded    = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void AddAuthenticationAndCookie(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        // Cookie settings
                        options.Cookie.HttpOnly   = false;
                        options.ExpireTimeSpan    = TimeSpan.FromDays(1);
                        options.LoginPath         = "/Identity/Account/Login";
                        options.AccessDeniedPath  = "/Identity/Account/AccessDenied";
                        options.LogoutPath        = "/Identity/Account/Logout";
                        options.SlidingExpiration = true;
                    });
        }
    }
}