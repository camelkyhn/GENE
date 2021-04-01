using System;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;
using Gene.Middleware.Dtos.Identity;

namespace Gene.Middleware.ViewModels.Identity
{
    public class ProfileViewModel : BaseViewModel
    {
        [Display(Name = Labels.Email)]
        public string Email { get; set; }
        [Display(Name = Labels.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Display(Name = Labels.CreatedDate)]
        public DateTimeOffset? CreatedDate { get; set; }
        [Display(Name = Labels.IsEmailConfirmed)]
        public bool IsEmailConfirmed { get; set; }
        [Display(Name = Labels.IsPhoneNumberConfirmed)]
        public bool IsPhoneNumberConfirmed { get; set; }
        [Display(Name = Labels.IsEmailEnabled)]
        public bool IsEmailEnabled { get; set; }
        [Display(Name = Labels.IsSmsEnabled)]
        public bool IsSmsEnabled { get; set; }
    }

    public class LoginViewModel : BaseViewModel
    {
        public LoginDto Login { get; set; }
    }

    public class RegisterViewModel : BaseViewModel
    {
        public RegisterDto Register { get; set; }
    }

    public class ConfirmEmailViewModel : BaseViewModel
    {
        public ConfirmEmailDto ConfirmEmail { get; set; }
    }

    public class ForgotPasswordViewModel : BaseViewModel
    {
        public ForgotPasswordDto ForgotPassword { get; set; }
    }

    public class ResetPasswordViewModel : BaseViewModel
    {
        public ResetPasswordDto ResetPassword { get; set; }
    }

    public class ChangePasswordViewModel : BaseViewModel
    {
        public ChangePasswordDto ChangePassword { get; set; }
    }
}