using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Core
{
    [Table(nameof(ControllerAction))]
    public class ControllerAction : Entity<Guid?>
    {
        [Required]
        [Display(Name = Labels.Action)]
        public Guid? ActionId { get; set; }
        [Required]
        [Display(Name = Labels.Controller)]
        public Guid? ControllerId { get; set; }

        public virtual Action Action { get; set; }
        public virtual Controller Controller { get; set; }
        public virtual ICollection<ControllerActionRole> ControllerActionRoles { get; set; }
    }
}
