using System;
using Gene.Middleware.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Core
{
    [Table(nameof(ControllerActionRole))]
    public class ControllerActionRole : Entity<Guid?>
    {
        [Required]
        [Display(Name = Labels.ControllerAction)]
        public Guid? ControllerActionId { get; set; }
        [Required]
        [Display(Name = Labels.Role)]
        public Guid? RoleId { get; set; }

        public virtual ControllerAction ControllerAction { get; set; }
        public virtual Role Role { get; set; }
    }
}
