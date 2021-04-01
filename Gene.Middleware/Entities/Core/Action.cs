using System;
using Gene.Middleware.Bases;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Core
{
    [Table(nameof(Action))]
    public class Action : Entity<Guid?>
    {
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.Name)]
        public string Name { get; set; }

        public virtual ICollection<ControllerAction> ControllerActions { get; set; }
    }
}
