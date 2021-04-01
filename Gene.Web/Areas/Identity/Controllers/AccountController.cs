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
using Gene.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gene.Web.Areas.Identity.Controllers
{
    [Area(nameof(Identity))]
    [Route(Urls.DefaultAreaFormat)]
    public class AccountController : BaseController<IAccountService>
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService service) : base(service)
        {
            _accountService = service;
        }

        // GET: /Identity/Account/Index
        [HttpGet]
        [GeneAuthorize]
        public async Task<IActionResult> Index(string notifyMessage)
        {
            var result = await _accountService.GetProfileAsync(notifyMessage);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel {Login = new LoginDto()});
        }

        // POST: /Identity/Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel, string returnUrl)
        {
            var result = await _accountService.LoginAsync(viewModel.Login);
            return result.IsFailed() ? HandleResult(result) : (Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : Redirect("/"));
        }

        // POST: Identity/Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [GeneAuthorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync();
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Index", "Home");
        }

        // GET: /Identity/Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel {Register = new RegisterDto()});
        }

        // POST: /Identity/Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            var result = await _accountService.RegisterAsync(viewModel.Register);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Info", "Home", new InfoViewModel { Title = "Account Created", Message = "Check your mails and confirm your email address. Without confirmation, you can not login."});
        }

        // GET: /Identity/Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(Guid? userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(new ConfirmEmailDto {UserId = userId, Token = token});
            return result.IsFailed() ? HandleResult(result) : View();
        } 

        // GET: /Identity/Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel {ForgotPassword = new ForgotPasswordDto()});
        }

        // POST: /Identity/Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            var result = await _accountService.ForgotPasswordAsync(viewModel.ForgotPassword);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Info", "Home", new InfoViewModel { Title = "Reset Password Link Sent", Message = "In order to reset your password, please check your mails."});
        }

        // GET: /Identity/Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(Guid? userId, string token)
        {
            return View(new ResetPasswordViewModel {ResetPassword = new ResetPasswordDto {Token = token, UserId = userId}});
        }

        // POST: /Identity/Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            var result = await _accountService.ResetPasswordAsync(viewModel.ResetPassword);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Info", "Home", new InfoViewModel { Title = "Password Has Been Reset", Message = "Now, you can login with your new password."});
        }

        // GET: /Identity/Account/ChangePassword
        [HttpGet]
        [GeneAuthorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel {ChangePassword = new ChangePasswordDto()});
        }

        // POST: /Identity/Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [GeneAuthorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            var result = await _accountService.ChangePasswordAsync(viewModel.ChangePassword);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Info", "Home", new InfoViewModel { Title = "Password Has Been Changed", Message = "Now you can try after you logout."});
        }

        // GET: Identity/Account/AccessDenied
        [HttpGet]
        [GeneAuthorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Identity/Account/Notifications
        [HttpGet]
        [GeneAuthorize]
        public async Task<IActionResult> Notifications()
        {
            var result = await _accountService.GetUserNotificationListAsync(new NotificationFilter());
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // POST: /Identity/Account/Notifications
        [HttpPost]
        [ValidateAntiForgeryToken]
        [GeneAuthorize]
        public async Task<IActionResult> Notifications([FromForm] NotificationViewModel viewModel)
        {
            var result = await _accountService.GetUserNotificationListAsync(viewModel.Filter);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Account/GetNotification/5
        [HttpGet]
        [GeneAuthorize]
        public async Task<IActionResult> GetNotification(Guid? id)
        {
            var result = await _accountService.OpenAndGetNotificationAsync(id);
            return result.IsFailed() ? HandleResult(result) : View(result.Data);
        }

        // GET: /Identity/Account/DeleteNotification/5
        [HttpGet]
        [GeneAuthorize]
        public async Task<IActionResult> DeleteNotification(Guid? id)
        {
            var result = await _accountService.DeleteNotificationAsync(id);
            return result.IsFailed() ? HandleResult(result) : RedirectToAction("Notifications");
        }
    }
}