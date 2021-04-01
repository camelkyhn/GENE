using Gene.Middleware.Bases;
using Gene.Middleware.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Identity
{
    [Table(nameof(User))]
    public class User : Entity<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [EmailAddress]
        [Display(Name = Labels.Email)]
        public string Email { get; set; }
        [Phone]
        [StringLength(MaxLengths.TinyText)]
        [Display(Name = Labels.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        [Display(Name = Labels.IsEmailEnabled)]
        public bool IsEmailEnabled { get; set; }
        [Display(Name = Labels.IsSmsEnabled)]
        public bool IsSmsEnabled { get; set; }
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

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        #region User Created Relations

        public virtual ICollection<User> CreatedUsers { get; set; }
        public virtual ICollection<Role> CreatedRoles { get; set; }
        public virtual ICollection<UserRole> CreatedUserRoles { get; set; }
        public virtual ICollection<Notification> CreatedNotifications { get; set; }

        public virtual ICollection<Core.Action> CreatedActions { get; set; }
        public virtual ICollection<Area> CreatedAreas { get; set; }
        public virtual ICollection<Controller> CreatedControllers { get; set; }
        public virtual ICollection<ControllerAction> CreatedControllerActions { get; set; }
        public virtual ICollection<ControllerActionRole> CreatedControllerActionRoles { get; set; }

        #endregion

        #region User Updated Relations

        public virtual ICollection<User> UpdatedUsers { get; set; }
        public virtual ICollection<Role> UpdatedRoles { get; set; }
        public virtual ICollection<UserRole> UpdatedUserRoles { get; set; }
        public virtual ICollection<Notification> UpdatedNotifications { get; set; }

        public virtual ICollection<Core.Action> UpdatedActions { get; set; }
        public virtual ICollection<Area> UpdatedAreas { get; set; }
        public virtual ICollection<Controller> UpdatedControllers { get; set; }
        public virtual ICollection<ControllerAction> UpdatedControllerActions { get; set; }
        public virtual ICollection<ControllerActionRole> UpdatedControllerActionRoles { get; set; }

        #endregion
    }
}
