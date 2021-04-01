using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Identity
{
    public class UserRoleDto : BaseDto<Guid?>
    {
        [Required]
        [Display(Name = Labels.User)]
        public Guid? UserId { get; set; }
        [Required]
        [Display(Name = Labels.Role)]
        public Guid? RoleId { get; set; }
    }
}
