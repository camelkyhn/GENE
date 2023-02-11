using System;
using Gene.Business.IServices.System;
using Gene.Business.IServices.Identity;
using Gene.Business.IServices.Core;
using Gene.Business.Services.Core;
using Gene.Business.Services.Identity;
using Gene.Business.Services.System;
using Gene.Middleware.Constants;
using Gene.Middleware.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gene.Storage;
using Gene.Storage.Initializers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Gene.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/NotExist";
                    await next();
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            // Migrate and seed the database
            new AppDatabaseInitializer().Initialize(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAppServices(services);
            RegisterServices(services);
        }

        private void ConfigureAppServices(IServiceCollection services)
        {
            services.Configure<AppConfiguration>(Configuration.GetSection("Configuration"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        // Cookie settings
                        options.Cookie.HttpOnly   = false;
                        options.ExpireTimeSpan    = TimeSpan.FromDays(1);
                        options.LoginPath         = $"/{Middleware.Constants.Areas.Identity}/{Middleware.Constants.Controllers.Account}/{Actions.Login}";
                        options.AccessDeniedPath  = $"/{Middleware.Constants.Areas.Identity}/{Middleware.Constants.Controllers.Account}/{Actions.AccessDenied}";
                        options.LogoutPath        = $"/{Middleware.Constants.Areas.Identity}/{Middleware.Constants.Controllers.Account}/{Actions.Logout}";
                        options.SlidingExpiration = true;
                    });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded    = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["Configuration:DbConnectionString"]));
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort          = 5001;
            });
            services.AddControllersWithViews();
            services.AddRazorPages(options =>
            {
                // Redirect "/" request to "/Identity/Account/Login" route.
                options.Conventions.AddAreaPageRoute(Middleware.Constants.Areas.Identity, $"/{Middleware.Constants.Controllers.Account}/{Actions.Login}", "");
            });
            services.AddAutoMapper(typeof(MapperProfile));
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IRepositoryContext, RepositoryContext>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddTransient<IActionService, ActionService>();
            services.AddTransient<IAreaService, AreaService>();
            services.AddTransient<IControllerService, ControllerService>();
            services.AddTransient<IControllerActionService, ControllerActionService>();
            services.AddTransient<IControllerActionRoleService, ControllerActionRoleService>();

            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<INotificationService, NotificationService>();
        }
    }
}