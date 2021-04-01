using System;
using Gene.Middleware.Bases;
using Gene.Middleware.Entities.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Identity
{
    [Table(nameof(Role))]
    public class Role : Entity<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.LongText, MinimumLength = MinLengths.LongText)]
        [Display(Name                                    = Labels.Name)]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<ControllerActionRole> ControllerActionRoles { get; set; }
    }
}
