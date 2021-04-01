using Gene.Business.IServices.System;
using Gene.Business.IServices.Identity;
using Gene.Business.IServices.Core;
using Gene.Business.Services.Core;
using Gene.Business.Services.Identity;
using Gene.Business.Services.System;
using Gene.Middleware.Extensions;
using Gene.Middleware.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gene.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gene.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServices<DatabaseContext>(EnvironmentVariable.DbConnectionString);
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache cache)
        {
            app.ConfigureApp<DatabaseContext>(env, cache);
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
