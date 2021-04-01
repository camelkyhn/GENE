using Gene.Middleware.Bases;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Gene.Middleware.Attributes;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Identity
{
    public class UserDto : BaseDto<Guid?>
    {
        [Required]
        [EmailAddress]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Email)]
        public string Email { get; set; }

        [Phone]
        [Display(Name = Labels.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = Labels.IsEmailEnabled)]
        public bool IsEmailEnabled { get; set; }

        [Display(Name = Labels.IsSmsEnabled)]
        public bool IsSmsEnabled { get; set; }

        [Display(Name = Labels.SecurityStamp)]
        public string SecurityStamp { get; set; }

        [Display(Name = Labels.IsEmailConfirmed)]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = Labels.IsPhoneNumberConfirmed)]
        public bool IsPhoneNumberConfirmed { get; set; }

        [Display(Name = Labels.IsTwoFactorEnabled)]
        public bool IsTwoFactorEnabled { get; set; }

        [Display(Name = Labels.AccessFailedCount)]
        public short AccessFailedCount { get; set; }

        [Display(Name = Labels.IsLockoutEnabled)]
        public bool IsLockoutEnabled { get; set; }

        [Display(Name = Labels.LockoutEnd)]
        public DateTimeOffset? LockoutEnd { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Email)]
        [PlaceHolder(PlaceHolders.Email)]
        public string Email { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Display(Name = Labels.Password)]
        [PlaceHolder(PlaceHolders.Password)]
        public string Password { get; set; }

        [Display(Name = Labels.RememberMe)]
        public bool RememberMe { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Email)]
        [PlaceHolder(PlaceHolders.Email)]
        public string Email { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Display(Name = Labels.Password)]
        [PlaceHolder(PlaceHolders.Password)]
        [Tooltip(Message = Tooltips.Password, Position = Positions.Bottom)]
        public string Password { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = Errors.ConfirmPassword)]
        [Display(Name                           = Labels.ConfirmPassword)]
        [PlaceHolder(PlaceHolders.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessage = Errors.PhoneNumber)]
        [StringLength(MaxLengths.TinyText)]
        [Display(Name = Labels.PhoneNumber)]
        [PlaceHolder(PlaceHolders.PhoneNumber)]
        [Tooltip(Message = Tooltips.PhoneNumber, Position = Positions.Bottom)]
        public string PhoneNumber { get; set; }
    }

    public class ConfirmEmailDto
    {
        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Email)]
        [PlaceHolder(PlaceHolders.Email)]
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Display(Name = Labels.Password)]
        [PlaceHolder(PlaceHolders.Password)]
        [Tooltip(Message = Tooltips.Password, Position = Positions.Bottom)]
        public string Password { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = Errors.ConfirmPassword)]
        [Display(Name                           = Labels.ConfirmPassword)]
        [PlaceHolder(PlaceHolders.ConfirmPassword)]
        public string ConfirmPassword { get; set; }

        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Display(Name = Labels.CurrentPassword)]
        [PlaceHolder(PlaceHolders.CurrentPassword)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Display(Name = Labels.NewPassword)]
        [PlaceHolder(PlaceHolders.NewPassword)]
        [Tooltip(Message = Tooltips.Password, Position = Positions.Bottom)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(MaxLengths.PasswordText, MinimumLength = MinLengths.PasswordText)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = Errors.ConfirmNewPassword)]
        [Display(Name                              = Labels.ConfirmPassword)]
        [PlaceHolder(PlaceHolders.ConfirmNewPassword)]
        public string ConfirmPassword { get; set; }
    }
}