using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gene.Middleware.Bases;
using Gene.Middleware.Constants;

namespace Gene.Middleware.Entities.Core
{
    [Table(nameof(Controller))]
    public class Controller : Entity<Guid?>
    {
        [Display(Name = Labels.Area)]
        public Guid? AreaId { get; set; }
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.Name)]
        public string Name { get; set; }
        [Required]
        [StringLength(MaxLengths.Text, MinimumLength = MinLengths.Text)]
        [Display(Name                                = Labels.DisplayName)]
        public string DisplayName { get; set; }
        [StringLength(MaxLengths.ShortText, MinimumLength = MinLengths.ShortText)]
        [Display(Name                                     = Labels.IconText)]
        public string IconText { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<ControllerAction> ControllerActions { get; set; }
    }
}
