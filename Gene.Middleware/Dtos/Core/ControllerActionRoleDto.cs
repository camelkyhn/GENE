using System;
using Gene.Middleware.Bases;
using System.ComponentModel.DataAnnotations;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Dtos.Core
{
    public class ControllerActionRoleDto : BaseDto<Guid?>
    {
        [Required]
        [Display(Name = Labels.ControllerAction)]
        public Guid? ControllerActionId { get; set; }
        [Required]
        [Display(Name = Labels.Role)]
        public Guid? RoleId { get; set; }
    }
}
