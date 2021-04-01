using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Gene.Business.IServices.Core;
using Gene.Business.IServices.Identity;
using Gene.Middleware.Bases;
using Gene.Middleware.Entities.Core;
using Gene.Middleware.Exceptions;
using Gene.Middleware.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gene.Web.TagHelpers
{
    [HtmlTargetElement("gene-menu")]
    public class GeneMenuTagHelper : BaseTagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        private readonly ClaimsPrincipal _user;
        private readonly IHtmlHelper _htmlHelper;
        private readonly INotificationService _notificationService;
        private readonly IControllerActionRoleService _controllerActionRoleService;

        public GeneMenuTagHelper(IHttpContextAccessor httpContextAccessor, IHtmlHelper htmlHelper, INotificationService notificationService, IControllerActionRoleService controllerActionRoleService)
        {
            if (httpContextAccessor.HttpContext != null)
            {
                _user = httpContextAccessor.HttpContext.User;
            }

            _htmlHelper                  = htmlHelper;
            _notificationService         = notificationService;
            _controllerActionRoleService = controllerActionRoleService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            var html = await ToHtmlAsync();
            output.Content.SetHtmlContent(html);
        }

        private async Task<string> ToHtmlAsync()
        {
            var dynamicMenuItemsHtml = await ToDynamicMenuItemsHtmlAsync();
            var notificationsHtml    = await ToNotificationsHtmlAsync();
            var loginHtml            = ToLoginHtml();
            const string tagName     = "ul";
            return 
                GetOpeningTag(tagName) +
                    "<li class='hoverable'>" +
                        "<div class='user-view'>" +
                        "<a class='sidenav-close' href='javascript:void(0);'><i class='material-icons white-text right'>clear</i></a>" +
                        "<a href='/'><img alt='pp' class='responsive-img' src='/img/Logo.jpeg'/></a>" +
                        "<a href='/'><strong class='white-text name center'>Gene Software</strong></a>" +
                        "<a href='/Home/Contact'><span class='white-text email center'>Contact : info@gene.com</span></a>" +
                        "</div>" +
                    "</li>" +
                    "<li class='hoverable'><a class='collapsible-header waves-effect waves-red' href='/Home/Index'><i class='material-icons'>home</i>Home</a></li>" +
                    "<li class='hoverable'><a class='collapsible-header waves-effect waves-red' href='/Home/About'><i class='material-icons'>info</i>About</a></li>" +
                    "<li class='hoverable'><a class='collapsible-header waves-effect waves-red' href='/Home/Contact'><i class='material-icons'>contact_mail</i>Contact</a></li>" +
                    dynamicMenuItemsHtml +
                    notificationsHtml +
                    loginHtml +
                GetClosingTag(tagName);
        }

        private async Task<string> ToDynamicMenuItemsHtmlAsync()
        {
            if (_user.GetId() == null)
            {
                return string.Empty;
            }

            var controllers = await _controllerActionRoleService.GetAuthorizedControllerListAsync(_user.GetId());
            if (controllers.IsFailed())
            {
                throw new OperationFailedException(nameof(ToDynamicMenuItemsHtmlAsync), nameof(GeneMenuTagHelper));
            }

            var areaList = controllers.Data.GroupBy(c => c.Area.Id).Select(x => x.First().Area).ToList();
            var areas = string.Empty;
            foreach (var area in areaList)
            {
                areas = areas +
                       "<li>" +
                           "<ul class='collapsible collapsible-accordion'>" +
                               "<li>" +
                                   "<a class='collapsible-header waves-effect waves-red'>" +
                                        $"{area.DisplayName}" +
                                        $"<i class='material-icons'>{area.IconText}</i>" +
                                   "</a>" +
                                   "<div class='collapsible-body'>" +
                                        $"<ul>{GetAreaControllers(area, controllers.Data)}</ul>" +
                                   "</div>" +
                               "</li>" +
                           "</ul>" +
                       "</li>";
            }

            return areas;
        }

        private string GetAreaControllers(Area area, List<Controller> controllerList)
        {
            var controllers = "";
            foreach (var controller in controllerList.Where(c => c.AreaId == area.Id))
            {
                controllers = controllers +
                              "<li class='hoverable'>" +
                                  $"<a href='/{area.Name}/{controller.Name}/Index' class='waves-effect waves-red'>" +
                                      $"{controller.DisplayName}" +
                                      $"<i class='material-icons'>{controller.IconText}</i>"  +
                                  "</a>" +
                              "</li>";
            }

            return controllers;
        }

        private async Task<string> ToNotificationsHtmlAsync()
        {
            if (_user.GetId() != null)
            {
                var newNotifications = await _notificationService.GetNewNotificationsCountAsync();
                return
                    "<li class='hoverable'>" +
                        "<a class='collapsible-header waves-effect waves-red' href='/Identity/Account/Notifications'>" +
                            "<i class='material-icons'>notifications</i>" +
                            "Notifications" +
                            (newNotifications.Data > 0 ? $"<span class='badge gene-badge'>{newNotifications.Data}</span>" : string.Empty) +
                        "</a>" +
                    "</li>";
            }

            return string.Empty;
        }

        private string ToLoginHtml()
        {
            (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            var tokenInput = GenerateAntiForgeryTokenInput(_htmlHelper.AntiForgeryToken());
            if (_user.GetId() != null)
            {
                return
                    "<li class='hoverable'>" +
                        "<a class='collapsible-header waves-effect waves-red' onclick='document.getElementById(\"logoutForm\").submit();'>" +
                            "<i class='material-icons'>clear</i>Logout" +
                            "<form id='logoutForm' method='post' action='/Identity/Account/Logout'>" +
                                tokenInput +
                            "</form>" +
                        "</a>" +
                    "</li>";
            }

            return 
                "<li class='hoverable'>" +
                    "<a class='collapsible-header waves-effect waves-red' href='/Identity/Account/Register'><i class='material-icons'>person_add</i>Register</a>" +
                "</li>" +
                "<li class='hoverable'>" +
                    "<a class='collapsible-header waves-effect waves-red' href='/Identity/Account/Login'><i class='material-icons'>vpn_key</i>Login</a>" +
                "</li>";
        }

        private static string GenerateAntiForgeryTokenInput(IHtmlContent content)
        {
            var stringWriter = new StringWriter();
            content.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
}